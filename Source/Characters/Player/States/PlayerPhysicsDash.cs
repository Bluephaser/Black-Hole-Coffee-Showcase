// Name: PlayerPhysicsDash.cs
// Author: Liam Binford
// Desc: Apply horizontal impulse in the current facing direction. Gets PhysicsDashForce property from Player and multiplies
//		 by 100.
using Godot;
using System;

public partial class PlayerPhysicsDash : State
{
	private Player _player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_player = Owner as Player; // use this instead of RootCharacter for pretty much everything character-related
		if(_player is null)
			throw new NullReferenceException("Owner character is not Player");
	}

	// Virtual functions
	public override void StateEnter() {}
	public override void StateExit() {}

	public override void StateUpdate(double delta)
	{
		_player.ApplyForce(new Vector2(_player.FacingDirection * _player.PhysicsDashForce * 100.0f, 0.0f));
	}
}
