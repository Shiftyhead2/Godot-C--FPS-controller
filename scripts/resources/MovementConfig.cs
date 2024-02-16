using Godot;
using System;

public partial class MovementConfig : Resource
{
  [Export]
  public float Acceleration { get; set; }

  [Export]
  public float Decceleration { get; set; }

  [Export]
  public float Speed { get; set; }


  public MovementConfig() : this(0f, 0f, 0f) { }

  public MovementConfig(float acceleration, float decceleration, float speed)
  {
    Acceleration = acceleration;
    Decceleration = decceleration;
    Speed = speed;
  }

}
