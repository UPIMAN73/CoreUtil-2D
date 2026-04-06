using System.Collections;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

[Tool]
public partial class CoreGameUtil : EditorPlugin
{

	private Dictionary libraryTypeMap;
	private Json jsonParser;

	// 
	public override void _EnterTree()
	{
		jsonParser = new Json();
		var file_access_test = FileAccess.Open("res://addons/CoreUtil2D/libdep.json", FileAccess.ModeFlags.Read).GetAsText();
		var parseStatus = jsonParser.Parse(file_access_test);
		GD.Print(parseStatus);
		
		// Check the parsing status based on JSON and print error message if parsing fails.
		if (parseStatus != Error.Ok) {
			GD.PrintErr("Can't parse library dependency file: " + parseStatus);
			GD.PrintErr(jsonParser.GetErrorLine());
			GD.PrintErr(jsonParser.GetErrorMessage());
			return;
		} else {
			GD.Print("Library dependency file parsed successfully.");
			libraryTypeMap = jsonParser.Data.AsGodotDictionary();
			GD.Print(libraryTypeMap);
		}
		
		// Load the Core Game Util addon based on the dictionary
		LoadCoreGameUtilAddon(libraryTypeMap);
	}

	public void CreateCustomType(string CustomName, string BaseName, string ScriptPath, string TexturePath)
	{
		AddCustomType(CustomName, BaseName, 
						GD.Load<Script>(ScriptPath),
						GD.Load<Texture2D>(TexturePath));
						
	}

	// Print out dictionary contents
	public void PrintDictionaryContents(Godot.Collections.Dictionary dictionaryObject)
	{
		// 
		foreach (var (key, value) in dictionaryObject) {
			GD.Print(key, "\t:\t", value);
		}
	}

	public Error CustomTypeCheck(Dictionary customData)
	{
		return (customData == null ? Error.DoesNotExist : 
				(customData.ContainsKey("name") && customData.ContainsKey("base_name") 
				&& customData.ContainsKey("script") && customData.ContainsKey("sprite") 
				? Error.Ok : Error.InvalidData));
	}

	public void LoadCoreGameUtilAddon(Dictionary config)
	{
		PrintDictionaryContents(config);

		if (!config.ContainsKey("custom_types")) {
			GD.PrintErr("Config File not configured properly. Did not see a 'custom_type' key.");
			return;
		} else {
			// setup a static config dictionary so that we aren't wasting time looking up the same items 2 (less instructions)
			var staticConfigList = config["custom_types"].AsGodotArray();

			// Add-on load code goes here.
			foreach (var customTypeDataVariant in staticConfigList) {
				Dictionary customTypeData = customTypeDataVariant.AsGodotDictionary();
				var customTypeDataError = CustomTypeCheck(customTypeData);
				if (customTypeDataError != Error.Ok) {
					GD.PrintErr(customTypeDataError);
					GD.PrintErr(customTypeData);
					continue;
				} else {
					var directory_value = config["parent_directory"] + (customTypeData.ContainsKey("local_directory") ? customTypeData["local_directory"] : "").ToString();
					CreateCustomType(directory_value + customTypeData["name"].ToString(), 
								 directory_value + customTypeData["base_name"].ToString(), 
								 directory_value + customTypeData["script"].ToString(), 
								 directory_value + customTypeData["sprite"].ToString());
					PrintDictionaryContents(customTypeData);
				}
			} 
		}
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
		RemoveCustomType("Player");
	}
}
