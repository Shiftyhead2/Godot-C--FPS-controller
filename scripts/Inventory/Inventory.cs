using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Node
{


	public Dictionary<int, ItemSlot> ItemSlots { get; private set; } = new Dictionary<int, ItemSlot>();


	[Export]
	public int InventorySize { get; private set; } = 24;

	[Export]
	private string Item1StringPath;

	[Export]
	private string Item2StringPath;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		for (int i = 0; i < InventorySize; i++)
		{
			ItemSlot itemSlot = new ItemSlot(null, 0);
			ItemSlots[i] = itemSlot;
		}

		if (GlobalSignalBus.instance == null)
		{
			GD.PrintErr($"{Name}: Error:GlobalSignalBus singleton couldn't be found!");
		}
		GlobalSignalBus.instance.EmitSignal(nameof(GlobalSignalBus.instance.OnInventoryUpdated), this);
		GlobalSignalBus.instance.OnInventoryInteracted += OnInventoryInteracted;

	}


	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event.IsActionPressed("add item1"))
		{
			LoadItem(Item1StringPath);
		}

		if (@event.IsActionPressed("add item2"))
		{
			LoadItem(Item2StringPath);
		}
	}

	public void LoadItem(string filePath)
	{
		if (filePath == null || filePath == string.Empty)
		{
			GD.PrintErr("File path cannot be empty!");
			return;
		}

		if (ItemSlots == null)
		{
			GD.PrintErr("Items slots are null for some reason!");
			return;
		}

		if (CheckIfInventoryFull())
		{
			GD.Print("Inventory full!");
			return;
		}

		Item itemToAdd = ResourceLoader.Load<Item>(filePath);
		if (itemToAdd == null)
		{
			GD.PrintErr($"Failed to load item at file path: {filePath}. Either the item does not exist or the path is incorrect!");
			return;
		}

		AddItemToSlots(itemToAdd);

		GlobalSignalBus.instance.EmitSignal(GlobalSignalBus.SignalName.OnInventoryUpdated, this);

	}

	public void AddItemToSlots(Item itemToAdd)
	{

		if (itemToAdd == null)
		{
			GD.PrintErr("For some reason this item doesn't exist!");
			return;
		}

		if (CheckIfInventoryFull())
		{
			GD.Print("Inventory full!");
			return;
		}

		int remainingQuantity = itemToAdd.Quantity;

		foreach (var slot in ItemSlots.Values)
		{
			if (slot.Item == null)
			{
				remainingQuantity = slot.AddItem(itemToAdd, remainingQuantity);
			}

			if (slot.Item.ID == itemToAdd.ID && slot.CurrentStack < itemToAdd.MaxStackSize)
			{
				remainingQuantity = slot.StackItem(remainingQuantity);
			}

			if (remainingQuantity <= 0)
			{
				break;
			}
		}

	}

	private void RemoveItemAtSlot(int index)
	{
		if (ItemSlots.TryGetValue(index, out ItemSlot slot))
		{
			if (slot.Item == null)
			{
				GD.Print($"Cannot remove item at slot {index} because it has no items!");
				return;
			}

			slot.DecreaseStack();
			GlobalSignalBus.instance.EmitSignal(GlobalSignalBus.SignalName.OnInventoryUpdated, this);
		}
	}

	private bool CheckIfInventoryFull()
	{
		int emptySlots = 0;
		foreach (var slot in ItemSlots.Values)
		{
			if (slot.Item == null || slot.CurrentStack < slot.Item.MaxStackSize)
			{
				emptySlots++;
			}
		}

		if (emptySlots > 0)
		{
			return false;
		}

		return true;
	}

	private void OnInventoryInteracted(int index, long button_index)
	{
		GD.Print($"Inventory interacted. Item slot:{ItemSlots[index].Item}, mouse button: {button_index}");

		switch (button_index)
		{
			case 1:
				break;
			case 2:
				RemoveItemAtSlot(index);
				break;
			default:
				break;
		}
	}
}
