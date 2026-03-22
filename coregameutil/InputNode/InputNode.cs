/*
 * @name : InputNode
 * @author : Joshua Calzadillas
 * @version : 0.1.0
 * @creation-date : March 22, 2026
 * @license : MIT
 * @file : InputNode.cs
 * @description : A node that will handle the input from the player,
 * such as keyboard & mouse inputs, joystick inputs, etc. 
 * This will be used to manage the player's input and provide an interface for 
 * other nodes to interact with the input.
 */

using Godot;


 [GlobalClass]
public partial class InputNode : Node2D
{
	// Default input mapping for the player
	[Export]
	private Godot.Collections.Dictionary<string, StringName> defaultKeyActionMapping = new Godot.Collections.Dictionary<string, StringName>()
	{
		{"W", "up"},
		{"A", "left"},
		{"S", "down"},
		{"D", "right"},
		{"Up", "up"},
		{"Left", "left"},
		{"Down", "down"},
		{"Right", "right"},
		{"Shift", "shift"},
		{"Space", "jump"},
		{"R", "respawn"},
		{"Escape", "pause"}
	};

	// Player keyboard input mapping
	[Export]
	private Godot.Collections.Dictionary<string, StringName> keyActionMapping = new Godot.Collections.Dictionary<string, StringName>();


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
	public void InitilizeKeyActions()
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
					InputMap.AddAction(action, 0.2f);
					InputMap.ActionAddEvent(action, @currentEvent);
				}
			}
		} else {
			// Use default key action mapping if no custom mapping is provided
			foreach (var (name, action) in this.defaultKeyActionMapping)
			{
				var @currentEvent = new InputEventKey();
				@currentEvent.PhysicalKeycode = OS.FindKeycodeFromString(name);
				if (InputMap.HasAction(action)) {
					InputMap.ActionAddEvent(action, @currentEvent);
				} else {
					InputMap.AddAction(action, 0.2f);
					InputMap.ActionAddEvent(action, @currentEvent);
				}
			}
		}
	}

	// Update Keyboard inputs
	public (Vector2 Direction, float RunSpeed) UpdateInput() {
		return (Input.GetVector("left", "right", "up", "down"), Input.GetActionStrength("shift"));
	}
}
