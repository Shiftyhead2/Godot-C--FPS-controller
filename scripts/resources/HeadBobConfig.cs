using Godot;
using System;

public partial class HeadBobConfig : Resource
{
  [Export]
  public float BobSpeed = 14f;
  [Export]
  public float BobAmount = 0.05f;


  [Export]
  public float HeadYPos = 0.585f;


}
