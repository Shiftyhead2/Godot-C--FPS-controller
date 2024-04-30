using Godot;

public partial class DropSlotUI : PanelContainer
{
	private ItemSlot heldItemSlot;
	private int slotIndex;

	[Export]
	private TextureRect itemTexture;

	[Export]
	private Label itemQuantityLabel;

	[Export]
	private Vector2 offset;


	public override void _Ready()
	{
		if (GlobalSignalBus.instance != null)
		{
			GlobalSignalBus.instance.OnItemSlotDragged += SetUpDropSlotUI;
			GlobalSignalBus.instance.OnGetItemSlot += ReturnItemSlot;
			GlobalSignalBus.instance.OnGetSlotIndex += ReturnSlotIndex;
			GlobalSignalBus.instance.OnEndDrag += ResetDrag;
		}
	}

	public override void _ExitTree()
	{
		GlobalSignalBus.instance.OnGetItemSlot -= ReturnItemSlot;
		GlobalSignalBus.instance.OnGetSlotIndex -= ReturnSlotIndex;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (heldItemSlot != null)
		{
			Visible = true;
			Position = GetGlobalMousePosition() + offset;
		}
		else
		{
			Visible = false;
		}
	}

	private void SetUpDropSlotUI(ItemSlot itemSlot, int index)
	{
		if (itemSlot != null)
		{
			heldItemSlot = itemSlot.Duplicate() as ItemSlot;
			heldItemSlot.AddDroppedItem(itemSlot.Item, itemSlot.CurrentStack);
			slotIndex = index;
			if (heldItemSlot.Item != null)
			{
				itemTexture.Texture = heldItemSlot.Item.Sprite;
			}

		}

		if (heldItemSlot.CurrentStack > 1)
		{
			itemQuantityLabel.Visible = true;
			itemQuantityLabel.Text = $"{heldItemSlot.CurrentStack}";
		}
		else
		{
			itemQuantityLabel.Visible = false;
		}

	}

	private ItemSlot ReturnItemSlot()
	{
		if (heldItemSlot == null)
		{
			return null;
		}

		return heldItemSlot;
	}

	private int ReturnSlotIndex()
	{
		return slotIndex;
	}

	private void ResetDrag()
	{
		heldItemSlot = null;
		slotIndex = 0;
	}
}
