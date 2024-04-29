using Godot;

public partial class GlobalSignalBus : Node
{
	[Signal]
	public delegate void OnInventoryUpdatedEventHandler(Inventory inventory);
	[Signal]
	public delegate void OnInventoryInteractedEventHandler(int index, long button_index);


	public static GlobalSignalBus instance { get; private set; }

	public override void _Ready()
	{
		if (instance != null)
		{
			QueueFree();
			return;
		}
		instance = this;
	}

}
