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
    CurrentStack += 1;
    int remainingQuantity = Quantity - CurrentStack;
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

  public void RemoveItem(Item item)
  {
    GD.Print($"Attempting to remove an item: {item.Name}");
  }
}
