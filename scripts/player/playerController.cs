using Godot;

public partial class playerController : CharacterBody3D
{
	public const float JUMP_VELOCITY = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

	[ExportCategory("Node References")]
	[Export]
	private Node3D head;

	[Export]
	public Camera3D camera { get; private set; }

	[Export]
	public AnimationPlayer animationPlayer;

	public float defaultYPos { get; set; } = 0f;
	private float bobTimer;

	private Vector3 direction;
	private Vector3 velocity;
	private Vector2 inputDir;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		defaultYPos = head.Position.Y;
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleJump();
	}

	public void HandleInput(float speed, float acceleration, float deceleration)
	{

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
			velocity.Y -= gravity * delta;
	}


	private void HandleJump()
	{
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
				defaultYPos + Mathf.Sin(bobTimer) * currentBobAmount,
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
		head.Position = new Vector3(head.Position.X, Mathf.Lerp(head.Position.Y, defaultYPos, 0.5f), head.Position.Z);
	}
}
