// Name: PlayerFall.cs
// Author: Liam Binford
// Desc: Apply gravity to player as a force every frame. Also contains logic for variable jump height. Contained here
//		 because the actual jump state is called once and then this state handles the rest of the jump.
using Godot;
using System;

public partial class PlayerFall : State
{
	private Player _player;
	private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private float _gravTimer; // timer to determine 
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_player = Owner as Player;
		
	}

	// Virtual functions
	public override void StateEnter()
	{
		
	}

	public override void StateExit()
	{
		
	}
	
	public override void StateUpdate(double delta)
	{
		Vector2 airMovementForce = Vector2.Zero; // combined forces applied to character via the weight force and horizontal movement
		float dir = Input.GetAxis("ui_left", "ui_right");
		float weight = _gravity * _player.Mass;
		Vector2 vel = _player.Velocity;
		
		if(!_player.RunLocked && !(Mathf.Abs(_player.Velocity.X) >= _player.MaxAerialRunVelocity))
			airMovementForce.X = dir * _player.AerialRunAcceleration; // aerial movement is slidier than grounded movement
		
		if(dir != 0)
			_player.FacingDirection = dir; // set player direction for the purposes of animations
		
		if(Input.IsActionPressed("ui_accept") || vel.Y >= 0) // gravity affects player like normal if jump button held and after apex of jump
			airMovementForce.Y = weight;
		else
			airMovementForce.Y = weight * _player.GravityReleaseMod; // player loses upwards velocity faster if they let go of the button
		
		_player.ApplyForce(airMovementForce);
	}

	
}
