using Godot;
using System;

[GlobalClass]
public partial class Item : Resource
{
  [Export]
  public int ID { get; private set; }
  [Export]
  public bool Stackable { get; private set; }
  [Export]
  public string Name { get; private set; }
  [Export]
  public string Description { get; private set; }
  [Export]
  public Texture2D Sprite { get; private set; }
  [Export]
  public int Quantity { get; private set; }
  [Export]
  public int MaxStackSize { get; private set; }

  public Item() : this(0, false, string.Empty, string.Empty, null, 0, 0) { }

  public Item(int id, bool stackable, string name, string description, Texture2D sprite, int quantity, int maxStackSize)
  {
    ID = id;
    Stackable = stackable;
    Name = name;
    Description = description;
    Sprite = sprite;
    Quantity = quantity;
    MaxStackSize = maxStackSize;
  }
}
