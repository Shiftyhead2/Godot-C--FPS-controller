using System;
using Godot;

public partial class GlobalSignalBus : Node
{
	[Signal]
	public delegate void OnInventoryUpdatedEventHandler(Inventory inventory);
	[Signal]
	public delegate void OnStartedDraggingEventHandler();
	[Signal]
	public delegate void OnInventoryInteractedEventHandler(int index, long button_index, bool isDoubleClick);

	[Signal]
	public delegate void OnItemSlotDraggedEventHandler(ItemSlot itemSlot, int index);

	[Signal]
	public delegate void OnEndDragEventHandler();

	public Func<ItemSlot> OnGetItemSlot;
	public Func<int> OnGetSlotIndex;




	public static GlobalSignalBus instance { get; private set; }

	public override void _Ready()
	{
		instance = this;
	}

}
