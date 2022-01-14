using System.Collections.Generic;
using UnityEngine;

public class Globals
{
    public static int GROUND_LAYER_MASK = 1 << 8;

    public static BuildingData[] BUILDING_DATA = new BuildingData[] { };

    public static Dictionary<string, GameResource> GAME_RESOURCES = new Dictionary<string, GameResource>()
    {
        {"food", new GameResource ("Food", 0) },
        {"wood", new GameResource("Wood", 0) },
        {"stone", new GameResource("Stone", 0) },
        {"gold", new GameResource ("Gold", 0) }, //name, initial amount
    };

    public static Dictionary<int, GameObject> SELECTED_UNITS= new Dictionary<int, GameObject>();

    public static bool SomethingSelected()
    {
        if (SELECTED_UNITS.Count > 0)
        {
            return true;
        }
        else return false;
    }
}

/****************************************************************************************************************************************************/

public class GameResource //make into separate script if encounter problems
{
    string _name;
    int _currentAmount;

    public GameResource(string name, int initialAmount)
    {
        _name = name;
        _currentAmount = initialAmount;
        _currentAmount = 300; //for testing
    }

    public void AddAmount(int value)
    {
        _currentAmount += value;
        if (_currentAmount < 0) _currentAmount = 0;
    }

    public string Name { get => _name; } //necessary?, what is get? help

    public int Amount { get => _currentAmount; }
}