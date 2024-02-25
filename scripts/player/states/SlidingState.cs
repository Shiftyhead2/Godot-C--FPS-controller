using Godot;

public partial class SlidingState : PlayerMovementState
{

	[Export]
	private ShapeCast3D crouchingCast;

	[Export]
	private float tiltAmount = 0.09f;

	[Export(PropertyHint.Range, "1,6,0.1")]
	private float slideAnimSpeed = 4.0f;

	[Export]
	private float speed;


	public override void Enter(State previousState)
	{
		SetTilt();
		Player.animationPlayer.GetAnimation("sliding").TrackSetKeyValue(5, 0, Player.Velocity.Length());
		Player.animationPlayer.SpeedScale = 1f;
		Player.animationPlayer.Play("sliding", -1.0, slideAnimSpeed);
	}

	public override void Update(float delta)
	{
		Player.HandleGravity(delta);
		Player.HandleMovement();
	}

	private void SetTilt()
	{
		Vector3 tilt = Vector3.Zero;
		tilt.Z = 0.05f;
		Player.animationPlayer.GetAnimation("sliding").TrackSetKeyValue(6, 1, tilt);
		Player.animationPlayer.GetAnimation("sliding").TrackSetKeyValue(6, 2, tilt);
	}

	private void finish()
	{
		Player.camera.Rotation = Vector3.Zero;
		EmitSignal(nameof(TransitionState), "CrouchingState");
	}
}
