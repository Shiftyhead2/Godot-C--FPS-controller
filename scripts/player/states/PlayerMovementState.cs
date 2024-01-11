using Godot;

public partial class PlayerMovementState : State
{

	protected playerController Player;

	[Export]
	protected MovementConfig movementConfig;

	[Export]
	protected HeadBobConfig headBobConfig;

	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		await ToSignal(Owner, Node.SignalName.Ready);
		Player = Owner as playerController;
	}
}
