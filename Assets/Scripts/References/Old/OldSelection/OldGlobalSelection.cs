using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using RTS2.Selection;

public class OldGlobalSelection : MonoBehaviour
{
    #region Universal Variables
    
    //IDictionary id_table; ??
    OldSelectionDictionary selectedTable;
    RaycastHit raycastHitInfo;

    bool dragSelected;

    LayerMask playerUnitLayer = 9;
    LayerMask enemyUnitLayer = 10;
    LayerMask playerBuildingLayer = 11;
    LayerMask enemyBuildingLayer = 12;

    //Collider variables

    MeshCollider selectionBox;
    Mesh selectionMesh;

    Vector3 p1;
    Vector3 p2;

    Vector2[] corners;    

    Vector3[] verts; //the vertices of meshcollider
    Vector3[] vecs;

    #endregion

    void Start()
    {
        //id_table = GetComponent<IDictionary>();
        selectedTable = GetComponent<OldSelectionDictionary>();
        print("Debug key for selection is 'K.' Debug key for hover is 'H.'");
        //print("To place building, press 'B.'");
        print("Debug key for building placement is 'Q.'"); 
    }

    void Update()
    {
        Camera mainCamera = Camera.main;

        #region ToSelectionDictionary
        //1. when left mouse button clicked (but not released)
        if (Input.GetMouseButtonDown(0))
        {
            p1 = Input.mousePosition;
        }

        //2. while left mouse button held
        if (Input.GetMouseButton(0))
        {
            if ((p1 - Input.mousePosition).magnitude > 40)
            {
                dragSelected = true;
            }
        }

        //3. when mouse button comes up
        if (Input.GetMouseButtonUp(0))
        {
            #region Non-Marquee Select

            if (dragSelected == false) //single select
            {
                Ray ray = mainCamera.ScreenPointToRay(p1);

                if (Physics.Raycast(ray, out raycastHitInfo, Mathf.Infinity))
                {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftControl)) //inclusive select
                    {
                        selectedTable.AddSelected(raycastHitInfo.transform.gameObject);
                    }

                    else //exclusive select
                    {
                        selectedTable.DeselectAll();
                        selectedTable.AddSelected(raycastHitInfo.transform.gameObject);
                    }
                }

                else //if we didnt hit something
                {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftControl))
                    {
                        //do nothing
                    }
                    else
                    {
                        selectedTable.DeselectAll();
                    }
                }
            }
            #endregion

            #region Marquee Select
            else
            {
                verts = new Vector3[4];
                vecs = new Vector3[4];
                int i = 0;
                p2 = Input.mousePosition;
                corners = getBoundingBox(p1, p2);

                foreach (Vector2 corner in corners)
                {
                    Ray ray = mainCamera.ScreenPointToRay(corner);

                    if (Physics.Raycast(ray, out raycastHitInfo, Mathf.Infinity, 1 << 8))
                    {
                        verts[i] = new Vector3(raycastHitInfo.point.x, raycastHitInfo.point.y, raycastHitInfo.point.z);
                        vecs[i] = ray.origin - raycastHitInfo.point;
                        //Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), raycastHitInfo.point, Color.red, 1.0f);
                    }

                    i++;
                }

                //generate the mesh
                selectionMesh = generateSelectionMesh(verts, vecs);

                selectionBox = gameObject.AddComponent<MeshCollider>();
                selectionBox.sharedMesh = selectionMesh;
                selectionBox.convex = true;
                selectionBox.isTrigger = true;

                if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
                {
                    selectedTable.DeselectAll();
                }

                Destroy(selectionBox, 0.02f);
            } //end marquee select

            dragSelected = false;
        }
        #endregion
        #endregion
    }

    #region Box Select UI ??
    private void OnGUI()
    {
        if (dragSelected == true)
        {
            var rect = Utilities.GetScreenRect(p1, Input.mousePosition);
            Utilities.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utilities.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    //create a bounding box (4 corners in order) from the start and end mouse position
    Vector2[] getBoundingBox(Vector2 p1, Vector2 p2)
    {
        Vector2 newP1;
        Vector2 newP2;
        Vector2 newP3;
        Vector2 newP4;

        if (p1.x < p2.x) //if p1 is to the left of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = p1;
                newP2 = new Vector2(p2.x, p1.y);
                newP3 = new Vector2(p1.x, p2.y);
                newP4 = p2;
            }
            else //if p1 is below p2
            {
                newP1 = new Vector2(p1.x, p2.y);
                newP2 = p2;
                newP3 = p1;
                newP4 = new Vector2(p2.x, p1.y);
            }
        }
        else //if p1 is to the right of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = new Vector2(p2.x, p1.y);
                newP2 = p1;
                newP3 = p2;
                newP4 = new Vector2(p1.x, p2.y);
            }
            else //if p1 is below p2
            {
                newP1 = p2;
                newP2 = new Vector2(p1.x, p2.y);
                newP3 = new Vector2(p2.x, p1.y);
                newP4 = p1;
            }
        }

        Vector2[] corners = { newP1, newP2, newP3, newP4 };
        return corners;
    }

    //generate a mesh from the 4 bottom points
    Mesh generateSelectionMesh(Vector3[] corners, Vector3[] vecs) //?
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; //map the tris of our cube

        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j - 4];
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }
    #endregion

    #region Marquee Trigger
    private void OnTriggerEnter(Collider trigger)
    {

        int selectionLayer = trigger.gameObject.layer;
        
        
        if (selectionLayer == playerUnitLayer || selectionLayer == playerBuildingLayer || selectionLayer == enemyUnitLayer || selectionLayer == enemyBuildingLayer)
        {
            dragSelected = true;
            selectedTable.AddDragSelected(trigger.gameObject);
            dragSelected = false;
        }
    }
    #endregion

}