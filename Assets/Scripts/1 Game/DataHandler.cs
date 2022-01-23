using UnityEngine;

public static class DataHandler
{
    public static void LoadGameData()
    {
        Globals.UNIT_DATA = Resources.LoadAll<UnitData>("Prefabs & Scriptable Objects/Units") as UnitData[];
        Globals.CHARACTER_DATA = Resources.LoadAll<CharacterData>("Prefabs & Scriptable Objects/Units/Characters") as CharacterData[];
        Globals.BUILDING_DATA = Resources.LoadAll<BuildingData>("Prefabs & Scriptable Objects/Units/Buildings") as BuildingData[];
    }
}