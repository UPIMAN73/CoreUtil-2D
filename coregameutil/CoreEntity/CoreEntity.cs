/*
 * @name : CoreEntity Object
 * @author : Joshua Calzadillas
 * @version : 0.1.0
 * @creation-date : February 28, 2026
 * @license : MIT
 */

// Simple Imports
using Godot;
using Godot.Collections;

[GlobalClass]
/*
 * CoreEntity
 * This is a simple example of a global class in Godot. This class, `CoreEntity`, extends
 * Node2D. For the Core Utility System, this is the most basic class required to optimize 
 * movement, and can be used as a base for more complex objects that will be referenced in
 * the future. The most critical component of this object is the how it interacts with every
 * other node in the game. This allows for animations, sprites, and other components to be referenced.
 * This also allows for base variables such as directions, movement speed, spawn position, etc.
 * This class also allows for custom naming, type references (character, background, etc.).
 * It also allows for more 
 */
public partial class CoreEntity : Node2D
{
    [Export]
    private StringName name;

    [Export] 
    private StringName type;

    // This is where the sprite/animation information goes
    // TODO

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
	private Vector2 walkSpeed = new Vector2();

	[Export]
	// player's shift speed	
	private Vector2 runSpeed = new Vector2();

	[Export]
	// player's jump speed
	private Vector2 jumpSpeed = new Vector2();

	// player's Current speed
	private Vector2 currentSpeed = new Vector2();

    // This is where the functions belong

    public override void _Ready();
    public override void _Process();
}