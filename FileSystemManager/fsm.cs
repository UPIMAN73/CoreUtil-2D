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
	private static StringName CLASS_NAME = "FileSystemManager";
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

	/// <summary>
	/// Checks to see if a directory exists and if not creates it. 
	/// It will also log the process of creating the directory in a way that is easy to read and understand.
	/// </summary>
	/// <param name="path">The path to the directory to check or create.</param>
	/// <para><code>
	/// FileSystemManager.CreateDirectory("user://my_directory");
	/// </code></para>
	public static void CreateDirectory(StringName path)
	{
		StringName methodName = "CreateDirectory";
		var absBasePath = path.ToString().Split("://")[0] + "://";
		var dirPath = path.ToString().GetBaseDir();
		using var dir = DirAccess.Open(absBasePath);
		if (dir == null) {
			ExceptionManager.LogError(CLASS_NAME, methodName, "Failed to open directory: " + path + " on " + absBasePath);
			return;
		} else {
			var openError = DirAccess.GetOpenError();
			if (openError != Error.Ok) {
				ExceptionManager.LogError(CLASS_NAME, methodName, "Failed to open directory: " + absBasePath + " Error: " + openError);
				return;
			} else {
				ExceptionManager.LogInfo(CLASS_NAME, methodName, "Directory opened successfully: " + absBasePath);
			
				if (!dir.DirExists(dirPath)) {
					var err = dir.MakeDir(dirPath);
					if (err != Error.Ok) {
						ExceptionManager.LogError(CLASS_NAME, methodName, "Failed to create directory: " + dirPath + " Error: " + err);
					} else {
						ExceptionManager.LogInfo(CLASS_NAME, methodName, "Directory created successfully: " + dirPath);
					}
				}
			}
		}
	}

	/// <summary>
	/// Initializes the Fast File IO system by creating a new dictionary to store the file access objects. 
	/// This method should be called before using any of the other methods in the FileSystemManager that rely on the FFIO system.
	///  It will also log the initialization process in a way that is easy to read and understand.
	/// <para>EXAMPLE:</para>
	/// <para><code>
	/// FileSystemManager.FFIOInit();
	/// </code></para>
	/// <para>NOTE:</para>
	/// <para>This method must be called before using any of the other methods in the FileSystemManager that rely on the FFIO system.</para>
	/// <para>It is recommended to call this method in the '_Ready()' function of a node that is guaranteed to be initialized before any other nodes that may use the FFIO system.</para>
	/// </summary>
	/// <para><code>
	/// FileSystemManager.FFIOInit();
	/// </code></para>
	public static void FFIOInit()
	{
		if (FFIO == null) {
			FFIO = new Godot.Collections.Dictionary<StringName, FileAccess>();
			GD.Print("[INFO] Initilized FFIO System");
		}
	}

	/// <summary>
	/// Shuts down the Fast File IO system by closing all open file access objects and clearing
	/// the dictionary. This method should be called when the program is closing or when the FFIO system is no longer needed. 
	/// It will also log the shutdown process in a way that is easy to read and understand.
	/// <para>EXAMPLE:</para>
	/// <para><code>
	/// FileSystemManager.FFIOShutdown();
	/// </code></para>
	/// <para>NOTE:</para>
	/// <para>This method should be called when the program is closing or when the FFIO system is no longer needed to ensure that all file access objects are properly closed and that there are no memory leaks or other issues related to open files.</para>
	/// <para>It is recommended to call this method in the '_ExitTree()' function of a node that is guaranteed to be the last node to be active before the program closes or before the FFIO system is no longer needed.</para>
	/// <para>Calling this method will close all open file access objects in the FFIO system and clear the dictionary, so it should be used with caution to avoid accidentally closing files that are still in use.</para>
	/// <para>After calling this method, the FFIO system will need to be re-initialized with 'FileSystemManager.FFIOInit()' before it can be used again.</para>
	/// It will also log the shutdown process in a way that is easy to read and understand, including any errors that may occur during the shutdown
	/// </summary>
	public static void FFIOShutdown()
	{
		StringName methodName = "FFIOShutdown";
		foreach (var (path, file) in FFIO) {
			ExceptionManager.LogInfo(CLASS_NAME, methodName, "Closing file: " + path);
			try {
				file.Flush();
			} catch (Exception ex) {
				ExceptionManager.LogError(CLASS_NAME, methodName, "Failed to flush file: " + path + " Exception: " + ex.Message);
			}
			file.Close();
		}
		FFIO.Clear();
	}

	/// <summary>
	/// Adds a file to the Fast File IO system.
	/// This method will attempt to open the file at the specified path with the specified mode and if successful, it will add the file access object to the FFIO dictionary. It will also log the process of adding the file in a way that is easy to read and understand.
	/// <para>EXAMPLE:</para>
	/// <para><code>
	/// FileSystemManager.AddFastFile("user://my_file.txt", FileAccess.ModeFlags.Write);
	/// </code></para>
	/// <para>NOTE:</para>
	/// <para>The file name must be valid and the file must be able to be opened</para>
	/// </summary>
	/// <param name="path">The path to the file to add.</param>
	/// <param name="fileMode">The mode in which to open the file.</param>
	public static void AddFastFile(StringName path, FileAccess.ModeFlags fileMode)
	{
		StringName methodName = "AddFastFile";
		if (FFIO == null) {
			ExceptionManager.LogError(CLASS_NAME, methodName, "FFIO has not been initilized yet. Please do so by running the 'FileSystemManager.FFIOInit()' in your '_Ready()' function");
			return;
		}

		var pathString = path.ToString();
		if (!FFIO.ContainsKey(path)) {
			if (!pathString.GetFile().IsValidFileName()) {
				ExceptionManager.LogError(CLASS_NAME, methodName, "The file name is not valid: " + pathString);
				return;
			} else {
				ExceptionManager.LogInfo(CLASS_NAME, methodName, "Validity check complete. Begin openning file: " + pathString);
				GD.Print(pathString.GetBaseDir() + pathString.GetFile());
				using var file = FileAccess.Open(pathString, fileMode);
				GD.Print(file);
				GD.Print(fileMode);
				if (file == null) {
					ExceptionManager.LogError(CLASS_NAME, methodName, "Failed to open file: " + pathString);
				} else {
					if (file.GetError() != Error.Ok) {
						ExceptionManager.LogError(CLASS_NAME, methodName, "Failed to open file: " + pathString + " Error: " + file.GetError());
					} else
					{
						ExceptionManager.LogInfo(CLASS_NAME, methodName, "File has opened succesfully: " + pathString);
						FFIO[path] = file;
					}
				}
			}
		} else {
			ExceptionManager.LogError(CLASS_NAME, methodName, "The file is already in the FFIO system: " + pathString);
		}
	}

	/// <summary>
	/// Removes a file from the Fast File IO system by closing the file access object and removing it from the dictionary. 
	/// This method should be called when you are done using a file that was added to the FFIO system. 
	/// It will also log the process of removing the file in a way that is easy to read and understand.
	/// 
	/// <para>EXAMPLE:</para>
	/// <para><code>
	/// FileSystemManager.RemoveFastFile("user://my_file.txt");
	/// </code></para>
	/// </summary>
	/// <param name="path">The path to the file to remove.</param>
	public static void RemoveFastFile(StringName path)
	{
		StringName methodName = "RemoveFastFile";
		if (FFIO == null) {
			ExceptionManager.LogError(CLASS_NAME, methodName, "FFIO system is not initialized. Cannot remove file: " + path);
			return;
		}
		CloseFile(path);
	}

	/// <summary>
	/// Gets a file from the Fast File IO system.
	/// <param name="path">The path to the file to get.</param>
	/// <para>
	/// <code>
	/// var file = FileSystemManager.GetFastFile("user://my_file.txt");
	/// </code>
	/// </para>
	/// </summary>
	/// <returns>The FileAccess object for the requested file, or null if the file is not found.</returns>
	public static FileAccess GetFastFile(StringName path)
	{         
		StringName methodName = "GetFastFile";
		if (FFIO == null) {
			ExceptionManager.LogError(CLASS_NAME, methodName, "FFIO system is not initialized. Cannot get file: " + path);
			return null;
		}
		if (FFIO.ContainsKey(path)) {
			return FFIO[path];
		} else {
			ExceptionManager.LogError(CLASS_NAME, methodName, "The file is not in the FFIO system: " + path);
			return null;
		}
	}

	public static void CloseFile(StringName path)
	{
		StringName methodName = "CloseFile";
		if (FFIO == null) {
			ExceptionManager.LogError(CLASS_NAME, methodName, "FFIO system is not initialized. Cannot close file: " + path);
			return;
		}
		if (FFIO.ContainsKey(path)) {
			FFIO[path].Close();
			FFIO.Remove(path);
		} else {
			ExceptionManager.LogError(CLASS_NAME, methodName, "The file is not in the FFIO system: " + path);
		}
	}
}
