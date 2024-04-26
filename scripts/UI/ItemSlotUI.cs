using Godot;
using System;

public partial class ItemSlotUI : PanelContainer
{

	[Export]
	private TextureRect itemTexture;

	[Export]
	private Label itemQuantityLabel;

	public void SetUpItemSlotUI(ItemSlot itemSlot)
	{

		if (itemSlot.Item != null)
		{
			itemTexture.Texture = itemSlot.Item.Sprite;
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
}
