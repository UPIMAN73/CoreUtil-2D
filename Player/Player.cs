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

[GlobalClass]
public partial class Player : CoreEntity
{
	// Exception Reference
	private ExceptionReference exceptionReference = new ExceptionReference{ className = "Player" };

	// Core entity
	private InputNode _inputNode;
	//private StatNode _statNode;
	// NetworkEntity _networkEntity; // TODO Create a network entity that will handle the network interactions for the player

	/// <summary>
	/// Respawns the player at the spawn position. This will be used when the player dies or needs to be reset to the spawn point. It will also log the process of respawning the player in a way that is easy to read and understand.
	/// <para><code>
	/// player.RespawnPlayer();
	/// </code></para>
	/// </summary>
	public void RespawnPlayer()
	{
		this.Position = this.spawnPosition;
	}

	// Stop movement
	/// <summary>
	/// Stops the player's movement by setting the current speed to zero and the direction to zero. This will be used when the player needs to stop moving, such as when they are hit or when they need to stop for any reason. It will also log the process of stopping the player in a way that is easy to read and understand.
	/// <para><code>
	/// player.StopPlayer();
	/// </code></para>
	/// </summary>
	/// <returns></returns>
	public Vector2 StopPlayer()
	{
		return Vector2.Zero;
	}

	// Based on the input and character's direction, we can move the player
	/// <summary>
	/// Moves the player based on the current speed and direction. This will be called every frame in the _Process function to update the player's position based on their movement. It will also log the process of moving the player in a way that is easy to read and understand.
	/// <para><code>
	/// player.MovePlayer();
	/// </code></para>
	/// </summary>
	public void MovePlayer()
	{
		this.currentSpeed = this.shiftSpeed * this.runSpeed + this.walkSpeed;
		this.Position += this.currentSpeed * this.direction;
	}

	// Get the proper vector direction
	/// <summary>
	/// Gets the player's current movement direction as a Vector2.
	/// This will be used to determine the direction the player is moving in based on their input. 
	/// It will also log the process of getting the player's direction in a way that is easy to read and understand.
	/// <para><code>
	/// Vector2 direction = player.GetDirection();
	/// </code></para>
	/// </summary>
	/// <returns>The player's current movement direction as a Vector2.</returns>
	public Vector2 getDirection()
	{
		return this.direction;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		exceptionReference.methodName = "_Ready";
		ExceptionManager.LogInfo(exceptionReference, "Initializing Key Actions");
		initializeInputNode();
		ExceptionManager.LogInfo(exceptionReference, "Player has been initialized");
		ExceptionManager.LogInfo(exceptionReference, "Player's current position: " + this.Position);
		RespawnPlayer();
	}

	/// <summary>
	/// Called when the node is removed from the scene tree. 
	/// This is where we will disconnect any signals that we connected in the 
	/// _Ready function to prevent memory leaks and other issues. 
	/// It will also log the process of exiting the tree in a way that is easy to read and understand.
	/// <para><code>
	/// player._ExitTree();
	/// </code></para>
	/// </summary>
	public override void _ExitTree()
	{
		if (_inputNode != null) {
			_inputNode.InputActionTriggered -= OnInputActionTriggered;
		}
	}

	/// <summary>
	/// Initializes the input node for the player. 
	/// This will check to see if a child node of type InputNode exists and if not, 
	/// it will create one and add it as a child to the player node. 
	/// It will also connect the input action triggered signal to the player node so that we can react to player input.
	/// It will log the process of initializing the input node in a way that is easy to read and understand.
	/// <para><code>
	/// player.initializeInputNode();
	/// </code></para>
	/// </summary>
	private void initializeInputNode()
	{
		exceptionReference.methodName = "initializeInputNode";
		_inputNode = GetNodeOrNull<InputNode>("InputNode");

		// Check to see if a child input node exists
		// if not, then create one and add it as a child to the player node.
		if (_inputNode == null) {
			// Input Node doesn't exist here
			ExceptionManager.LogWarning(exceptionReference, "Cannot find child node 'InputNode' of 'Player'.");
			ExceptionManager.LogInfo(exceptionReference, "Player will need to create an 'InputNode' child node in order to handle player input.");
			AddChild(new InputNode(), false);
			_inputNode = GetNodeOrNull<InputNode>("InputNode");
			ExceptionManager.LogInfo(exceptionReference, "Created new 'InputNode' child node for 'Player'.");
		} else {
			ExceptionManager.LogInfo(exceptionReference, "Found 'InputNode' child node for 'Player'.");
		}

		// Check to see if the input node was successfully created or found and if so, 
		// connect the input action triggered signal to the player node so that we can react to player input.
		if (_inputNode != null) {
			_inputNode.InputActionTriggered += OnInputActionTriggered;
		} else {
			ExceptionManager.LogError(exceptionReference, "Failed to initialize 'InputNode' for 'Player'. Player input will not work.");
			
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
	/// <summary>
	/// This method is called when an input action is triggered by the player.
	///  It will log the input action that was triggered in a way that is easy to read and understand, 
	/// and then it will call the UpdateInput method to update the player's input state based on the new input action.
	/// <para><code>
	/// player.OnInputActionTriggered("ui_up");
	/// </code></para>
	/// <para>NOTE:</para>
	/// <para>This method is meant to be connected to the InputNode's InputAction
	/// Triggered signal, so that it is called automatically whenever the player triggers an input action.
	/// </para>
	/// </summary>
	/// <param name="action"></param>
	private void OnInputActionTriggered(StringName action) 
	{
		exceptionReference.methodName = "OnInputActionTriggered";
		ExceptionManager.LogInfo(exceptionReference, "Input action triggered: " + action);
		UpdateInput();
	}
}
