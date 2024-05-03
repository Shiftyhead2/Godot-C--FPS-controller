using Godot;
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

	private bool dragging = false;


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
		UpdateInventory();
		GlobalSignalBus.instance.OnInventoryInteracted += OnInventoryInteracted;
		GlobalSignalBus.instance.OnEndDrag += UpdateInventory;

	}


	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event.IsActionPressed("add item1") && !dragging)
		{
			LoadItem(Item1StringPath);
		}

		if (@event.IsActionPressed("add item2") && !dragging)
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

		//Stacking item
		foreach (var slot in ItemSlots.Values)
		{

			if (!itemToAdd.Stackable)
			{
				break;
			}

			if (remainingQuantity <= 0)
			{
				return;
			}


			if (slot.Item == null)
			{
				continue;
			}

			if (slot.Item.ID == itemToAdd.ID && slot.CurrentStack < itemToAdd.MaxStackSize)
			{
				remainingQuantity = slot.StackItem(remainingQuantity);
			}

		}

		//Adding items
		foreach (var slot in ItemSlots.Values)
		{
			if (remainingQuantity <= 0)
			{
				return;
			}

			if (slot.Item == null)
			{
				remainingQuantity = slot.AddItem(itemToAdd, remainingQuantity);
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
			UpdateInventory();
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


	private void StartDrag(int index)
	{
		if (ItemSlots.TryGetValue(index, out ItemSlot slot))
		{
			if (slot.Item == null)
			{
				GD.Print("Cannot drag an empty item!");
				dragging = false;
				return;
			}

			GlobalSignalBus.instance.EmitSignal(GlobalSignalBus.SignalName.OnItemSlotDragged, slot, index);
			GlobalSignalBus.instance.EmitSignal(GlobalSignalBus.SignalName.OnStartedDragging);
			dragging = true;
			slot.RemoveItem();
			UpdateInventory();
		}
	}

	private void UpdateInventory()
	{
		GlobalSignalBus.instance.EmitSignal(GlobalSignalBus.SignalName.OnInventoryUpdated, this);
	}

	private void EndDrag(int index)
	{
		ItemSlot draggedItem = GlobalSignalBus.instance.OnGetItemSlot?.Invoke();

		if (draggedItem == null)
		{
			GD.PushError($"Dragged item is empty for some reason");
			return;
		}
		int draggedSlotIndex = (int)GlobalSignalBus.instance.OnGetSlotIndex?.Invoke();

		if (ItemSlots.TryGetValue(index, out ItemSlot slot))
		{
			if (slot.Item == null)
			{
				slot.AddDroppedItem(draggedItem.Item, draggedItem.CurrentStack);

			}
			else
			{
				if (slot.Item.ID == draggedItem.Item.ID)
				{
					if (draggedItem.CurrentStack < slot.Item.MaxStackSize)
					{
						draggedItem.CurrentStack = slot.MergeDroppedItem(draggedItem.CurrentStack);
						if (draggedItem.CurrentStack > 0)
						{
							ItemSlots[draggedSlotIndex].AddItem(draggedItem.Item, draggedItem.CurrentStack);
						}
					}
					else
					{
						if (ItemSlots.TryGetValue(draggedSlotIndex, out ItemSlot draggedSlot))
						{
							if (draggedSlot.Item == null)
							{
								draggedSlot.AddItem(draggedItem.Item, draggedItem.CurrentStack);
							}
						}
					}
				}
				else
				{
					SwapItems(slot, ItemSlots[draggedSlotIndex], slot.Item, draggedItem.Item, draggedItem.CurrentStack);
				}
			}
		}
		dragging = false;
		GlobalSignalBus.instance.EmitSignal(GlobalSignalBus.SignalName.OnEndDrag);
	}

	private void SwapItems(ItemSlot itemSlot1, ItemSlot itemSlot2, Item item1, Item item2, int item2Stack)
	{
		itemSlot2.AddDroppedItem(item1, itemSlot1.CurrentStack);
		itemSlot1.AddDroppedItem(item2, item2Stack);
	}

	private void OnInventoryInteracted(int index, long button_index, bool isDoubleClick)
	{
		GD.Print($"Inventory interacted. Item slot:{ItemSlots[index].Item}, mouse button: {button_index}, double click: {isDoubleClick}");

		switch (button_index)
		{
			case 1:
				if (!dragging && isDoubleClick)
				{
					StartDrag(index);
				}
				else if (dragging)
				{
					EndDrag(index);
				}
				break;
			case 2:
				if (!dragging)
				{
					RemoveItemAtSlot(index);
				}
				break;
			default:
				break;
		}
	}
}
