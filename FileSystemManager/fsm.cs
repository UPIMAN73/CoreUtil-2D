/*
 * @name : FileSystemManager
 * @author : Joshua Calzadillas
 * @version : 0.1.0
 * @creation-date : March 22, 2026
 * @license : MIT
 * @file : fsm.cs
 * @description : This is going to be a concurrency safe file
 * system manager class that allows for Godot to be able to operate
 * on the file systems that are allowed by the program. So it is intended
 * to be safe from race conditions and other concurrency issues.
 */

using System;
using Godot;

public static class FileSystemManager
{
	// Fast File IO
	/*
	 * @object Fast File IO
	 * 
	 * @key : string : File Path 
	 * abs path, relative path 'res://' or 'user://', etc.
	 * 
	 * @value : bool : isReadLocked
	 * Prevents reads from occuring during a write. Also prevents
	 * other writes from occuring to the same file/path.
	 */
	private static Godot.Collections.Dictionary<StringName, FileAccess> FFIO;

	// Check to see if the key is in the dictionary

	// Check to see if a directory exists and if not create it
	public static void CreateDirectory(string path)
	{
	   using var dir = Godot.DirAccess.Open(path.ToString());
	   if (dir == null) {
		   GD.PrintErr("[ERROR] Failed to open directory: " + path);
		   return;
	   } else {
		var openError = DirAccess.GetOpenError();
		if (openError != Error.Ok) {
			GD.PrintErr("[ERROR] Failed to open directory: " + path + " Error: " + openError);
			return;
		} else {
			GD.Print("[INFO] Directory opened successfully: " + path);
		
			if (!dir.DirExists(path)) {
				var err = dir.MakeDir(path);
				if (err != Error.Ok) {
					GD.PrintErr("[ERROR] Failed to create directory: " + path + " Error: " + err);
				} else {
					GD.Print("[INFO] Directory created successfully: " + path);
				}
			}
		}
		}
	}

	public static void FFIOInit()
	{
        if (FFIO == null) {
		    FFIO = new Godot.Collections.Dictionary<StringName, FileAccess>();
        }
	}

	public static void FFIOShutdown()
	{
		foreach (var (path, file) in FFIO) {
			GD.Print("[INFO] Closing file: " + path);
			try {
				file.Flush();
			} catch (Exception ex) {
				GD.PrintErr("[ERROR] Failed to flush file: " + path + " Exception: " + ex.Message);
			}
			file.Close();
		}
		FFIO.Clear();
	}

	
	public static void AddFastFile(StringName path)
	{
		if (FFIO == null) {
			FFIOInit();
		}
		if (!FFIO.ContainsKey(path)) {
			if (!path.ToString().IsValidFileName()) {
				GD.PrintErr("[ERROR] The file name is not valid: " + path);
				return;
			}
			CreateDirectory(path);
			FFIO[path] = FileAccess.Open(path, FileAccess.ModeFlags.ReadWrite);
			if (FFIO[path] == null) {
				GD.PrintErr("[ERROR] Failed to open file: " + path);
				return;
			} else if (FFIO[path].GetError() != Error.Ok) {
				GD.PrintErr("[ERROR] Failed to open file: " + path + " Error: " + FFIO[path].GetError());
				FFIO.Remove(path);
				return;
			}
		} else {
			GD.PrintErr("[ERROR] The file is already in the FFIO system: " + path);
		}
	}

	public static void RemoveFastFile(StringName path)
	{
		if (FFIO == null) {
			GD.PrintErr("[ERROR] FFIO system is not initialized. Cannot remove file: " + path);
			return;
		}
		CloseFile(path);
	}

	public static FileAccess GetFastFile(StringName path)
	{         
		if (FFIO == null) {
			GD.PrintErr("[ERROR] FFIO system is not initialized. Cannot get file: " + path);
			return null;
		}
		if (FFIO.ContainsKey(path)) {
			return FFIO[path];
		} else {
			GD.PrintErr("[ERROR] The file is not in the FFIO system: " + path);
			return null;
		}
	}

	public static void CloseFile(StringName path)
	{
		if (FFIO == null) {
			GD.PrintErr("[ERROR] FFIO system is not initialized. Cannot close file: " + path);
			return;
		}
		if (FFIO.ContainsKey(path)) {
			FFIO[path].Close();
			FFIO.Remove(path);
		} else {
			GD.PrintErr("[ERROR] The file is not in the FFIO system: " + path);
		}
	}
}
