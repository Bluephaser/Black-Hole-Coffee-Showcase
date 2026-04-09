// Name: Run.cs
// Author: Liam Binford
// Desc: Apply movement acceleration force along horizontal input axis until max grounded velocity reached.
using Godot;
using System;

public partial class Run : State
{
	// Virtual functions
	public override void StateEnter() {}
	public override void StateExit() {}

	public override void StateUpdate(double delta)
	{
		if (RootCharacter.RunLocked || Mathf.Abs(RootCharacter.Velocity.X) >= RootCharacter.MaxGroundedRunVelocity)
			return; // don't apply any more acceleration if max grounded run velocity reached
		
		// set the current x velocity to the input direction times speed
		float dir = Input.GetAxis("ui_left", "ui_right");
		Vector2 runForce = Vector2.Zero; // force applied to player through running
		runForce.X = dir * RootCharacter.GroundedRunAcceleration;
		if(dir != 0.0f)
			RootCharacter.FacingDirection = dir; // set player direction for the purposes of animations
		//_player.Velocity = vel
		RootCharacter.ApplyForce(runForce);
	}
}
