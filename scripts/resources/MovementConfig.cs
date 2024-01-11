using Godot;
using System;

public partial class MovementConfig : Resource
{
  [Export]
  public float Acceleration = 0.1f;

  [Export]
  public float Decceleration = 0.25f;

  [Export]
  public float speed = 5f;
}
