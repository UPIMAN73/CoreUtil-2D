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
    public int GetStat(string statName)
    {
        if (maxStats.ContainsKey(statName))
        {
            return maxStats[statName];
        }
        else
        {
            GD.PrintErr($"Stat '{statName}' does not exist.");
            return -1; // Return -1 to indicate an error
        }
    }

    // Method to set the current value of a stat
    public void SetStat(string statName, int value)
    {
        if (maxStats.ContainsKey(statName))
        {
            maxStats[statName] = Mathf.Clamp(value, 0, maxStats[statName]);
        }
        else
        {
            GD.PrintErr($"Stat '{statName}' does not exist.");
        }
    }

    // Method to deplete a stat by a certain amount
    public void DepleteStat(string statName, int amount)
    {
        if (maxStats.ContainsKey(statName))
        {
            int currentValue = GetStat(statName);
            SetStat(statName, currentValue - amount);
        }
        else
        {
            GD.PrintErr($"Stat '{statName}' does not exist.");
        }
    }

    // Method to check if a stat is depleted
    public bool IsStatDepleted(string statName)
    {
        if (maxStats.ContainsKey(statName))
        {
            return GetStat(statName) <= 0;
        }
        else
        {
            GD.PrintErr($"Stat '{statName}' does not exist.");
            return false; // Return false to indicate an error
        }
    }
}

