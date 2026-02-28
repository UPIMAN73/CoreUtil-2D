/*
 * @name : Player Object
 * @author : Joshua Calzadillas
 * @version : 0.1.0
 * @creation-date : February 17, 2026
 * @license : MIT
 */

// Simple Imports
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Player : CharacterBody2D
{
	
	[Export]
	private float deadzone = 0.2f;

	[Export]
	// Direction (easier for managing more complicated tasks)
	private Vector2 direction = new Vector2();

	[Export]
	// Spawn Position (this will be used to set the spawn point)
	private Vector2 spawnPosition = new Vector2();

	[Export]
	// player's move speed	
	private Vector2 moveSpeed = new Vector2();

	[Export]
	// player's shift speed	
	private Vector2 shiftSpeed = new Vector2();

	[Export]
	// player's jump speed
	private Vector2 jumpSpeed = new Vector2();

	// player's Current speed
	private Vector2 currentSpeed = new Vector2();

	/*
	 * Key input mappings
	 */
	[Export]
	private Dictionary<string, StringName> keyActionMapping = new Dictionary<string, StringName>();


	// Handle user input
	public override void _UnhandledInput(InputEvent @event)
	{
		UpdateInput();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey inputEventKey)
		{
			var keyInput = inputEventKey.AsText().Split("+");
			foreach (string key in keyInput) 
			{
				GD.Print(key);
				if (this.keyActionMapping.Keys.Contains(key)) {
					var keyAction = keyActionMapping[key];
					GD.Print(keyAction);
					GD.Print(InputMap.ActionGetEvents(keyAction));
					GD.Print(InputMap.ActionHasEvent(keyAction, inputEventKey));	
				} else {
					GD.Print("No action found for key: " + key );
				}
			}
		}
	}

	// Initilize the key action status based on the key action mapping
	private void InitilizeKeyActions()
	{
		if (keyActionMapping.Count > 0)
		{
			// TODO INPUT MAP 
			foreach (var (name, action) in this.keyActionMapping)
			{
				var @currentEvent = new InputEventKey();
				@currentEvent.PhysicalKeycode = OS.FindKeycodeFromString(name);
				if (InputMap.HasAction(action)) {
					InputMap.ActionAddEvent(action, @currentEvent);
				} else {
					InputMap.AddAction(action, this.deadzone);
					InputMap.ActionAddEvent(action, @currentEvent);
				}
			}
		} else {
			InputMap.LoadFromProjectSettings();
		}
	}

	public void UpdateKeyActionsMapping()
	{
		// TODO
		// set the value of the mappings based on a current state of a mapping effect
	}

	public void UpdatePlayerActions()
	{
		// TODO
		// This is for tasks such as respawn, pause, play, and other game mechanics.
	}
	
	// Update Keyboard inputs
	public void UpdateInput()
	{		
		UpdateDirection();
		UpdateSpeed();
	}
	
	public void UpdateDirection() {
		// Changing the direction based on the keyboard input
		this.direction = Input.GetVector("left", "right", "up", "down");
		//GD.Print(this.direction);
	}
	
	public void UpdateSpeed() {
		// Set the speed of the player movement based on the shift key
		this.currentSpeed = Input.GetActionStrength("shift") * shiftSpeed + moveSpeed;
		//GD.Print(this.currentSpeed);
	}


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
		InitilizeKeyActions();
		RespawnPlayer();
		GD.Print("Player has been initialized");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Handle the user input
		MovePlayer();
	}
}
