using System.Collections.Generic;
using UnityEngine;

public class UnitData : ScriptableObject
{
    public GameObject prefab;
    public string code;
    public string unitName;
    public string description;
    public int HP;
    public int maxHP;
    //public int unitSpeed;
    //public int maxSpeed;
    //public string faction;
    //public int playerNumber;
    //public int teamNumber;
    public List<ResourceValue> cost;

    public bool CanBuy()
    {
        foreach (ResourceValue resource in cost)
            if (Globals.GAME_RESOURCES[resource.code].Amount < resource.amount)
                return false;
        return true;
    }
}

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Unit/Character", order = 1)]
public class CharacterData : UnitData
{
    //public List<UnitProduction> unitProduction;
    //public List<BuildingProduction> buildingProduction;
}

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/Unit/Building", order = 2)]
public class BuildingData : UnitData
{
    //public List<UnitProduction> unitProduction;
    //public List<BuildingProduction> buildingProduction;
}

[System.Serializable]
public class ResourceValue //make into separate script if there are problems
{
    public string code = "";
    public int amount = 0;
}








