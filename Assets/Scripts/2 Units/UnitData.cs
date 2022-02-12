using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Objects/Unit", order = 2)] //do I need a separate unit asset menu? I have characters and buildings, don't think there would be a scenario where an object is neither.
[System.Serializable]

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
            {
                return false;
            }

        return true;
    }
}












