using Godot;

public partial class playerController : CharacterBody3D
{

	public const float CROUNCHING_SPEED = 3.0f;

	public const float WALKING_SPEED = 5.0f;
	public const float RUNNING_SPEED = 8.0F;
	public const float JUMP_VELOCITY = 4.5f;

	private bool isSprinting = false;

	private bool isCrouching = false;

	[ExportCategory("Hold Crouch Toggle")]
	[Export]
	private bool TOGGLE_CROUCH = true;


	private float current_speed;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = 9.8f;

	[ExportCategory("Node References")]
	[Export]
	private Node3D head;
	[Export]
	private Camera3D camera;

	[Export]
	private AnimationPlayer animationPlayer;

	[Export]
	private ShapeCast3D CrouchShapeCast;

	[ExportCategory("Look Variables")]
	[Export]
	private float Sensitivity = 0.03f;



	private Vector3 targetRotation;

	[Export]
	private float ActualCameraRotationSpeed;

	[ExportCategory("Head Bob Configuration")]
	[ExportGroup("Head bob Variables")]
	[ExportSubgroup("Walking variables")]
	[Export]
	private float walkBobSpeed = 14f;
	[Export]
	private float walkBobAmount = 0.05f;
	[ExportSubgroup("Running variables")]
	[Export]
	private float runBobSpeed = 15f;
	[Export]
	private float runBobAmount = 0.065f;

	[ExportSubgroup("Crouching variables")]
	[Export]
	private float crouchBobSpeed = 8f;
	[Export]
	private float crouchBobAmount = 0.025f;

	private float defaultYPos = 0f;
	private float bobTimer;

	private float crouchHeadYPos = 0.485f;
	private float standHeadYPos = 0.585f;

	private Vector3 direction;
	private Vector3 velocity;
	private Vector2 inputDir;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;

		CrouchShapeCast.AddException(this);

		standHeadYPos = head.Position.Y;
		current_speed = WALKING_SPEED;
	}

	public override void _PhysicsProcess(double delta)
	{

		velocity = Velocity;


		defaultYPos = isCrouching ? crouchHeadYPos : standHeadYPos;

		current_speed = isCrouching ? CROUNCHING_SPEED : isSprinting ? RUNNING_SPEED : WALKING_SPEED;


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

		if (@event.IsActionPressed("crouch") && IsOnFloor() && TOGGLE_CROUCH)
		{
			HandleToggleCrouch();
		}

		if (@event.IsActionPressed("crouch") && !isCrouching && IsOnFloor() && !TOGGLE_CROUCH)
		{
			crouching(true);
		}

		if (@event.IsActionReleased("crouch") && !TOGGLE_CROUCH)
		{
			crouching(false);
		}



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

		if (Input.IsActionPressed("sprint") && IsOnFloor() && !isCrouching)
		{
			HandleSprintInput();
		}
		else
		{
			isSprinting = false;
		}
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
				velocity.X = direction.X * current_speed;
				velocity.Z = direction.Z * current_speed;
			}
			else
			{
				velocity.X = Mathf.Lerp(velocity.X, direction.X * current_speed, delta * 7.0f);
				velocity.Z = Mathf.Lerp(velocity.Z, direction.Z * current_speed, delta * 7.0f);
			}
		}
		else
		{
			velocity.X = Mathf.Lerp(velocity.X, direction.X * current_speed, delta * 3.0f);
			velocity.Z = Mathf.Lerp(velocity.Z, direction.Z * current_speed, delta * 3.0f);
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
			bobTimer += delta * (isCrouching ? crouchBobSpeed : isSprinting ? runBobSpeed : walkBobSpeed);
			head.Position = new Vector3(
				head.Position.X,
				defaultYPos + Mathf.Sin(bobTimer) * (isCrouching ? crouchBobAmount : isSprinting ? runBobAmount : walkBobAmount),
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

	private void HandleToggleCrouch()
	{
		if (isCrouching && CrouchShapeCast.IsColliding() == false)
		{
			crouching(false);
		}
		else if (!isCrouching)
		{
			crouching(true);
		}
	}


	private void crouching(bool state)
	{
		switch (state)
		{
			case true:
				animationPlayer.Play("crouch", 0, 7.0f);
				break;
			case false:
				animationPlayer.Play("crouch", 0, -7.0f, true);
				break;
		}
	}

	private void HandleSprintInput()
	{
		isSprinting = !isSprinting;
	}

	private void _on_animation_player_animation_started(string anim_name)
	{
		if (anim_name == "crouch")
		{
			isCrouching = !isCrouching;
		}
	}
}
