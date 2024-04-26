using Godot;

public partial class GlobalSignalBus : Node
{
	[Signal]
	public delegate void OnInventoryUpdatedEventHandler(Inventory inventory);


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
