/*
 * @name : Player Object
 * @author : Joshua Calzadillas
 * @version : 0.1.0
 * @creation-date : February 17, 2026
 * @license : MIT
 * @file : StatNode.cs
 * @description : A node that will handle the stats of a character, 
 * such as health, mana, stamina, etc. This will be used to manage the character's 
 * stats and provide an interface for other nodes to interact with the stats. 
 * This will also handle the regeneration of stats over time, as well as the depletion 
 * of stats when certain actions are performed. This will be a child node of the character node, 
 * and will be responsible for managing the character's stats and providing an interface for other
 * nodes to interact with the stats.
 */

// Simple Imports
using Godot;

[GlobalClass]
public partial class StatNode : Node
{
    // Exception Reference
    private ExceptionReference exceptionReference = new ExceptionReference{ className = "StatNode" };

    // Stat variables
    [Export]
    private Godot.Collections.Dictionary<string, int> maxStats = new Godot.Collections.Dictionary<string, int>()
    {
        {"health", 100},
        {"mana", 100},
        {"stamina", 100}
    };

    // Regeneration rates
    [Export]
    private Godot.Collections.Dictionary<string, float> regenRates = new Godot.Collections.Dictionary<string, float>()
    {
        {"health", 1.0f},
        {"mana", 1.0f},
        {"stamina", 1.0f}
    };

    public override void _Process(double delta)
    {
        RegenerateStats(delta);
    }

    // Method to regenerate stats over time
    private void RegenerateStats(double delta)
    {
        foreach (var stat in maxStats.Keys)
        {
            int currentValue = GetStat(stat);
            int maxValue = maxStats[stat];
            float regenRate = regenRates[stat];

            if (currentValue < maxValue)
            {
                int newValue = Mathf.Min(currentValue + Mathf.RoundToInt(regenRate * (float)delta), maxValue);
                SetStat(stat, newValue);
            }
        }
    }

    // Method to get the current value of a stat
    /// <summary>
    /// <param name="statName">The name of the stat to get.</param>
    ///  Gets the current value of a stat. 
    /// This will be used to retrieve the current value of a stat when it is needed, 
    /// such as when the player takes damage or uses mana. 
    /// It will also log the process of getting a stat in a way that is easy to read and understand.
    /// <para><code>
    /// int currentHealth = statNode.GetStat("health");
    /// </code></para>
    /// <returns>
    /// The current value of the specified stat.
    /// </returns>
    /// </summary>
    public int GetStat(string statName)
    {
        exceptionReference.methodName = "GetStat";
        if (maxStats.ContainsKey(statName))
        {
            return maxStats[statName];
        }
        else
        {
            ExceptionManager.LogError(exceptionReference, $"Stat '{statName}' does not exist.");
            return -1; // Return -1 to indicate an error
        }
    }

    // Method to set the current value of a stat
    /// <summary>
    ///  Sets the current value of a stat. 
    /// This will be used to update the value of a stat when it is depleted or regenerated. 
    /// It will also log the process of setting a stat in a way that is easy to read and understand.
    /// <para><code>
    /// statNode.SetStat("health", 50);
    /// </code></para>
    /// <param name="statName">The name of the stat to set.</param>
    /// <param name="value">The new value for the stat.</param>
    /// </summary>
    public void SetStat(string statName, int value)
    {
        exceptionReference.methodName = "SetStat";
        if (maxStats.ContainsKey(statName))
        {
            maxStats[statName] = Mathf.Clamp(value, 0, maxStats[statName]);
        }
        else
        {
            ExceptionManager.LogError(exceptionReference, $"Stat '{statName}' does not exist.");
        }
    }

    // Method to deplete a stat by a certain amount
    /// <summary>
    /// Depletes a stat by a certain amount. 
    /// This will be used when the player performs an action that depletes a stat, 
    /// such as taking damage or using mana. 
    /// It will also log the process of depleting a stat in a way that is easy to read and understand.
    /// <param name="statName">The name of the stat to deplete.</param>
    /// <param name="amount">The amount to deplete the stat by.</param>
    /// <para><code>
    /// statNode.DepleteStat("health", 10);
    /// </code></para>
    /// </summary>
    public void DepleteStat(string statName, int amount)
    {
        exceptionReference.methodName = "DepleteStat";
        if (maxStats.ContainsKey(statName))
        {
            int currentValue = GetStat(statName);
            SetStat(statName, currentValue - amount);
        }
        else
        {
            ExceptionManager.LogError(exceptionReference, $"Stat '{statName}' does not exist.");
        }
    }

    // Method to check if a stat is depleted
    /// <summary>
    ///    Checks if a stat is depleted (i.e., its value is less than or equal to zero). 
    /// This will be used to determine if the player has run out of a certain stat, 
    /// such as health, mana, or stamina. 
    /// It will also log the process of checking if a stat is depleted in a way that is easy to read and understand.
    /// <para><code>
    /// bool isHealthDepleted = statNode.IsStatDepleted("health");
    /// </code></para>
    /// <param name="statName">The name of the stat to check.</param>
    /// <returns>
    /// True if the stat is depleted, false otherwise.
    /// </returns>
    /// </summary>
    public bool IsStatDepleted(string statName)
    {
        exceptionReference.methodName = "IsStatDepleted";
        if (maxStats.ContainsKey(statName))
        {
            return GetStat(statName) <= 0;
        }
        else
        {
            ExceptionManager.LogError(exceptionReference, $"Stat '{statName}' does not exist.");
            return false; // Return false to indicate an error
        }
    }
}

