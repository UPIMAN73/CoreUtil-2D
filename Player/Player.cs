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
using System.Runtime.CompilerServices;
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
		ExceptionManager.LogInfo("Player", "_Ready", "Initializing Key Actions");
		initializeInputNode();
		ExceptionManager.LogInfo("Player", "_Ready", "Player has been initialized");
		ExceptionManager.LogInfo("Player", "_Ready", "Player's current position: " + this.Position);
		RespawnPlayer();
	}

	public override void _ExitTree()
	{
		if (_inputNode != null)
		{
			_inputNode.InputActionTriggered -= OnInputActionTriggered;
		}
	}

	private void initializeInputNode()
	{
		_inputNode = GetNodeOrNull<InputNode>("InputNode");

		// Check to see if a child input node exists
		// if not, then create one and add it as a child to the player node.
		if (_inputNode == null) {
			// Input Node doesn't exist here
			ExceptionManager.LogWarning("Player", "initializeInputNode", "Cannot find child node 'InputNode' of 'Player'.");
			ExceptionManager.LogInfo("Player", "initializeInputNode", "Player will need to create an 'InputNode' child node in order to handle player input.");
			AddChild(new InputNode(), false);
			_inputNode = GetNodeOrNull<InputNode>("InputNode");
			ExceptionManager.LogInfo("Player", "initializeInputNode", "Created new 'InputNode' child node for 'Player'.");
		} else {
			ExceptionManager.LogInfo("Player", "initializeInputNode", "Found 'InputNode' child node for 'Player'.");
		}

		// Check to see if the input node was successfully created or found and if so, 
		// connect the input action triggered signal to the player node so that we can react to player input.
		if (_inputNode != null) {
			_inputNode.InputActionTriggered += OnInputActionTriggered;
		} else {
			ExceptionManager.LogError("Player", "initializeInputNode", "Failed to initialize 'InputNode' for 'Player'. Player input will not work.");
			
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
		ExceptionManager.LogInfo("Player", "OnInputActionTriggered", "Input action triggered: " + action);
		UpdateInput();
	}
}
