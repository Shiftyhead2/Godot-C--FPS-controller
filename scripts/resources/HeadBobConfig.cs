using Godot;
using System;

public partial class HeadBobConfig : Resource
{
  [Export]
  public float BobSpeed { get; set; }
  [Export]
  public float BobAmount { get; set; }


  [Export]
  public float HeadYPos { get; set; }


  public HeadBobConfig() : this(0f, 0f, 0f) { }

  public HeadBobConfig(float bobSpeed, float bobAmount, float headYPos)
  {
    BobSpeed = bobSpeed;
    BobAmount = bobAmount;
    HeadYPos = headYPos;
  }

}
