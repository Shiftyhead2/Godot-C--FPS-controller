using Godot;

public partial class SprintingState : PlayerMovementState
{

	public override void Enter(State previousState)
	{
		Player.DefaultYPos = HeadBobConfig.HeadYPos;
	}

	public override void Update(float delta)
	{
		Player.HandleGravity(delta);
		Player.HandleInput(MovementConfig.Speed, MovementConfig.Acceleration, MovementConfig.Decceleration);
		Player.HandleHeadBob(delta, HeadBobConfig.BobSpeed, HeadBobConfig.BobAmount);
		Player.HandleMovement();

		if (Player.IsOnFloor())
		{
			if (Input.IsActionJustPressed("crouch") && Player.Velocity.Length() > 0.0f && Player.CanSlide)
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
