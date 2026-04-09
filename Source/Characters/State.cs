// Name: State.cs
// Author: Liam Binford
// Desc: Base State to be inherited from by all states. Has enter and exit functions for animations and various other functionalities.
using Godot;
using System;
[GlobalClass]

public partial class State : Node
{
	protected Character RootCharacter; // root Character node of Character scene
	protected StateMachine Sm; // parent state machine
	
	public string StateName { get; private set; } // readonly property for the state's name

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		StateName = Name;
		Sm = GetParent() as StateMachine;
		RootCharacter = Owner as Character;
	}

	// Virtual functions
	public virtual void StateEnter() {}
	public virtual void StateExit() {}
	public virtual void StateUpdate(double delta) {}
}
