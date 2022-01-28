using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : UnitManager
{
    public Transform parent;
    Character _character;

    protected override Unit Unit //what is the purpose of this?
    {
        get { return _character; }
        set { _character = value is Character character ? character : null; }
    }

    void Update()
    {
        if (parent == null)
        {
            parent = GameObject.Find("Characters").transform; 
            transform.SetParent(parent);
        }

        UpdateUnitDisplay();
    }

    protected override void UpdateUnitDisplay()
   {
        //HoverCircle
        if (isHoveringOver && IsDrawingMarquee() == false && isPlacingBuilding == false)
        {
            transform.Find("HoverCircle").gameObject.SetActive(true);
            HealthbarCreate();
        }

        if (!isHoveringOver)
        {
            transform.Find("HoverCircle").gameObject.SetActive(false);
            if (!selectedUnits.ContainsKey(id))
            {
                HealthbarDestroy();
            }
        }
    }
}
