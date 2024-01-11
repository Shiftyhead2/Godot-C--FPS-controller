using Godot;

public partial class SprintingState : PlayerMovementState
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
			if (Input.IsActionJustPressed("crouch"))
			{
				EmitSignal(nameof(TransitionState), "CrouchingState");
			}
		}

		if (Input.IsActionJustReleased("sprint"))
		{
			EmitSignal(nameof(TransitionState), "WalkingState");
		}
	}
}
