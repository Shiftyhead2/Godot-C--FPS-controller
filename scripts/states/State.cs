using Godot;

public partial class State : Node
{
	[Signal]
	public delegate void TransitionStateEventHandler(string newState);


	public virtual void Enter(State previousState) { }

	public virtual void Exit() { }
	public virtual void Update(float delta) { }

	public virtual void PhysicsUpdate(float delta) { }
}
