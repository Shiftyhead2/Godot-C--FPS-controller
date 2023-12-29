using Godot;
using System.Collections.Generic;

public partial class PlayerStateMachine : Node
{

	[Export]
	private NodePath initialState;

	private State currentState;

	private Dictionary<string, State> states;




	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{

		states = new Dictionary<string, State>();
		foreach (Node child in GetChildren())
		{
			if (child is State s)
			{
				states[child.Name] = s;
				s.TransitionState += on_state_transition;
			}
			else
			{
				GD.PushWarning("State machine contains incompatible child nodes");
			}
		}

		await ToSignal(Owner, "ready");
		currentState = GetNode<State>(initialState);
		currentState.Enter();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		currentState.Update((float)delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		currentState.PhysicsUpdate((float)delta);
	}

	private void on_state_transition(string newStateName)
	{
		State new_state = states[newStateName];
		if (new_state != null)
		{
			if (new_state != currentState)
			{
				currentState.Exit();
				new_state.Enter();
				currentState = new_state;
			}
		}
		else
		{
			GD.PushWarning("State does not exist!");
		}
	}
}
