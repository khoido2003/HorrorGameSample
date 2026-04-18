using System.Collections.Generic;
using Godot;

public partial class PlayerInventory : Node
{
    private List<(ItemResource resource, int amount)> items = new();
    private int heldIndex = -1; // -1 = empty hand

    [Signal]
    public delegate void InventoryChangedEventHandler();

    [Signal]
    public delegate void HeldItemChangedEventHandler(ItemResource item); // null = empty hand

    // ── Inventory ──────────────────────────────────────────────

    public void AddItem(ItemResource item, int amount = 1)
    {
        if (item == null)
            return;

        int existing = items.FindIndex(e => e.resource.Id == item.Id);
        if (existing >= 0)
        {
            var entry = items[existing];
            entry.amount += amount;
            items[existing] = entry;
        }
        else
        {
            items.Add((item, amount));
        }

        // Auto-equip only if hand is empty
        if (heldIndex == -1)
        {
            int newIdx = items.FindIndex(e => e.resource.Id == item.Id);
            SetHeldIndex(newIdx);
        }

        EmitSignal(SignalName.InventoryChanged);
    }

    public bool HasItem(ItemResource item, int amount = 1)
    {
        if (item == null)
        {
            return false;
        }

        int idx = items.FindIndex(e => e.resource.Id == item.Id);

        return idx >= 0 && items[idx].amount >= amount;
    }

    public bool RemoveItem(ItemResource item, int amount = 1)
    {
        if (!HasItem(item, amount))
        {
            return false;
        }

        int idx = items.FindIndex(e => e.resource.Id == item.Id);
        var entry = items[idx];
        entry.amount -= amount;

        if (entry.amount <= 0)
        {
            items.RemoveAt(idx);

            // If the held item was removed, fix the held index
            if (heldIndex == idx)
            {
                heldIndex = -1;

                EmitSignal(SignalName.HeldItemChanged, default(Variant)); // null item
            }
            else if (heldIndex > idx)
            {
                heldIndex--; // shift down to stay pointing at same item
            }
        }
        else
        {
            items[idx] = entry;
        }

        EmitSignal(SignalName.InventoryChanged);
        return true;
    }

    // ── Held item ──────────────────────────────────────────────

    public ItemResource GetHeldItem()
    {
        if (heldIndex < 0 || heldIndex >= items.Count)
        {
            return null;
        }

        return items[heldIndex].resource;
    }

    /// <summary>Equip item at inventory slot index. Pass -1 to unequip.</summary>
    public void SetHeldIndex(int index)
    {
        if (index < -1 || index >= items.Count)
        {
            return;
        }

        if (index == heldIndex)
        {
            return;
        }

        heldIndex = index;
        EmitSignal(SignalName.HeldItemChanged, GetHeldItem());
    }

    /// <summary>Cycle through inventory slots with scroll wheel / hotkeys.</summary>
    public void CycleHeld(int direction)
    {
        if (items.Count == 0)
        {
            SetHeldIndex(-1);
            return;
        }

        // -1 → treat as "before slot 0" when cycling forward
        int next = heldIndex + direction;

        if (next >= items.Count)
        {
            next = -1; // wrap past end → empty hand
        }
        else if (next < -1)
        {
            next = items.Count - 1; // wrap before empty → last slot
        }

        SetHeldIndex(next);
    }

    // ── Interaction ────────────────────────────────────────────

    public bool TryInteractWith(Node target, Player player)
    {
        // Only the held item interacts
        var held = GetHeldItem();
        if (held != null && held.TryInteract(target, player))
        {
            return true;
        }

        return false;
    }

    public IReadOnlyList<(ItemResource resource, int amount)> GetItems() => items;

    public int HeldIndex => heldIndex;
}
