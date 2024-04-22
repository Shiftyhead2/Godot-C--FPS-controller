using Godot;

public partial class WalkingState : PlayerMovementState
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
			if (Input.IsActionPressed("sprint") && Player.CanSprint)
			{
				EmitSignal(nameof(TransitionState), "SprintingState");
			}

			if (Input.IsActionJustPressed("crouch") && Player.CanCrouch)
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
