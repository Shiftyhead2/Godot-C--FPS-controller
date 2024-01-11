using Godot;

public partial class CrouchingState : PlayerMovementState
{

	[Export]
	private ShapeCast3D CrouchShapeCast;

	[Export]
	private float crouch_speed = 4.0f;

	public override void Enter()
	{
		Player.defaultYPos = headBobConfig.HeadYPos;
		Player.animationPlayer.Play("crouch", -1.0f, crouch_speed);
	}


	public override void Update(float delta)
	{
		Player.HandleGravity(delta);
		Player.HandleInput(movementConfig.speed, movementConfig.Acceleration, movementConfig.Decceleration);
		Player.HandleHeadBob(delta, headBobConfig.BobSpeed, headBobConfig.BobAmount);
		Player.HandleMovement();

		if (Input.IsActionJustReleased("crouch"))
		{
			uncrouch();
		}
	}


	public async void uncrouch()
	{
		if (CrouchShapeCast.IsColliding() == false && Input.IsActionPressed("crouch") == false)
		{
			Player.animationPlayer.Play("crouch", -1.0, -crouch_speed * 1.5f, true);
			if (Player.animationPlayer.IsPlaying())
			{
				await ToSignal(Player.animationPlayer, AnimationMixer.SignalName.AnimationFinished);
			}

			if (Player.Velocity.Length() == 0.0f)
			{
				EmitSignal(nameof(TransitionState), "IdleState");
			}
			else if (Player.Velocity.Length() > 0.0f)
			{
				EmitSignal(nameof(TransitionState), "WalkingState");
			}

		}
		else if (CrouchShapeCast.IsColliding() == true)
		{
			await ToSignal(GetTree().CreateTimer(0.1), Timer.SignalName.Timeout);
			uncrouch();
		}
	}
}
