// Name: PlayerDash.cs
// Author: Liam Binford
// Desc: Player dashes forward for a set time/intended distance and then stops. Motion is completely linear and all other
//		 forms of movement not cause by collisions are disabled during the time. TODO: Doesn't work right now, needs fixing
using Godot;
using System;
using GDExtensionWrappers;

public partial class PlayerDash : State
{
	private Player _player;
	private float _dashTime;
	private float _timer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_player = Owner as Player;
	}

	// Virtual functions
	public override void StateEnter()
	{
		_player.Velocity = Vector2.Zero;
		_dashTime = _player.DashDistance / _player.DashSpeed; // calculate time spend dashing (distance / velocity)
		_timer = _dashTime;
	}
	public override void StateExit() {}

	public override void StateUpdate(double delta)
	{
		Wwise.PostEventId(AKCS.EVENTS.METAL_PIPE, this);
		
		_timer -= (float)delta;
		if (_timer > 0)
		{
			Vector2 newVel = new Vector2(_player.DashSpeed * _player.FacingDirection, 0.0f);
			_player.Velocity =  newVel;
		}
		else
			Sm.ActionComplete = true;
	}
}
