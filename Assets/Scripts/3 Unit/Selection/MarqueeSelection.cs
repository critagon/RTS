using System.Collections.Generic;
using UnityEngine;

public class MarqueeSelection : SelectionFunctions
{
    #region Universal Variables and Setup
    public bool dragMarqueeEnabled;
    public bool isHoldingLMBDownCheck;
    
    bool haveMarqueedObjects;

    Vector3 dragStartPosition;

    Dictionary<int, GameObject> selectedUnits = Globals.SELECTED_UNITS;

    public List<GameObject> marqueedObjects = new List<GameObject>();

    private void OnEnable()
    {
        EventManager.AddListener("isPlacingBuilding", CannotDragMarquee);
        EventManager.AddListener("isNotPlacingBuilding", CanDragMarquee);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener("isPlacingBuilding?", CannotDragMarquee);
        EventManager.RemoveListener("isNotPlacingBuilding", CanDragMarquee);
    }

    //I could only figure out how to set dragMarqueeEnabled with events triggered in BuildingPlacer.cs. Is there a better way to do this?
    void CanDragMarquee()
    {
        dragMarqueeEnabled = true;
    }

    void CannotDragMarquee()
    {
        dragMarqueeEnabled = false;
    }
    #endregion

    private void Update()
    {
        if (marqueedObjects.Count > 0) haveMarqueedObjects = true;
        else haveMarqueedObjects = false;

        if (dragMarqueeEnabled) //and not pressing on a ui element?
        {
            if (Inputs.LMBDown())
            {
                dragStartPosition = Input.mousePosition;
            }

            if (Inputs.LMBHold())
            {
                isHoldingLMBDownCheck = true;
            }

            if (Inputs.LMBUp() && isHoldingLMBDownCheck == true)
            {
                isHoldingLMBDownCheck = false;
                if (haveMarqueedObjects) 
                    SelectInMarquee();
            }

            if (isHoldingLMBDownCheck && dragStartPosition != Input.mousePosition)
                HoverInMarquee();
        }
    }

    #region Hover
    void HoverInMarquee()
    {
        Bounds selectionBounds = Utilities.GetViewportBounds(Camera.main, dragStartPosition, Input.mousePosition);
        GameObject[] selectionBox = GameObject.FindGameObjectsWithTag("Unit");
        
        bool inBounds;

        foreach (GameObject selection in selectionBox)
        {
            inBounds = selectionBounds.Contains(Camera.main.WorldToViewportPoint(selection.transform.position));
            bool canMarqueeSelection = !selection.GetComponent<UnitSelectionHandler>().isABuilding;

            if (inBounds && canMarqueeSelection)
            {
                selection.transform.Find("HoverCircle").gameObject.SetActive(true);
                if (!marqueedObjects.Contains(selection))
                {
                    marqueedObjects.Add(selection);
                }
            }

            if (!inBounds)
            {
                UnHover(selection);
            }
        }
    }

    void UnHover(GameObject selection)
    {
        selection.transform.Find("HoverCircle").gameObject.SetActive(false);
        marqueedObjects.Remove(selection);
    }
    #endregion

    #region Select in Marquee
    void SelectInMarquee()
    {
        foreach(GameObject selection in marqueedObjects)
        {
            selection.transform.Find("HoverCircle").gameObject.SetActive(false);
            MarqueeSelect(selection);
        }
        marqueedObjects.Clear();
    }
    #endregion

    void OnGUI()
    {
        if (isHoldingLMBDownCheck && dragMarqueeEnabled)
        {
            // Create a rect from both mouse positions
            var rect = Utilities.GetScreenRect(dragStartPosition, Input.mousePosition);
            Utilities.DrawScreenRect(rect, new Color(0.5f, 1f, 0.4f, 0.2f));
            Utilities.DrawScreenRectBorder(rect, 1, new Color(0.5f, 1f, 0.4f));
        }
    }
}


