public partial class PlayerMovementState : State
{

	public playerController Player;

	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		await ToSignal(Owner, "ready");
		Player = Owner as playerController;
	}
}
