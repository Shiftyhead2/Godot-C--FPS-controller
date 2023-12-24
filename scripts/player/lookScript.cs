using Godot;

public partial class lookScript : Node3D
{

  [ExportCategory("Node References")]
  [Export]
  private Node3D head;


  [ExportCategory("Look Variables")]
  [Export]
  private float Sensitivity = 0.05f;



  private Vector3 targetRotation;

  [Export]
  private float ActualCameraRotationSpeed = 20.0f;



  public override void _UnhandledInput(InputEvent @event)
  {
    base._UnhandledInput(@event);

    if (@event is InputEventMouseMotion mouseMotion)
    {
      targetRotation = new Vector3(
      Mathf.Clamp((-1 * mouseMotion.Relative.Y * Sensitivity) + targetRotation.X, -90f, 90f),
      Mathf.Wrap((-1 * mouseMotion.Relative.X * Sensitivity) + targetRotation.Y, 0f, 360f),
    0);
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    HandleHeadRotation((float)delta);
  }


  private void HandleHeadRotation(float delta)
  {
    head.Rotation = new Vector3(
      Mathf.LerpAngle(head.Rotation.X, Mathf.DegToRad(targetRotation.X), ActualCameraRotationSpeed * delta),
      Mathf.LerpAngle(head.Rotation.Y, Mathf.DegToRad(targetRotation.Y), ActualCameraRotationSpeed * delta),
      0
    );
  }

}
