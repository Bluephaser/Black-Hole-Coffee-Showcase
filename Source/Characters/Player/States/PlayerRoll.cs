// Name: PlayerRoll.cs
// Author: Liam Binford
// Desc: Greatly lower player friction, but lock horizontal input. Similar to Sonic the Hedgehog's roll.
using Godot;
using System;

public partial class PlayerRoll : State
{
	private Player _player;
	private float _nonRollFriction; // temp variable to store the normal friction value
	private Sprite2D _sprite; // reference to player sprite
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_player = Owner as Player;
		if(_player is null)
			throw new NullReferenceException("Owner is not a Player");
		_sprite = _player.GetNode<Sprite2D>("Sprite2D");
		_nonRollFriction = _player.Friction;
	}

	// Virtual functions
	public override void StateEnter()
	{
		_player.Friction = _player.RollFriction; // when entering state, set friction to roll friction
		_player.RunLocked = true;
		_sprite.SelfModulate = new Color(0.0f, 1.0f, 0.0f); // tint sprite green for debug purposes
	}

	public override void StateExit()
	{
		_player.Friction = _nonRollFriction; // when exiting state, set friction back to normal
		_player.RunLocked = false;
		_sprite.SelfModulate = new Color(1.0f, 1.0f, 1.0f); // set sprite back to normal
	}

	public override void StateUpdate(double delta) { }
}
