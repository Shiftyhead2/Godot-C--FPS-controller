using Godot;
using System;

public partial class InventoryUI : PanelContainer
{

	[Export]
	private PackedScene ItemSlot;

	[Export]
	private GridContainer gridContainer;



	public override void _Ready()
	{
		if (GlobalSignalBus.instance == null)
		{
			GD.PrintErr($"{Name}: Error:GlobalSignalBus singleton couldn't be found!");
		}
		GlobalSignalBus.instance.OnInventoryUpdated += UpdateInventory;
	}


	public void UpdateInventory(Inventory inventory)
	{
		foreach (var child in gridContainer.GetChildren())
		{
			child.QueueFree();
		}

		foreach (var slot in inventory.ItemSlots.Values)
		{
			ItemSlotUI itemSlotUI = ItemSlot.Instantiate() as ItemSlotUI;
			gridContainer.AddChild(itemSlotUI);

			itemSlotUI.ItemSlotUIInteracted += InventoryInteract;

			if (slot != null)
			{
				itemSlotUI.SetUpItemSlotUI(slot);
			}
		}
	}

	private void InventoryInteract(int index, long button_index)
	{
		GlobalSignalBus.instance.EmitSignal(GlobalSignalBus.SignalName.OnInventoryInteracted, index, button_index);
	}

}
