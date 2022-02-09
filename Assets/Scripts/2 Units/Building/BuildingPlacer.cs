using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer: MonoBehaviour
{
    Building placedBuilding;
    
    Vector3 _lastPlacementPosition;
    public static bool isPlacingBuilding;

    private void Update()
    {
        if (isPlacingBuilding == true)
        {
            if (placedBuilding != null)
            {
                if (Inputs.EscapeDown() || Inputs.RMBUp())
                {
                    CancelPlacedBuilding();
                    return;
                }

                if (placedBuilding.HasValidPlacement && Inputs.LMBUp() && !EventSystem.current.IsPointerOverGameObject())
                {   
                    PlaceBuilding();
                }
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHitInfo;
             
            if (Physics.Raycast(ray, out raycastHitInfo, Mathf.Infinity, Globals.GROUND_LAYER_MASK))
            {
                if (placedBuilding != null)
                {
                    placedBuilding.SetPosition(raycastHitInfo.point);
                    if (_lastPlacementPosition != raycastHitInfo.point)
                    {
                        placedBuilding.CheckValidPlacement();
                    }
                    _lastPlacementPosition = raycastHitInfo.point;
                }
            }
        }
    }

    #region Building Preparation
    public void PreparePlacedBuilding(int buildingDataIndex)
    {
        if (placedBuilding != null)
        {
            CancelPlacedBuilding();
        }
        
        isPlacingBuilding = true;
        
        Building building = new Building(Globals.BUILDING_DATA[buildingDataIndex]);

        building.Transform.GetComponent<BuildingManager>().Initialize(building); // link the data into the manager
        placedBuilding = building;
        _lastPlacementPosition = Vector3.zero;

        if ((placedBuilding != null) && (placedBuilding.isFixed))  //destroy the previous phantom if there is one -- !_placedBuilding.isFixed
        {
            Destroy(placedBuilding.Transform.gameObject);
        }
    }


    void CancelPlacedBuilding()
    {
        if (!(placedBuilding.isFixed))
        {
            Destroy(placedBuilding.Transform.gameObject);
            placedBuilding = null;
            isPlacingBuilding = false;
        }
    }
    #endregion

    #region Place Building
    void PlaceBuilding()
    {
        placedBuilding.PlaceCost();
        isPlacingBuilding = false;

        EventManager.TriggerEvent("UpdateResourceTexts");
        EventManager.TriggerEvent("CheckBuildingButtons");
        //EventManager.TriggerEvent("DeselectAll");

        if ((Inputs.ShiftHold() || (Inputs.CtrlHold()))
            && placedBuilding.CanBuy())
        {
            PreparePlacedBuilding(placedBuilding.DataIndex);
        }
    }
    #endregion
}
