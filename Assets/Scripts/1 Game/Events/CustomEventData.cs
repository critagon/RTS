using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomEventData
{
    UnitData unitData;
    public Unit unit;

    public CustomEventData(UnitData unitData)
    {
        this.unitData = unitData;
        this.unit = null;
    }

    public CustomEventData(Unit unit)
    {
        this.unitData = null;
        this.unit = unit;
    }
}

[System.Serializable]
public class CustomEvent : UnityEvent<CustomEventData> { }








