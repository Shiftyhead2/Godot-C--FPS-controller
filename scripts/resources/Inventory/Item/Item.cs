using Godot;
using System;

[GlobalClass]
public partial class Item : Resource
{
  [Export]
  public int ID { get; private set; }
  [Export]
  public string Name { get; private set; }
  [Export]
  public string Description { get; private set; }
  [Export]
  public int Quantity { get; private set; }
  [Export]
  public int MaxStackSize { get; private set; }

  public Item() : this(0, string.Empty, string.Empty, 0, 0) { }

  public Item(int id, string name, string description, int quantity, int maxStackSize)
  {
    ID = id;
    Name = name;
    Description = description;
    Quantity = quantity;
    MaxStackSize = maxStackSize;
  }
}
