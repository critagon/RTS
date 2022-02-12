using System.Collections.Generic;
using UnityEngine;

public class MarqueeSelection : MonoBehaviour
{
    #region Universal Variables and Setup
    public static bool isDrawingMarquee;
    
    public static bool haveMarqueedObjects;

    Vector3 dragStartPosition;

    Dictionary<int, GameObject> selectedUnits = Globals.SELECTED_UNITS;

    public static List<GameObject> marqueedObjects = new List<GameObject>();

    UnitManager unitManager;

    bool dragMarqueeEnabled()
    {
        if (BuildingPlacer.isPlacingBuilding == false) return true;
        else return false;
    }
    #endregion
    void Awake()
    {
        unitManager = GetComponent<UnitManager>();
    }
    
    private void Update()
    {
        if (marqueedObjects.Count > 0) haveMarqueedObjects = true;
        else haveMarqueedObjects = false;

        if (dragMarqueeEnabled()) //and not pressing on a ui element?
        {
            if (Inputs.LMBDown())
            {
                dragStartPosition = Input.mousePosition;
            }

            if (Inputs.LMBHold())
            {
                isDrawingMarquee = true;
            }

            if (Inputs.LMBUp() && isDrawingMarquee == true)
            {
                isDrawingMarquee = false;
                if (haveMarqueedObjects) 
                    SelectInMarquee();
            }

            if (isDrawingMarquee && dragStartPosition != Input.mousePosition)
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
            bool canMarqueeSelection = !selection.GetComponent<BuildingManager>(); //way to do this through inheritance?

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
            unitManager = selection.GetComponent<UnitManager>();
            unitManager.MarqueeSelect(selection);
        }
        marqueedObjects.Clear();
    }
    #endregion

    void OnGUI()
    {
        if (isDrawingMarquee && dragMarqueeEnabled())
        {
            // Create a rect from both mouse positions
            var rect = Utilities.GetScreenRect(dragStartPosition, Input.mousePosition);
            Utilities.DrawScreenRect(rect, new Color(0.5f, 1f, 0.4f, 0.2f));
            Utilities.DrawScreenRectBorder(rect, 1, new Color(0.5f, 1f, 0.4f));
        }
    }
}


