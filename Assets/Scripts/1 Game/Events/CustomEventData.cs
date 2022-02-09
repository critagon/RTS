using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomEventData
{
    public UnitData unitData;
    public Unit unit;

    public CustomEventData(UnitData unitData) //constructor for if we have unitdata but nothing for unit
    {
        this.unitData = unitData;
        this.unit = null;
    }

    public CustomEventData(Unit unit)
    {
        this.unitData = null;
        this.unit = unit;
        Debug.Log("Unit in CustomEventData is: " + unit);
    }
}

[System.Serializable]
public class CustomEvent : UnityEvent<CustomEventData> { }








