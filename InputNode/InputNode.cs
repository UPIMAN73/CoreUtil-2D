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

// using System;
using Godot;

 [GlobalClass]
public partial class InputNode : Node2D
{
	// Exception Reference
	private ExceptionReference exceptionReference = new ExceptionReference{ className = "InputNode" };

	// Setup a signal for the input action triggered so that other nodes can listen to it and react accordingly
	[Signal]
	public delegate void InputActionTriggeredEventHandler(StringName action);

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
		exceptionReference.methodName = "_Ready";
		FileSystemManager.FFIOInit();
		ExceptionManager.LogInfo(exceptionReference,  "InputNode is being initialized");
		InitilizeKeyActions();
		LoadInputMapping("user://settings/input.json");
		ExceptionManager.LogInfo(exceptionReference, "InputNode has been initialized");
	}

	// Handle user input
	public override void _UnhandledInput(InputEvent @event)
	{
		//UpdateInput();
	}

	public override void _Input(InputEvent @event)
	{
		exceptionReference.methodName = "_Input";
		if (@event is InputEventKey inputEventKey)
		{
			var keyInput = inputEventKey.AsText().Split("+");
			foreach (string key in keyInput) 
			{
				ExceptionManager.LogInfo(exceptionReference, "Key pressed: " + key);
				if (keyActionMapping.Keys.Contains(key)) {
					var keyAction = keyActionMapping[key];
					ExceptionManager.LogInfo(exceptionReference, "Action " + keyAction + " triggered by key: " + key);
					// Deploy a signal so that other nodes can listen to the input actions and react accordingly
					EmitSignal(SignalName.InputActionTriggered, keyAction);
				} else {
					ExceptionManager.LogWarning(exceptionReference, "No action found for key: " + key );
				}
			}
		}
	}

	public bool IsActionsLoaded() 
	{
		return (keyActionMapping.Count > 0) && (mouseActionMapping.Count > 0) && (inputActionMapping.Count > 0);
	}

	private void InitInputMapping(Godot.Collections.Dictionary<string, StringName> dict, StringName eventType)
	{
		exceptionReference.methodName = "InitInputMapping";
		switch(eventType)
		{
			case "keyboard":
			foreach (var (name, action) in dict) {
				var @currentEvent = new InputEventKey();
				@currentEvent.PhysicalKeycode = OS.FindKeycodeFromString(name);
				AddKeyEvent(action, @currentEvent);
			}
			break;

			case "mouse":
			foreach (var (name, action) in dict) {
				var @currentEvent = new InputEventMouseButton();
				AddKeyEvent(action, @currentEvent);
			}
			break;

			case "joystick":
			ExceptionManager.LogWarning(exceptionReference, "Joystick Compatability is not yet developed. Please wait until we finish our integration with Gamepads/Joysticks.");
			// foreach (var (name, action) in dict) {
			// 	var @currentEvent = new InputEventKey();
			// 	@currentEvent.PhysicalKeycode = OS.FindKeycodeFromString(name);
			// 	AddKeyEvent(action, @currentEvent);
			// }
			break;

			default:
			ExceptionManager.LogError(exceptionReference, "Failed due to eventType not being a valid type: " + eventType);
			break;
		}
	}

	private void AddKeyEvent(StringName action, InputEvent @currentEvent)
	{
		if (!InputMap.HasAction(action)) {
			InputMap.AddAction(action, 0.2f);
		}
		InputMap.ActionAddEvent(action, @currentEvent);
	}

	// Initilize the key action status based on the key action mappingCount
	public void InitilizeKeyActions()
	{
		exceptionReference.methodName = "InitilizeKeyActions";
		var kamCount = keyActionMapping.Count;
		var mamCount = mouseActionMapping.Count;
		var iamCount = inputActionMapping.Count;

		// First check if the actions have been loaded
		if (!IsActionsLoaded()) {
			ExceptionManager.LogWarning(exceptionReference, 
										"None of the Actions have been previously defined. We are going to use the default input mapping.");
			UpdateInputMapping(defaultKeyActionMapping, defaultMouseActionMapping, defaultInputActionMapping);
		}
		if (kamCount == 0) {
			keyActionMapping = defaultKeyActionMapping;
			ExceptionManager.LogInfo(exceptionReference, 
									 "Key Action Mapping has been initialized with the default mapping.");
		} else {
			ExceptionManager.LogInfo(exceptionReference, 
									 "Key Action Mapping has been initialized with the loaded mapping.");
		}
		InitInputMapping(keyActionMapping, "keyboard");

		if (mamCount == 0) {
			mouseActionMapping = defaultMouseActionMapping;
			ExceptionManager.LogWarning(exceptionReference, 
										"Mouse Action Mapping has been initialized with the default mapping.");
		} else {
			ExceptionManager.LogInfo(exceptionReference, 
									 "Mouse Action Mapping has been initialized with the loaded mapping.");
		}
		InitInputMapping(mouseActionMapping, "mouse");

		if (iamCount == 0) {
			inputActionMapping = defaultInputActionMapping;
			ExceptionManager.LogWarning(exceptionReference, 
										"Input Action Mapping has been initialized with the default mapping.");
		} else {
			ExceptionManager.LogInfo(exceptionReference, 
									 "Input Action Mapping has been initialized with the loaded mapping.");
		}
		InitInputMapping(inputActionMapping, "joystick");
	}

	// Update the custom input mapping based on the provided mapping
	public void UpdateInputMapping(	Godot.Collections.Dictionary<string, StringName> newKeyActionMapping,
									Godot.Collections.Dictionary<string, StringName> newMouseActionMapping,
									Godot.Collections.Dictionary<string, StringName> newInputActionMapping)
	{
		keyActionMapping = newKeyActionMapping;
		mouseActionMapping = newMouseActionMapping;
		inputActionMapping = newInputActionMapping;
	}

	// save the custom input mapping to a file
	public void SaveInputMappingOld(string filePath) 
	{
		exceptionReference.methodName = "SaveInputMappingOld";
		// using directory access to ensure that the file access can be writable
		using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Write);
		if (file == null) {
			ExceptionManager.LogError(exceptionReference, "Error saving file: " + filePath);
			ExceptionManager.LogError(exceptionReference, "Error: " + FileAccess.GetOpenError());
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
				ExceptionManager.LogError(exceptionReference, 
										  "Error saving file: " + filePath + " Error: " + file.GetError().ToString());
			}
		}
	}

	// Load the custom input mapping from a file
	public void LoadInputMappingOld(string filePath)
	{
		exceptionReference.methodName = "LoadInputMappingOld";
		using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
		if (file == null) {
			ExceptionManager.LogError(exceptionReference, "Error reading file: " + filePath);
			return;
		} else {
			if (file.GetError() == Error.Ok)		{
			using var inputMappingData = Json.ParseString(file.GetAsText());
			GD.Print(inputMappingData);
			file.Flush();
			
			// Assignment mapping data to the current mapping
			// this.keyActionMapping = inputMappingData["keyActionMapping"];
			// this.mouseActionMapping = inputMappingData["mouseActionMapping"];
			// this.inputActionMapping = inputMappingData["inputActionMapping"];

			// // Initilize the key actions based on the loaded mapping
			// InitilizeKeyActions();
			} else {
				ExceptionManager.LogError(exceptionReference, 
										  "Error reading file: " + filePath + " Error: " + file.GetError().ToString());
			}
			file.Close();
		}
	}

	public void SaveInputMapping(StringName filePath)
	{
		exceptionReference.methodName = "SaveInputMapping";
		FileSystemManager.CreateDirectory(filePath);
		FileSystemManager.AddFastFile(filePath, FileAccess.ModeFlags.Write);
		var file = FileSystemManager.GetFastFile(filePath);
		if (file == null) {
			ExceptionManager.LogError(exceptionReference, "Error reading file: " + filePath);
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
			}
			FileSystemManager.CloseFile(filePath);
		}
	}

	public void LoadInputMapping(StringName filePath)
	{
		exceptionReference.methodName = "LoadInputMapping";
		FileSystemManager.AddFastFile(filePath, FileAccess.ModeFlags.Read);
		var file = FileSystemManager.GetFastFile(filePath);
		if (file == null) {
			ExceptionManager.LogError(exceptionReference, "Error reading file: " + filePath);
		} else {
			if (file.GetError() == Error.Ok) {
				using var inputMappingData = Json.ParseString(file.GetAsText());
				InitilizeKeyActions();
			} else {
				ExceptionManager.LogError(exceptionReference, 
										  "Failed to read file: " + filePath + " Error: " + file.GetError().ToString());
			}
			// FileSystemManager.CloseFile(filePath); -- IGNORE ---
			// We don't need to load the input mapping file again, 
			// so we can just remove it from the FFIO system to free up resources.
			// It can get re-added if the user wants to load the input mapping again, 
			// but for now we just want to free up resources since we don't need the file anymore.
			FileSystemManager.RemoveFastFile(filePath);
		}
	}

	// Update Keyboard inputs
	public (Vector2 Direction, float RunSpeed) UpdateDirectionInput() {
		return (Input.GetVector("left", "right", "up", "down"), Input.GetActionStrength("shift"));
	}
}
