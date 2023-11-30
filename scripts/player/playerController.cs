using Godot;

public partial class playerController : CharacterBody3D
{
	public const float SPEED = 5.0f;
	public const float JUMP_VELOCITY = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = 9.8f;

	[ExportCategory("Node References")]
	[Export]
	private Node3D head;
	[Export]
	private Camera3D camera;

	[ExportCategory("Look Variables")]
	[Export]
	private float Sensitivity = 0.03f;



	private Vector3 targetRotation;

	[Export] private float ActualCameraRotationSpeed;

	[ExportCategory("Head Bob Variables")]
	[Export] private float walkBobSpeed = 14f;
	[Export] private float walkBobAmount = 0.05f;

	private float defaultYPos = 0f;
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

		velocity = Velocity;



		HandleInput();
		HandleGravity((float)delta);
		HandleJump();
		HandleMovement((float)delta);

		Velocity = velocity;


		HandleHeadRotation((float)delta);
		HandleHeadBob((float)delta);
		MoveAndSlide();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		base._UnhandledInput(@event);

		if (@event is InputEventMouseMotion mouseMotion)
		{
			targetRotation = new Vector3(
				Mathf.Clamp((-1 * mouseMotion.Relative.Y * Sensitivity) + targetRotation.X, -60f, 60f),
				Mathf.Wrap((-1 * mouseMotion.Relative.X * Sensitivity) + targetRotation.Y, 0f, 360f),
				0);
		}
	}


	private void HandleInput()
	{
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		inputDir = Input.GetVector("move left", "move right", "move forward", "move backwards");
		direction = (head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
	}

	private void HandleGravity(float delta)
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


	private void HandleMovement(float delta)
	{


		if (IsOnFloor())
		{
			if (direction != Vector3.Zero)
			{
				velocity.X = direction.X * SPEED;
				velocity.Z = direction.Z * SPEED;
			}
			else
			{
				velocity.X = Mathf.Lerp(velocity.X, direction.X * SPEED, delta * 7.0f);
				velocity.Z = Mathf.Lerp(velocity.Z, direction.Z * SPEED, delta * 7.0f);
			}
		}
		else
		{
			velocity.X = Mathf.Lerp(velocity.X, direction.X * SPEED, delta * 3.0f);
			velocity.Z = Mathf.Lerp(velocity.Z, direction.Z * SPEED, delta * 3.0f);
		}



	}

	private void HandleHeadRotation(float delta)
	{
		head.Rotation = new Vector3(
			Mathf.LerpAngle(head.Rotation.X, Mathf.DegToRad(targetRotation.X), ActualCameraRotationSpeed * delta),
			Mathf.LerpAngle(head.Rotation.Y, Mathf.DegToRad(targetRotation.Y), ActualCameraRotationSpeed * delta),
			0
		);
	}

	private void HandleHeadBob(float delta)
	{
		if (!IsOnFloor())
		{
			ResetHeadPosition();
			return;
		}

		if (Mathf.Abs(direction.X) > 0.1f || Mathf.Abs(direction.Z) > 0.1f)
		{
			bobTimer += delta * walkBobSpeed;
			head.Position = new Vector3(
				head.Position.X,
				defaultYPos + Mathf.Sin(bobTimer) * walkBobAmount,
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
