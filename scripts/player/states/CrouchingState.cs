using Godot;
using System;

public partial class CrouchingState : PlayerMovementState
{


	[Export]
	private float Acceleration = 0.1f;

	[Export]
	private float Decceleration = 0.25f;

	[Export]
	public float speed = 3.0f;

	[Export]
	private float BobSpeed = 8f;
	[Export]
	private float BobAmount = 0.025f;

	private float HeadYPos = 0.485f;

	[Export]
	private ShapeCast3D CrouchShapeCast;

	[Export]
	private float crouch_speed = 4.0f;

	public override void Enter()
	{
		Player.defaultYPos = HeadYPos;
		Player.animationPlayer.Play("crouch", -1.0f, crouch_speed);
	}


	public override void Update(float delta)
	{
		Player.HandleGravity(delta);
		Player.HandleInput(speed, Acceleration, Decceleration);
		Player.HandleHeadBob(delta, BobSpeed, BobAmount);
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
				await ToSignal(Player.animationPlayer, "animation_finished");
			}

			if (Player.Velocity.Length() == 0.0f)
			{
				EmitSignal(SignalName.TransitionState, "IdleState");
			}
			else if (Player.Velocity.Length() > 0.0f)
			{
				EmitSignal(SignalName.TransitionState, "WalkingState");
			}

		}
		else if (CrouchShapeCast.IsColliding() == true)
		{
			await ToSignal(GetTree().CreateTimer(0.1), "timeout");
			uncrouch();
		}
	}
}
