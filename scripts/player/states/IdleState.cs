using Godot;


public partial class IdleState : PlayerMovementState
{
	public override void Enter()
	{
		Player.defaultYPos = headBobConfig.HeadYPos;
	}

	public override void Update(float delta)
	{

		Player.HandleGravity(delta);
		Player.HandleInput(movementConfig.speed, movementConfig.Acceleration, movementConfig.Decceleration);
		Player.HandleHeadBob(delta, headBobConfig.BobSpeed, headBobConfig.BobAmount);
		Player.HandleMovement();

		if (Player.IsOnFloor())
		{
			if (Player.Velocity.Length() > 0.0f)
			{
				EmitSignal(nameof(TransitionState), "WalkingState");
			}

			if (Input.IsActionJustPressed("crouch"))
			{
				EmitSignal(nameof(TransitionState), "CrouchingState");
			}
		}
	}
}
