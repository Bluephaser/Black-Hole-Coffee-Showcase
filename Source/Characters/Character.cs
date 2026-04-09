// Name: Character.cs
// Author: Liam Binford
// Desc: Base character class to be inherited by scripts attached to any CharacterBody2D. Handles physics-based
//       movement and calculations.

using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]

public partial class Character : CharacterBody2D
{
	[Export] private float _groundedRunAcceleration = 1200.0f; // rate at which the player accelerates when running on the ground
	[Export] private float _aerialRunAcceleration = 600.0f; // rate at which the player accelerates when running in the air
	[Export] private float _mass = 1.0f;
	[Export] private float _friction = 0.1f; // deceleration scalar applied to velocity when player on the floor. should be less than 1.
	[Export] private float _drag = 0.1f; // deceleration scalar applied to velocity when player is in the air. should be less than 1.
	[Export] private float _maxGroundedRunVelocity = 300.0f; // clamp value for grounded run acceleration
	[Export] private float _maxAerialRunVelocity = 300.0f; // clamp value for aerial run acceleration
	
	// properties-------------------------------------------------------------------------------------------------------
	
	/* keeps track of the current facing direction of the character. It's a float because most of the time changing the
	facing direction of the player is done with input. -1.0f is left, 1.0f is right. */
	public float FacingDirection { get; set; } = 1.0f; 
	public bool RunLocked { get; set; } // set true to disable left/right movement
	public int DamageTaken { get; set; }
	public Vector2 Acceleration { get; protected set; } // character's current acceleration in a given frame
	public List<Vector2> Forces { get; } = []; // all forces being applied to character in a given frame
	
	// properties for public access of editor fields--------------------------------------------------------------------
	public float GroundedRunAcceleration { get => _groundedRunAcceleration; protected set => _groundedRunAcceleration = value; }
	public float AerialRunAcceleration { get => _aerialRunAcceleration; protected set => _aerialRunAcceleration = value; }
	public float Mass { get => _mass; protected set => _mass = value; }
	public float Friction { get => _friction; set => _friction = value; }
	public float Drag { get => _drag; protected set => _drag = value; }
	public float MaxGroundedRunVelocity { get => _maxGroundedRunVelocity; protected set => _maxGroundedRunVelocity = value; }
	public float MaxAerialRunVelocity { get => _maxAerialRunVelocity; protected set => _maxAerialRunVelocity = value; }

	// private variables------------------------------------------------------------------------------------------------
	private Health _healthComponent;
	
	// properties for member variables of children----------------------------------------------------------------------
	public Health HealthComponent => _healthComponent ??= GetNode<Health>("Health");

	public override void _Ready()
	{
		if (_healthComponent is not null)
			_healthComponent.DamageTaken += OnDamageTaken; // call this function when damage taken signal received
	}
	
	public override void _PhysicsProcess(double delta)
	{
		foreach (Vector2 force in Forces)
		{
			Acceleration += force; // acceleration equals the sum of all forces
		}
		Vector2 initialVel = Velocity;
		Vector2 newVel = Acceleration * (float)delta;
		Velocity = initialVel + newVel;


		if (IsOnFloor())
		{
			if (_friction > 1.0f || _friction < 0.0f)
				throw new Exception(Name + " Friction must be a value between 0 and 1!");
			Velocity *= (1 - _friction); // apply friction when on floor
		}
		else
		{
			if (_drag > 1.0f || _drag < 0.0f)
				throw new Exception(Name + " Drag must be a value between 0 and 1!");
			Velocity *= (1 - _drag); // apply drag when in air
		}

		Acceleration = Vector2.Zero;
		Forces.Clear(); // clear all forces for this frame
		MoveAndSlide();
	}

	/* Adds a Vector2 to the list of forces enacted on the character this frame. All added forces are impulses, so
	they're cleared each frame and must be re-added. */
	public void ApplyForce(Vector2 force)
	{
		Forces.Add(force);
	}

	// Called when damage taken signal received from health component.
	private void OnDamageTaken(int damage)
	{
		DamageTaken = damage;
		GD.Print("Current HP: " + _healthComponent.CurrentHealth + " / " + _healthComponent.MaxHealth);
	}
	
}
