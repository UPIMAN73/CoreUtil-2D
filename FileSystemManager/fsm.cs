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
 * to be safe from 
 */

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

    // Write to file
    // 

    public static void AddFastFile(string path)
    {
        if (!FFIO.ContainsKey(path)) {
            FFIO[path] = false;
        } else {
            GD.PrintErr("[ERROR] The file has already been added to the FFIO system: " + path);
        }
    }

    public static void RemoveFastFile(string path)
    {
        // 
    }
}