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
	public static void CreateDirectory(StringName path)
	{
        var absBasePath = path.ToString().Split("://")[0] + "://";
        var dirPath = path.ToString().GetBaseDir();
        using var dir = DirAccess.Open(absBasePath);
        if (dir == null) {
            GD.PrintErr("[ERROR] Failed to open directory: " + path + " on " + absBasePath);
            return;
        } else {
            var openError = DirAccess.GetOpenError();
            if (openError != Error.Ok) {
                GD.PrintErr("[ERROR] Failed to open directory: " + absBasePath + " Error: " + openError);
                return;
            } else {
                GD.Print("[INFO] Directory opened successfully: " + absBasePath);
            
                if (!dir.DirExists(dirPath)) {
                    var err = dir.MakeDir(dirPath);
                    if (err != Error.Ok) {
                        GD.PrintErr("[ERROR] Failed to create directory: " + dirPath + " Error: " + err);
                    } else {
                        GD.Print("[INFO] Directory created successfully: " + dirPath);
                    }
                }
            }
        }
	}

	public static void FFIOInit()
	{
        if (FFIO == null) {
		    FFIO = new Godot.Collections.Dictionary<StringName, FileAccess>();
            GD.Print("[INFO] Initilized FFIO System");
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

	
	public static void AddFastFile(StringName path, FileAccess.ModeFlags fileMode)
	{
		if (FFIO == null) {
			GD.PrintErr("[ERROR] FFIO has not been initilized yet. Please do so by running the 'FileSystemManager.FFIOInit()' in your '_Ready()' function");
            return;
		}

        var pathString = path.ToString();
		if (!FFIO.ContainsKey(path)) {
			if (!pathString.GetFile().IsValidFileName()) {
				GD.PrintErr("[ERROR] The file name is not valid: " + pathString);
				return;
			} else {
                GD.Print("[INFO] Validity check complete. Begin openning file: " + pathString);
                GD.Print(pathString.GetBaseDir() + pathString.GetFile());
                using var file = FileAccess.Open(pathString, fileMode);
                GD.Print(file);
                GD.Print(fileMode);
                if (file == null) {
                    GD.PrintErr("[ERROR] Failed to open file: " + pathString);
                } else {
                    if (file.GetError() != Error.Ok) {
                        GD.PrintErr("[ERROR] Failed to open file: " + pathString + " Error: " + file.GetError());
                    } else
                    {
                        GD.Print("[INFO] File has opened succesfully: " + pathString);
                        FFIO[path] = file;
                    }
                }
            }
		} else {
			GD.PrintErr("[ERROR] The file is already in the FFIO system: " + pathString);
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
