// Name: PlayerStateMachine.cs
// Author: Liam Binford
// Desc: State machine used for player. Most state change logic is based on input reading. Contains Idle, Run, Jump,
//       Fall, and Dash states.
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class PlayerStateMachine : StateMachine
{
    private Player _player;
    private bool _cTimeTriggered; // checks if coyote time is triggered, usually by the player walking off a ledge.
    private float _cTimer; // countdown timer for coyote time. For coyote time limit, check Player.cs
    private float _jTimer; // countdown timer for jump cooldown. For jump cooldown time limit, check Player.cs
    private bool _doubleJumped;
    private bool _airDashed; // TODO: implement. Will be used to more tightly control the number of times player can air-dash    

    // References to each child state
    private State _idle;
    private State _run;
    private State _jump;
    private State _fall;
    private State _dash;
    private State _physicsDash;
    private State _roll;
    private State _takeDamage;

    public override void _Ready()
    {
        base._Ready();
        _player = Owner as Player;
        _idle = FindChild("Idle") as State;
        _run = FindChild("Run") as State;
        _jump = FindChild("Jump") as State;
        _fall = FindChild("Fall") as State;
        _dash = FindChild("Dash") as State;
        _physicsDash = FindChild("PhysicsDash") as State;
        _roll = FindChild("Roll") as State;
       _takeDamage = FindChild("TakeDamage") as State;
        CurrentState = _idle; // Begin in idle state
    }

    // Physics update the current state
    public override void _PhysicsProcess(double delta)
    {
        string name = CurrentState.Name;
        float input = Mathf.Abs(Input.GetAxis("ui_left", "ui_right")); // If not 0, player is moving in a direction
        switch (name)
        {
            case "Run":
                // If jump pressed
                if (Input.IsActionJustPressed("ui_accept"))
                    SwitchState(_jump);
                else if(Input.IsActionPressed("pl_roll"))
                    SwitchState(_roll);
                // Else if no horizontal movement detected
                else if (input == 0)
                    SwitchState(_idle);
                
                // Falling with coyote time
                if (!_player.IsOnFloor())
                {
                    if (!_cTimeTriggered)
                    {
                        _cTimer = _player.CoyoteTime;
                        _cTimeTriggered = true;
                    }
                    _cTimer -= (float)delta;
                    if (_cTimer <= 0)
                    {
                        SwitchState(_fall);
                        _cTimeTriggered = false;
                    }
                }
                
                break;
            case "Jump": // pretty straight forward. StateUpdate is run once for Jump before this case is executed.

                SwitchState(_fall);
                _jTimer = _player.JumpCooldown;
                
                break;
            case "Fall":

                if (_player.IsOnFloor())
                {
                    _doubleJumped = false;
                    SwitchState(input > 0 ? _run : _idle); // switch to run or idle depending on if moving
                }
                else if (Input.IsActionPressed("pl_roll")) // mid-air switch to roll state
                    SwitchState(_roll);

                if (_jTimer > 0)
                    _jTimer -= (float)delta;
                else if (Input.IsActionJustPressed("ui_accept") && !_doubleJumped)
                {
                    _doubleJumped = true;
                    SwitchState(_jump);
                }

                break;
            case "Idle":
                
                if (Input.IsActionJustPressed("ui_accept")) // If jump pressed
                    SwitchState(_jump);
                else if(Input.IsActionPressed("pl_roll"))
                    SwitchState(_roll);
                else if (input > 0.0f) // Else if horizontal input detected
                    SwitchState(_run);

                // Falling with coyote time
                if (!_player.IsOnFloor())
                {
                    if (!_cTimeTriggered)
                    {
                        _cTimer = _player.CoyoteTime;
                        _cTimeTriggered = true;
                    }
                    _cTimer -= (float)delta;
                    if (_cTimer <= 0)
                    {
                        SwitchState(_fall);
                        _cTimeTriggered = false;
                    }
                }
                
                break;
            case "Dash":

                GD.Print("Facing direction: " + _player.FacingDirection);
                if (ActionComplete) // As of now, Dash is the only state that needs to report its own end.
                    SwitchState(_idle);
                    
                break;
            case "PhysicsDash": // StateUpdate is run once for PhysicsDash before this case is executed.
                
                GD.Print("Facing direction: " + _player.FacingDirection);
                SwitchState(_player.IsOnFloor() ? _idle : _fall); // switch to idle if on floor, otherwise fall

                break;
            case "Roll": // first nested action. Will perform fall logic if player is not on floor.

                if (!_player.IsOnFloor())
                {
                    if(!Input.IsActionPressed("pl_roll"))
                        SwitchState(_fall); // switch to falling if roll released mid-air
                    else
                        _fall.StateUpdate(delta); // ensure player falls as normal while rolling
                }
                else
                {
                    if (!Input.IsActionPressed("pl_roll"))
                        SwitchState(_idle); // switch to idling if roll released on the ground
                }

                break;
            case "TakeDamage":

                SwitchState(_player.IsOnFloor() ? _idle : _fall); // switch to idle if on floor, otherwise fall
                
                break;
            default:
                throw new Exception("PlayerStateMachine has entered unexpected state: " + name);
        }
        
        // Dash can happen during any of the cases, so it is checked for after the switch statement
        if (Input.IsActionJustPressed("pl_dash") && !_player.PhysicsDash)
            SwitchState(_dash);
        else if (Input.IsActionJustPressed("pl_dash") && _player.PhysicsDash)
            SwitchState(_physicsDash);

        if (_player.DamageTaken > 0) // taking damage overrides all other state changes
            SwitchState(_takeDamage);
        
        CurrentState.StateUpdate(delta);
    }
}
