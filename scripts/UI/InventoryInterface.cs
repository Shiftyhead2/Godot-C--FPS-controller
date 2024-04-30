using Godot;

public partial class InventoryInterface : Control
{

	private bool dragging = false;

	public override void _Ready()
	{
		if (GlobalSignalBus.instance != null)
		{
			GlobalSignalBus.instance.OnStartedDragging += StartDrag;
			GlobalSignalBus.instance.OnEndDrag += EndDrag;
		}
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event.IsActionPressed("open inventory") && !dragging)
		{
			openInventory();
		}
	}

	private void openInventory()
	{
		Visible = !Visible;
		if (Visible)
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		else
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
	}

	private void StartDrag()
	{
		dragging = true;
	}

	private void EndDrag()
	{
		dragging = false;
	}


}
