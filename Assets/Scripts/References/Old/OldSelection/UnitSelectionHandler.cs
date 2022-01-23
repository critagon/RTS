/*using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionHandler : MonoBehaviour
{
    #region Global Variables and Setup
    Dictionary<int, GameObject> selectedUnits = Globals.SELECTED_UNITS;
    GameObject hoverObject;

    int id;
    public bool isABuilding;
    public bool canBeSelected = true;
    public bool isHoveringOver;
    public bool isNotPlacingBuilding;

    UnitManager unitManager;
    BuildingManager buildingManager;


    #region MessyCode
    
    void IsPlacingBuildingCannotSelect(CustomEventData unitData)
    {
        isNotPlacingBuilding = false;
    }

    void IsNotPlacingBuildingCanSelect(CustomEventData unitData)
    {
        isNotPlacingBuilding = true;
    }

    #endregion

    //wont show healthbar if hovering

    private void Awake()
    {
        id = gameObject.GetInstanceID();

        unitManager = GetComponent<UnitManager>();
        buildingManager = GetComponent<BuildingManager>();

        if (GetComponent<BuildingManager>() != null)
        {
            isABuilding = true;
        }

        EventManager.AddTypedListener("isPlacingBuilding", IsPlacingBuildingCannotSelect);
        EventManager.AddTypedListener("isNotPlacingBuilding", IsNotPlacingBuildingCanSelect);
    }
    #endregion

    #region Update
    void Update()
    {

        if (isABuilding && !buildingManager._isActive) canBeSelected = false;
        else canBeSelected = true; 
        
        if (Input.GetKeyUp(KeyCode.K)) //debug
        {
            foreach (KeyValuePair<int, GameObject> pair in selectedUnits)
            {
                print(pair);
            }
            if (selectedUnits.Count == 0)
            {
                print("No units selected.");
            }
        }

        if ((Inputs.LMBDown() && isHoveringOver && canBeSelected && isNotPlacingBuilding) //single select
            && !(Inputs.ShiftHold() || (Inputs.CtrlHold())))
        {
            Select1(hoverObject);
        }

        if ((Inputs.LMBDown() && isHoveringOver && canBeSelected) 
           && (Inputs.ShiftHold() || (Inputs.CtrlHold()))) 
        {
            if (!selectedUnits.ContainsKey(id))//shift select
                ShiftSelect(hoverObject);
            else //deselect 1
                Deselect1(hoverObject);
        }

        if ((Globals.SELECTED_UNITS.Count > 0) && !(Inputs.ShiftHold()) || (Inputs.CtrlHold())) //deselect all
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                DeselectAll();

            if (Inputs.LMBDown())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHitInfo;
                if (Physics.Raycast(ray, out raycastHitInfo, Mathf.Infinity))
                {
                    if (raycastHitInfo.transform.tag == "Terrain")
                        DeselectAll();
                }
            }
        }
    }
    #endregion

    #region OnMouse
    void OnMouseEnter()
    {
        isHoveringOver = true;
        hoverObject = transform.gameObject;
    }

    void OnMouseExit()
    {
        isHoveringOver = false;
    }
    #endregion
}*/

