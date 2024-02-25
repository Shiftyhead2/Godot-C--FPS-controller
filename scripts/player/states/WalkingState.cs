using Godot;

public partial class WalkingState : PlayerMovementState
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
			if (Input.IsActionPressed("sprint"))
			{
				EmitSignal(nameof(TransitionState), "SprintingState");
			}

			if (Input.IsActionJustPressed("crouch"))
			{
				EmitSignal(nameof(TransitionState), "CrouchingState");
			}

		}

		if (Player.Velocity.Length() == 0.0f)
		{
			EmitSignal(nameof(TransitionState), "IdleState");
		}
	}
}
