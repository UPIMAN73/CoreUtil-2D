/*
 * @name : CoreEntity Object
 * @author : Joshua Calzadillas
 * @version : 0.1.0
 * @creation-date : February 28, 2026
 * @license : MIT
 * @file : CoreEntity.cs
 */

// Simple Imports
using Godot;
using Godot.Collections;


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
[GlobalClass]
public partial class CoreEntity : Node2D
{
	[Export]
	protected StringName name;

	[Export] 
	protected StringName type;

	// This is where the sprite/animation information goes
	// TODO
	// This will reference a completely different entity

	[Export]
	// Direction (easier for managing more complicated tasks)
	protected Vector2 direction = new Vector2();

	[Export]
	// Spawn Position (this will be used to set the spawn point)
	protected Vector2 spawnPosition = new Vector2();

	[Export]
	// player's move speed	
	protected Vector2 walkSpeed = new Vector2();

	[Export]
	// player's shift speed	
	protected Vector2 runSpeed = new Vector2();

	[Export]
	// player's jump speed
	protected Vector2 jumpSpeed = new Vector2();

	// player's Current speed
	protected Vector2 currentSpeed = new Vector2();

	// player's shift speed
	protected float shiftSpeed = 0.0f;

	// protected Sprite2D sprite;

	// This is where the functions belong
	public override void _Ready() {
		// TODO
	}

	// This is where the movement logic goes
	// TODO
	public void _Process()
	{
		// This is where the movement logic goes
		// TODO
	}

	// This is where the movement logic goes
	public void setDirection(Vector2 newDirection)
	{
		this.direction = newDirection;
	}

	// This is where the animation logic goes
	// TODO

	// This is where the sprite logic goes
	// TODO
}
