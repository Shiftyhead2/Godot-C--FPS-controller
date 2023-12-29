using Godot;


public partial class IdleState : PlayerMovementState
{

	[Export]
	private float Acceleration = 0.1f;

	[Export]
	private float Decceleration = 0.25f;

	[Export]
	private float BobSpeed = 14f;
	[Export]
	private float BobAmount = 0.05f;

	[Export]
	private float speed = 5f;

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
