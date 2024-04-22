using Godot;

public partial class playerController : CharacterBody3D
{
	public const float JUMP_VELOCITY = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float Gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

	[ExportCategory("Control Properties")]
	[Export]
	public bool CanMove { get; private set; } = true;
	[Export]
	public bool CanJump { get; private set; } = true;
	[Export]
	public bool CanSprint { get; private set; } = true;

	[Export]
	public bool CanCrouch { get; private set; } = true;
	[Export]
	public bool CanHeadBob { get; private set; } = true;

	[Export]
	public bool CanSlide { get; private set; } = true;



	[ExportCategory("Node References")]
	[Export]
	private Node3D head;

	[Export]
	public Camera3D Camera { get; private set; }

	[Export]
	public AnimationPlayer AnimationPlayer;

	public float DefaultYPos { get; set; } = 0f;
	private float bobTimer;

	private Vector3 direction;
	private Vector3 velocity;
	private Vector2 inputDir;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		DefaultYPos = head.Position.Y;
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleJump();
	}

	public void HandleInput(float speed, float acceleration, float deceleration)
	{

		if (!CanMove)
		{
			return;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		inputDir = Input.GetVector("move left", "move right", "move forward", "move backwards");
		direction = (head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			velocity.X = Mathf.Lerp(velocity.X, direction.X * speed, acceleration);
			velocity.Z = Mathf.Lerp(velocity.Z, direction.Z * speed, acceleration);
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, deceleration);
			velocity.Z = Mathf.MoveToward(velocity.Z, 0, deceleration);
		}


	}

	public void HandleGravity(float delta)
	{
		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= Gravity * delta;
	}


	private void HandleJump()
	{
		if (!CanJump)
		{
			return;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			velocity.Y = JUMP_VELOCITY;
	}


	public void HandleMovement()
	{

		Velocity = velocity;
		MoveAndSlide();

	}

	public void HandleHeadBob(float delta, float currentBobSpeed, float currentBobAmount)
	{

		if (!CanHeadBob)
		{
			return;
		}


		if (!IsOnFloor())
		{
			ResetHeadPosition();
			return;
		}


		if (Mathf.Abs(direction.X) > 0.1f || Mathf.Abs(direction.Z) > 0.1f)
		{

			bobTimer += delta * currentBobSpeed;
			head.Position = new Vector3(
				head.Position.X,
				DefaultYPos + Mathf.Sin(bobTimer) * currentBobAmount,
				head.Position.Z);
		}
		else
		{
			ResetHeadPosition();
		}
	}


	private void ResetHeadPosition()
	{
		bobTimer = 0f;
		head.Position = new Vector3(head.Position.X, Mathf.Lerp(head.Position.Y, DefaultYPos, 0.5f), head.Position.Z);
	}
}
