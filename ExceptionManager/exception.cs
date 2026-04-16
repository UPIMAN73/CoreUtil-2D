/*
 * @name : ExceptionManager
 * @author : Joshua Calzadillas
 * @version : 0.1.0
 * @creation-date : March 22, 2026
 * @license : MIT
 * @file : exception.cs
 * @description : This is going to be a concurrency safe exception manager class that allows for Godot to be able to operate
 * on exceptions that are thrown by the program. So it is intended to be safe from race conditions and other concurrency issues. 
 * It will also allow for the program to be able to log exceptions in a way that is easy to read and understand.
 */

using System.Collections;
using Godot;

/// <summary>
/// This struct is going to be used to store a reference to an exception that is thrown by
/// the program.
/// </summary>
public struct ExceptionReference
{
    public StringName className;
    public StringName methodName;
    public System.Exception exception;
}

public static class ExceptionManager
{

    // Exception Reference
    private static ExceptionReference exceptionReference = new ExceptionReference{ className = "ExceptionManager" };

    // TODO: Add a way to store exceptions in a way that is safe
    // Queue<ExceptionReference> exceptionQueue = new Queue<ExceptionReference>();

	/// <summary>
    /// Formats a log message with the class name, method name, and the message itself.
    /// </summary>
    /// <param name="className">Class Name</param>
    /// <param name="methodName">Method Name</param>
    /// <param name="message">Message</param>
    /// <returns>
    /// The formatted log message.
    /// </returns>

	public static StringName formatMessage(ExceptionReference eref, StringName message)
	{
		return eref.className + "." + eref.methodName + "(): " + message;
	}

    /// <summary>
    /// This method is going to be used to log error messages that are thrown by the program. It will use the Godot logging system to log the error in a way that is easy to read and understand.
    /// </summary>
    /// <param name="className"></param>
    /// <param name="methodName"></param>
    /// <param name="message"></param>
    /// <example>
    /// try {
    ///     // Some code that may throw an exception
    /// } catch (System.Exception ex) {
    ///     ExceptionManager.LogError("MyClass", "MyMethod", "This is an error message.");
    /// }
    /// </example>
	public static void LogError(ExceptionReference eref, StringName message)
	{
		GD.PrintErr("[ERROR] " + formatMessage(eref, message));
	}

    /// <summary>
    /// This method is going to be used to log warning messages that are thrown by the program. It will use the Godot logging system to log the warning in a way that is easy to read and understand.
    /// </summary>
    /// <param name="className">The name of the class where the warning was thrown.</param>
    /// <param name="methodName">The name of the method where the warning was thrown.</param>
    /// <param name="message">The warning message to be logged.</param>
    /// <example>
    /// ExceptionManager.LogWarning("MyClass", "MyMethod", "This is a warning message.");
    /// </example>
	public static void LogWarning(ExceptionReference eref, StringName message)
	{
		GD.Print("[WARNING] " + formatMessage(eref, message));
	}

    /// <summary>
    /// This method is going to be used to log informational messages that are thrown by the program. 
    /// It will use the Godot logging system to log the information in a way that is easy to read and understand.
    /// </summary>
    /// <param name="className">The name of the class where the informational message was thrown.</param>
    /// <param name="methodName">The name of the method where the informational message was thrown.</param>
    /// <param name="message">The informational message to be logged.</param>
    /// <example>
    /// ExceptionManager.LogInfo("MyClass", "MyMethod", "This is an informational message.");
    /// </example>
	public static void LogInfo(ExceptionReference eref, StringName message)
	{
		GD.Print("[INFO] " + formatMessage(eref, message));
	}

    /// <summary>
    /// This method is going to be used to log debug messages that are thrown by the program. It will use the Godot logging system to log the debug information in a way that is easy to read and understand.
    /// </summary>
    /// <param name="className">The name of the class where the debug message was thrown.</param>
    /// <param name="methodName">The name of the method where the debug message was thrown.</param>
    /// <param name="message">The debug message to be logged.</param>
    /// <example>
    /// ExceptionManager.LogDebug("MyClass", "MyMethod", "This is a debug message.");
    /// </example>
	public static void LogDebug(ExceptionReference eref, StringName message)
	{
		GD.Print("[DEBUG] " + formatMessage(eref, message));
	}

    /// <summary>
    /// This method is going to be used to log critical messages that are thrown by the program. It will use the Godot logging system to log the critical information in a way that is easy to read and understand.
    /// </summary>
    /// <param name="className">The name of the class where the critical message was thrown.</param>
    /// <param name="methodName">The name of the method where the critical message was thrown.</param>
    /// <param name="message">The critical message to be logged.</param>
    /// <example>
    /// ExceptionManager.LogCritical("MyClass", "MyMethod", "This is a critical message.");
    /// </example>
	public static void LogCritical(ExceptionReference eref, StringName message)
	{
		GD.PrintErr("[CRITICAL] " + formatMessage(eref, message));
	}

    /// <summary>
    /// This method is going to be used to log exceptions that are thrown by the program. It will use the Godot logging system to log the exception in a way that is easy to read and understand. It will also include the stack trace of the exception to help with debugging.
    /// </summary>
    /// <param name="ex">The exception that is being logged.</param>
    /// <param name="className">The name of the class where the exception was thrown.</param>
    /// <param name="methodName">The name of the method where the exception was thrown.</param>
    /// <example>
    /// try {
    ///     // Some code that may throw an exception
    /// } catch (System.Exception ex) {
    ///     ExceptionManager.LogException(ex, "MyClass", "MyMethod");
    /// }
    /// </example>
	public static void LogException(ExceptionReference eref)
	{
		if (eref.exception == null) {
			return;
		} else {
			GD.PrintErr("[EXCEPTION] " + formatMessage(eref, eref.exception.GetType().Name + ": " + eref.exception.Message));
			GD.PrintErr("[STACK TRACE] " + eref.exception.StackTrace);
		}
	}

	/// <summary>
    /// Writes log messages to a specified file.
    /// This method uses the <see cref="FileSystemManager"/> to write the log messages to a file in a way that is safe from
    /// race conditions and other concurrency issues. It allows for the program to log exceptions in a way that is easy to read and understand.
    /// </summary>
    /// <param name="logMessage">The log message to be written to the file.</param>
    /// <param name="filePath">The path to the file where the log message will be written. This can be an absolute path or a relative path using 'res://' or 'user://'.</param>
    /// <remarks>
    /// <para>This method will automatically create the directory if it does not exist and will handle any exceptions that may occur during the file writing process.</para>
    /// </remarks>
    /// <example>
    /// <literal>ExceptionManager.WriteLogToFile("This is a log message.", "user://logs/log.txt");</literal>
    /// </example>
	public static void WriteLogToFile(StringName logMessage, StringName filePath)
	{
		try {
			FileSystemManager.CreateDirectory(filePath);
			FileSystemManager.AddFastFile(filePath, Godot.FileAccess.ModeFlags.Write);
			var file = FileSystemManager.GetFastFile(filePath);
			if (file != null) {
				file.StoreLine(logMessage);
				file.Flush();
				FileSystemManager.CloseFile(filePath);
			} else {
				LogError(new ExceptionReference { className = "ExceptionManager", methodName = "WriteLogToFile" }, "Failed to write log to file: " + filePath);
			}
		} catch (System.Exception ex) {
			LogException(new ExceptionReference { exception = ex });
		}
	}
}
