/*
 * @name : Player Object
 * @author : Joshua Calzadillas
 * @version : 0.1.0
 * @creation-date : February 17, 2026
 * @license : MIT
 * @file : Player.cs
 * @description : A player object that will handle the player's movement, 
 * input, and other player-related mechanics. 
 * This will be the character that the player will control in the game.
 */

// Simple Imports
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Player : CoreEntity
{
	// Core entity
	private InputNode _inputNode; 
	//private StatNode _statNode;
	// NetworkEntity _networkEntity; // TODO Create a network entity that will handle the network interactions for the player

	public void RespawnPlayer()
	{
		this.Position = this.spawnPosition;
	}

	// Stop movement
	public Vector2 StopPlayer()
	{
		return Vector2.Zero;
	}

	// Based on the input and character's direction, we can move the player
	public void MovePlayer()
	{
		this.currentSpeed = this.shiftSpeed * this.runSpeed + this.walkSpeed;
		this.Position += this.currentSpeed * this.direction;
	}

	// Get the proper vedctor direction
	public Vector2 getDirection()
	{
		return this.direction;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Initilizing Key Actions");
		_inputNode = (InputNode) FindChild("InputNode");
		GD.Print("Player has been initialized");
		GD.Print("Player's current position: " + this.Position);
		RespawnPlayer();

		// handle the signal assocaited with the input action triggered so that we can react to the player's input
		if (_inputNode != null)
		{
			GD.Print("[INFO] Succesfully found 'InputNode' as a child node of 'Player'");
			_inputNode.InputActionTriggered += OnInputActionTriggered;
		} else {
			// Input Node doesn't exist here
			GD.PrintErr("[ERROR] CANNOT FIND CHILD NODE 'INPUTNODE'");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		MovePlayer();
	}

	 public void UpdateInput()
	 {
	 	(this.direction, this.shiftSpeed) = _inputNode.UpdateDirectionInput();
	 	//this.direction = playerInput.Direction;
	 	//this.runSpeed = playerInput.RunSpeed;
	 }

	// Example of how to listen to the input action triggered signal and react accordingly
	private void OnInputActionTriggered(StringName action) {
		GD.Print("Input action triggered: " + action);
		UpdateInput();
	}
}
