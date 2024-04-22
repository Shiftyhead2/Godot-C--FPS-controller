using Godot;

public partial class CrouchingState : PlayerMovementState
{

	[Export]
	private ShapeCast3D CrouchShapeCast;

	[Export]
	private float crouch_speed = 4.0f;


	private bool released = false;

	public override void Enter(State previousState)
	{
		Player.AnimationPlayer.SpeedScale = 1f;
		if (previousState.Name != "SlidingState")
		{
			Player.AnimationPlayer.Play("crouch", -1.0f, crouch_speed);
		}
		else if (previousState.Name == "SlidingState")
		{
			Player.AnimationPlayer.CurrentAnimation = "crouch";
			Player.AnimationPlayer.Seek(1.0, true);
		}
		//Player.defaultYPos = headBobConfig.HeadYPos;

	}


	public override void Update(float delta)
	{
		Player.HandleGravity(delta);
		Player.HandleInput(MovementConfig.Speed, MovementConfig.Acceleration, MovementConfig.Decceleration);
		//Player.HandleHeadBob(delta, headBobConfig.BobSpeed, headBobConfig.BobAmount);
		Player.HandleMovement();

		if (Input.IsActionJustReleased("crouch") || !Player.CanCrouch)
		{
			uncrouch();
		}
		else if (Input.IsActionPressed("crouch") == false && released == false)
		{
			released = true;
			uncrouch();
		}
	}


	public async void uncrouch()
	{
		if (CrouchShapeCast.IsColliding() == false)
		{
			Player.AnimationPlayer.Play("crouch", -1.0, -crouch_speed * 1.5f, true);
			if (Player.AnimationPlayer.IsPlaying())
			{
				await ToSignal(Player.AnimationPlayer, AnimationMixer.SignalName.AnimationFinished);
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

	public override void Exit()
	{
		released = false;
	}
}
