using Godot;
using System;

public class ItemSlot
{
  public Item Item { get; private set; }
  public int CurrentStack { get; private set; } = 0;

  public ItemSlot(Item item, int stack)
  {
    Item = item;
    CurrentStack = stack;
  }


  public int AddItem(Item item, int Quantity)
  {
    Item = item;
    int availableSpace = item.MaxStackSize - CurrentStack;
    int addedQuantity = Mathf.Min(Quantity, availableSpace);

    CurrentStack += addedQuantity;
    int remainingQuantity = Quantity - addedQuantity;
    return remainingQuantity;
  }

  public int StackItem(int Quantity)
  {
    int remainingQuantity = Quantity;
    while (CurrentStack < Item.MaxStackSize && remainingQuantity > 0)
    {
      CurrentStack++;
      remainingQuantity--;
    }

    return remainingQuantity;
  }

  public void DecreaseStack()
  {
    CurrentStack = Mathf.Max(0, CurrentStack - 1);
    if (CurrentStack <= 0)
    {
      Item = null;
    }
  }
}
