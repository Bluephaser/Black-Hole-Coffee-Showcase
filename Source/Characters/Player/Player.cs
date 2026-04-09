// Name: Player.cs
// Author: Liam Binford
// Desc: Player scene root node. Handles data for player-specific movement.
using Godot;
using System;

public partial class Player : Character
{
	// Y-value of impulse applied on player character when jumping. Multiplied by 100 in player jump state.
	[Export] private float _jumpForce = -400; 
	
	/* This modifier determines how much gravity is multiplied by if the jump button is released while still in the
	upwards-velocity part of a jump. Essentially, it allows for variable jump height by letting go of jump early. */
	[Export] private float _gravityReleaseMod = 1; 
	[Export] private float _coyoteTime = 0.2f; // small window of time between player leaving the ground and player falling
	[Export] private float _jumpCooldown = 0.25f; // cooldown time between leaving the ground and double-jumping
	[Export] private float _dashSpeed = 600.0f; // speed of player dash during dash state.
	[Export] private float _dashDistance = 1.0f; // distance player dashes during dash state (dash timer calculated from speed and distance)
	[Export] private float _physicsDashForce = 1.0f; // impulse applied to player during physics dash
	[Export] private float _rollFriction = 0.5f; // friction player experiences while rolling. Should be less than friction.
	[Export] private string[] _presetPaths; // array of paths to PlayerPreset resources. Presets found in Assets/Data/Characters/Player
	[Export] private bool _presetToggle; // in-editor bool to determine whether debug presets should be used for player stats
	[Export] private bool _physicsDash; // in-editor bool to determine if physics-based dash should be used or not
	
	private PlayerPreset _playerStats;
	
	// properties for public access of editor fields
	public float JumpForce => _jumpForce;
	public float GravityReleaseMod => _gravityReleaseMod;
	public float CoyoteTime => _coyoteTime;
	public float JumpCooldown => _jumpCooldown;
	public float DashSpeed => _dashSpeed;
	public float DashDistance => _dashDistance;
	public float PhysicsDashForce => _physicsDashForce;
	public float RollFriction => _rollFriction;
	public string[] PresetPaths => _presetPaths;
	public bool PresetToggle => _presetToggle;
	public bool PhysicsDash => _physicsDash;

	// Use a resource to set all variables to a preset. Used in PlayerPresetToggler
	public void SetStats(PlayerPreset preset)
	{
		_jumpForce = preset.JumpForce;
		_gravityReleaseMod = preset.GravityReleaseMod;
		_coyoteTime = preset.CoyoteTime;
		_jumpCooldown = preset.JumpCooldown;
		_dashSpeed = preset.DashSpeed;
		_dashDistance = preset.DashDistance;
		_physicsDashForce = preset.PhysicsDashForce;
		_rollFriction = preset.RollFriction;
    // Variables below this point are inherited from Character
		GroundedRunAcceleration = preset.GroundedRunAcceleration;
		AerialRunAcceleration = preset.AerialRunAcceleration;
		Mass = preset.Mass;
		Friction = preset.Friction;
		Drag = preset.Drag;
		MaxGroundedRunVelocity = preset.MaxGroundedRunVelocity;
		MaxAerialRunVelocity = preset.MaxAerialRunVelocity;
	}
}
