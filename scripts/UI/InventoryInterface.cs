using Godot;
using System;

public partial class InventoryInterface : Control
{

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event.IsActionPressed("open inventory"))
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



}
