using Godot;

public partial class SprintingState : PlayerMovementState
{
	[Export]
	private float Acceleration = 0.1f;

	[Export]
	private float Decceleration = 0.25f;

	[Export]
	private float BobSpeed = 15f;
	[Export]
	private float BobAmount = 0.065f;

	[Export]
	private float speed = 8f;


	private float HeadYPos = 0.585f;

	public override void Enter()
	{
		Player.defaultYPos = HeadYPos;
	}

	public override void Update(float delta)
	{
		Player.HandleGravity(delta);
		Player.HandleInput(speed, Acceleration, Decceleration);
		Player.HandleHeadBob(delta, BobSpeed, BobAmount);
		Player.HandleMovement();

		if (Player.IsOnFloor())
		{
			if (Input.IsActionJustPressed("crouch"))
			{
				EmitSignal(SignalName.TransitionState, "CrouchingState");
			}
		}

		if (Input.IsActionJustReleased("sprint"))
		{
			EmitSignal(SignalName.TransitionState, "WalkingState");
		}
	}
}
