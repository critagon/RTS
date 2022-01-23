using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldHoverOver : MonoBehaviour
{
    GameObject hoverOverObject;
    //Building placedBuilding = null;

    bool isHovering;
    bool hoveringDebug;

    void Update()
    {

        Camera mainCamera = Camera.main;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHitInfo;


        if (Input.GetKeyDown(KeyCode.H))
        {
            if (hoveringDebug == false)
            { 
                hoveringDebug = true;
            }
            else
            {
                hoveringDebug = false;
            }
        }
        
        if (isHovering == false)
        {
            if (Physics.Raycast(ray, out raycastHitInfo, Mathf.Infinity, 1 << 9) || (Physics.Raycast(ray, out raycastHitInfo, Mathf.Infinity, 1 << 10)))
            {
                hoverOverObject = raycastHitInfo.transform.gameObject;
                hoverOverObject.transform.Find("HoverHighlight").gameObject.SetActive(true);
                isHovering = true;
                if (hoveringDebug == true)
                { 
                    Debugg(isHovering, hoverOverObject);
                }
            }
        }

        if (isHovering == true)
        {
            if (!(Physics.Raycast(ray, out raycastHitInfo, Mathf.Infinity, 1 << 9) || (Physics.Raycast(ray, out raycastHitInfo, Mathf.Infinity, 1 << 10))))
            {
                hoverOverObject.transform.Find("HoverHighlight").gameObject.SetActive(false);
                isHovering = false;
            }
        }
    }

    void Debugg (bool isHovering, GameObject hoverOverObject)
    {
        bool stopHover = false;
        bool hasHovered = false;

        if (isHovering == true && stopHover == false)
        {
            Debug.Log("Hovering on " + hoverOverObject);
            stopHover = true;
            hasHovered = true;
        }
        if (isHovering == false && hasHovered == true)
        {
            print("No longer hovering.");
        }
    }
}

