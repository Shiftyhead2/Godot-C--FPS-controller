using Godot;

public partial class ItemSlotUI : PanelContainer
{
	[Signal]
	public delegate void ItemSlotUIInteractedEventHandler(int index, long button_index, bool isDoubleClick);

	[Export]
	private TextureRect itemTexture;

	[Export]
	private Label itemQuantityLabel;

	[Export]
	private Theme normalTheme;

	[Export]
	private Theme hoverTheme;

	public void SetUpItemSlotUI(ItemSlot itemSlot)
	{

		if (itemSlot.Item != null)
		{
			itemTexture.Texture = itemSlot.Item.Sprite;
			TooltipText = $"{itemSlot.Item.Name}\n{itemSlot.Item.Description}";
		}
		if (itemSlot.CurrentStack > 1)
		{
			itemQuantityLabel.Visible = true;
			itemQuantityLabel.Text = $"{itemSlot.CurrentStack}";
		}
		else
		{
			itemQuantityLabel.Visible = false;
		}
	}

	public void OnGuiInput(InputEvent @event)
	{

		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.IsPressed())
			{
				if (mouseButton.ButtonIndex == MouseButton.Left)
				{
					EmitSignal(SignalName.ItemSlotUIInteracted, GetIndex(), (long)mouseButton.ButtonIndex, mouseButton.DoubleClick);
				}
				else if (mouseButton.ButtonIndex == MouseButton.Right)
				{
					EmitSignal(SignalName.ItemSlotUIInteracted, GetIndex(), (long)mouseButton.ButtonIndex, mouseButton.DoubleClick);
				}
			}
		}
	}

	public void OnMouseEntered()
	{
		Theme = hoverTheme;
	}

	public void OnMouseExited()
	{
		Theme = normalTheme;
	}
}
