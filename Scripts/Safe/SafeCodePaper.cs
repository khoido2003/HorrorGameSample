using System;
using Godot;

public partial class SafeCodePaper : RigidBody3D
{
    [Export]
    private MeshInstance3D meshInstance;

    [Export]
    private Godot.Collections.Array<Node3D> spawnPosList;

    private TextMesh textMesh;
    private bool isAttached = false;

    public override void _Ready()
    {
        // Enable collision detection
        ContactMonitor = true;
        MaxContactsReported = 5;

        // Get TextMesh
        textMesh = meshInstance.Mesh as TextMesh;

        if (textMesh == null)
        {
            GD.PrintErr("Mesh is not a TextMesh!");
        }

        // Delay spawn to avoid tree errors
        CallDeferred(nameof(SetRandomSpawnPosition));
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Freeze || isAttached)
            return;

        // Wait until object is almost still
        if (LinearVelocity.Length() < 0.1f)
        {
            var bodies = GetCollidingBodies();

            foreach (var body in bodies)
            {
                if (body is Node node)
                {
                    Node3D drawer = FindDrawer(node);

                    if (drawer != null)
                    {
                        isAttached = true;
                        AttachToDrawer(drawer);
                        break;
                    }
                }
            }
        }
    }

    private Node3D FindDrawer(Node node)
    {
        while (node != null)
        {
            if (node is Drawer drawer)
            {
                return drawer as Node3D;
            }

            node = node.GetParent();
        }

        return null;
    }

    private void AttachToDrawer(Node3D drawerRoot)
    {
        if (drawerRoot is not Drawer drawer)
            return;

        Node3D target = drawer.MovingPart;

        if (target == null)
        {
            GD.PrintErr("Drawer has no MovingPart assigned!");
            return;
        }

        Transform3D global = GlobalTransform;

        Freeze = true;

        Reparent(target);
        GlobalTransform = global;

        GD.Print("Attached to moving part: " + target.Name);
    }

    private void SetRandomSpawnPosition()
    {
        if (spawnPosList == null || spawnPosList.Count == 0)
        {
            GD.PrintErr("No spawn positions assigned!");
            return;
        }

        Random random = new Random();
        int index = random.Next(spawnPosList.Count);

        Node3D chosenSpawn = spawnPosList[index];

        GlobalTransform = chosenSpawn.GlobalTransform;
    }

    public void SetPassword(string password)
    {
        if (textMesh == null)
        {
            textMesh = meshInstance.Mesh as TextMesh;
        }

        if (textMesh != null)
        {
            textMesh.Text = password;
        }
        else
        {
            GD.PrintErr("TextMesh is still null!");
        }
    }
}
