// Name: PlayerJump.cs
// Author: Liam Binford
// Desc: Apply jump force to player. Takes JumpForce property from player and multiplies by 100. Runs once before switching
//		 to PlayerFall.
using Godot;
using System;

public partial class PlayerJump : State
{
	private PlayerStateMachine _playerSm;
	private Player _player;
	private bool _ranOnce;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_playerSm = Sm as PlayerStateMachine;
		_player = Owner as Player;
	}


	// Virtual functions
	public override void StateEnter()
	{
		base.StateEnter();
		Vector2 currVel = _player.Velocity;
		currVel.Y = 0;
		_player.Velocity = currVel; // zero out vertical velocity so that double jumps feel better
	}
	public override void StateExit() {}

	public override void StateUpdate(double delta)
	{
		Vector2 jumpImpulse = new Vector2(0.0f, _player.JumpForce * 100.0f);
		_player.ApplyForce(jumpImpulse);
	}
}
