using Godot;

public partial class SprintingState : PlayerMovementState
{

	public override void Enter(State previousState)
	{
		Player.defaultYPos = headBobConfig.HeadYPos;
	}

	public override void Update(float delta)
	{
		Player.HandleGravity(delta);
		Player.HandleInput(movementConfig.Speed, movementConfig.Acceleration, movementConfig.Decceleration);
		Player.HandleHeadBob(delta, headBobConfig.BobSpeed, headBobConfig.BobAmount);
		Player.HandleMovement();

		if (Player.IsOnFloor())
		{
			if (Input.IsActionJustPressed("crouch") && Player.Velocity.Length() > 0.0f)
			{
				EmitSignal(nameof(TransitionState), "SlidingState");
			}
		}

		if (Input.IsActionJustReleased("sprint"))
		{
			EmitSignal(nameof(TransitionState), "WalkingState");
		}

		if (Player.Velocity.Length() == 0.0f)
		{
			EmitSignal(nameof(TransitionState), "IdleState");
		}
	}
}
