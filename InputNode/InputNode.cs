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
		{"Escape", "menu"},
		{"E", "interact"}
	};

	[Export]
	private Godot.Collections.Dictionary<string, StringName> defaultMouseActionMapping = new Godot.Collections.Dictionary<string, StringName>()
	{
		{"Left", "attack"},
		{"Right", "block"},
		{"Middle", "special"}
	};

	[Export]
	private Godot.Collections.Dictionary<string, StringName> defaultInputActionMapping = new Godot.Collections.Dictionary<string, StringName>()
	{
		{"Space", "jump"},
		{"R", "respawn"},
		{"Escape", "menu"},
		{"Left", "attack"},
		{"Right", "block"},
		{"Middle", "special"},
		{"E", "interact"}
	};

	// Player keyboard input mapping
	[Export]
	private Godot.Collections.Dictionary<string, StringName> keyActionMapping = new Godot.Collections.Dictionary<string, StringName>();

	// Player mouse input mapping
	[Export]
	private Godot.Collections.Dictionary<string, StringName> mouseActionMapping = new Godot.Collections.Dictionary<string, StringName>();

	// Non directional input mapping (e.g. jump, attack, interact, etc.)
	[Export]
	private Godot.Collections.Dictionary<string, StringName> inputActionMapping = new Godot.Collections.Dictionary<string, StringName>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitilizeKeyActions();
	}

	// Handle user input
	public override void _UnhandledInput(InputEvent @event)
	{
		//UpdateInput();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey inputEventKey)
		{
			var keyInput = inputEventKey.AsText().Split("+");
			foreach (string key in keyInput) 
			{
				GD.Print(key);
				if (keyActionMapping.Keys.Contains(key)) {
					var keyAction = keyActionMapping[key];
					GD.Print("Action " + keyAction + " triggered by key: " + key);
					// Deploy a signal so that other nodes can listen to the input actions and react accordingly
					//SignalBus.EmitSignal("InputActionTriggered", keyAction);
				} else {
					GD.Print("No action found for key: " + key );
				}
			}
		}
	}

	// Initilize the key action status based on the key action mappingCount
	public void InitilizeKeyActions()
	{
		var kamCount = keyActionMapping.Count;
		var mamCount = mouseActionMapping.Count;
		var iamCount = inputActionMapping.Count;

		// First check if the actions have been loaded
		if ((kamCount == mamCount) && (mamCount == iamCount) && (iamCount == 0)) {
			GD.PrintErr("None of the Actions have been previously defined. Please reload the actions.");
			return;
		}
		
		// Then do the keyboard mapping	
		if (kamCount > 0) {
			foreach (var (name, action) in this.keyActionMapping) {
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
			foreach (var (name, action) in this.defaultKeyActionMapping) {
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

		if (mamCount > 0)		
		{
			foreach (var (name, action) in this.mouseActionMapping) {
				var @currentEvent = new InputEventMouseButton();
				MouseButton current_button = @currentEvent.ButtonIndex;
				if (InputMap.HasAction(action)) {
					InputMap.ActionAddEvent(action, @currentEvent);
				} else {
					InputMap.AddAction(action, 0.2f);
					InputMap.ActionAddEvent(action, @currentEvent);
				}
			}
		} else {
			// Use default mouse action mapping if no custom mapping is provided
			foreach (var (name, action) in this.defaultMouseActionMapping) {
				var @currentEvent = new InputEventMouseButton();
				MouseButton current_button = @currentEvent.ButtonIndex;
				if (InputMap.HasAction(action)) {
					InputMap.ActionAddEvent(action, @currentEvent);
				} else {
					InputMap.AddAction(action, 0.2f);
					InputMap.ActionAddEvent(action, @currentEvent);
				}
			}
		}

		// TODO: Initilize non directional input mapping (e.g. jump, attack, interact, etc.)
		if (iamCount > 0) {
			//
			foreach (var (name, action) in this.inputActionMapping) {
				var @currentEvent = new InputEventKey();
				@currentEvent.PhysicalKeycode = OS.FindKeycodeFromString(name);
				if (InputMap.HasAction(action)) {
					InputMap.ActionAddEvent(action, @currentEvent);
				} else {
					InputMap.AddAction(action, 0.2f);
					InputMap.ActionAddEvent(action, @currentEvent);
				}
			}

			//
			foreach (var (name, action) in this.inputActionMapping) {
				var @currentEvent = new InputEventMouseButton();
				MouseButton current_button = @currentEvent.ButtonIndex;
				if (InputMap.HasAction(action)) {
					InputMap.ActionAddEvent(action, @currentEvent);
				} else {
					InputMap.AddAction(action, 0.2f);
					InputMap.ActionAddEvent(action, @currentEvent);
				}
			}	
		} else {
			// Use default non directional input mapping if no custom mapping is provided
			foreach (var (name, action) in this.defaultInputActionMapping) {
				var @currentEvent = new InputEventKey();
				@currentEvent.PhysicalKeycode = OS.FindKeycodeFromString(name);
				if (InputMap.HasAction(action)) {
					InputMap.ActionAddEvent(action, @currentEvent);
				} else {
					InputMap.AddAction(action, 0.2f);
					InputMap.ActionAddEvent(action, @currentEvent);
				}
			}

			// Mouse events
			foreach (var (name, action) in this.defaultInputActionMapping) {
				var @currentEvent = new InputEventMouseButton();
				MouseButton current_button = @currentEvent.ButtonIndex;
				if (InputMap.HasAction(action)) {
					InputMap.ActionAddEvent(action, @currentEvent);
				} else {
					InputMap.AddAction(action, 0.2f);
					InputMap.ActionAddEvent(action, @currentEvent);
				}
			}
		}
	}

	// Update the custom input mapping based on the provided mapping
	public void UpdateInputMapping(Godot.Collections.Dictionary<string, StringName> newKeyActionMapping
		, Godot.Collections.Dictionary<string, StringName> newMouseActionMapping
		, Godot.Collections.Dictionary<string, StringName> newInputActionMapping) 
	{
		keyActionMapping = newKeyActionMapping;
		mouseActionMapping = newMouseActionMapping;
		inputActionMapping = newInputActionMapping;
		InitilizeKeyActions();
	}

	// save the custom input mapping to a file
	public void SaveInputMapping(string filePath) 
	{
		using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Write);
		if (file == null) {
			GD.PrintErr("Error reading file: " + filePath);
			return;
		} else {
			if (file.GetError() == Error.Ok) {
				var inputMappingData = new Godot.Collections.Dictionary<string, Godot.Collections.Dictionary<string, StringName>>()
				{
					{"keyActionMapping", keyActionMapping},
					{"mouseActionMapping", mouseActionMapping},
					{"inputActionMapping", inputActionMapping}
				};
				file.StoreLine(Json.Stringify(inputMappingData));
				file.Flush();
			} else {
				GD.PrintErr("[ERROR] " + file.GetError());
			}
			file.Close();
		}
	}

	// Load the custom input mapping from a file
	public void LoadInputMapping(string filePath)
	{
		using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
		if (file == null) {
			GD.PrintErr("Error reading file: " + filePath);
			return;
		} else {
			if (file.GetError() == Error.Ok)		{
			// using var inputMappingData = Json.Parse(file.GetLine(), false);
			GD.Print(file.GetAsText());
			file.Flush();
			
			// // Assignment mapping data to the current mapping
			// this.keyActionMapping = inputMappingData["keyActionMapping"];
			// this.mouseActionMapping = inputMappingData["mouseActionMapping"];
			// this.inputActionMapping = inputMappingData["inputActionMapping"];

			// // Initilize the key actions based on the loaded mapping
			// InitilizeKeyActions();
			} else {
				GD.PrintErr("[ERROR] " + file.GetError());
			}
			file.Close();
		}
		
	}

	// Setup a signal for the input action triggered so that other nodes can listen to it and react accordingly
	[Signal]
	public delegate void InputActionTriggeredEventHandler(StringName action);

	// Update Keyboard inputs
	public (Vector2 Direction, float RunSpeed) UpdateDirectionInput() {
		return (Input.GetVector("left", "right", "up", "down"), Input.GetActionStrength("shift"));
	}
}
