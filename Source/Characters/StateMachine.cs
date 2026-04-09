// Name: StateMachine.cs
// Author: Liam Binford
// Desc: Base state machine class to be inherited from for all character state machines. All states should be their own
//       scripts attached to child nodes of the state machine. This class should be exclusively for logic to switch
//       between states.
using Godot;
using System;
[GlobalClass]

public partial class StateMachine : Node
{
	protected State CurrentState;
	public bool ActionComplete { get; set; } // extra variable for states to be able to report upward that their associated action is finished
	
	protected void SwitchState(State newState)
	{
		CurrentState.StateExit();
		CurrentState = newState;
		CurrentState.StateEnter();
		ActionComplete = false;
		GD.Print("Current state: " + CurrentState.StateName); // Debug print
	}
}
