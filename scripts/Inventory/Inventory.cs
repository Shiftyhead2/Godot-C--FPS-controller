using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Node
{


	public List<ItemSlot> ItemSlots { get; private set; } = new List<ItemSlot>();

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
			ItemSlots.Add(itemSlot);
		}

		if (GlobalSignalBus.instance == null)
		{
			GD.PrintErr($"{Name}: Error:GlobalSignalBus singleton couldn't be found!");
		}
		GlobalSignalBus.instance.EmitSignal(nameof(GlobalSignalBus.instance.OnInventoryUpdated), this);

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

		GlobalSignalBus.instance.EmitSignal(nameof(GlobalSignalBus.instance.OnInventoryUpdated), this);

	}

	public void AddItemToSlots(Item itemToAdd)
	{

		if (CheckIfInventoryFull())
		{
			GD.Print("Inventory full!");
			return;
		}

		int remainingQuantity = itemToAdd.Quantity;

		foreach (var slot in ItemSlots)
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

	private bool CheckIfInventoryFull()
	{
		int emptySlots = 0;
		foreach (var slot in ItemSlots)
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
}
