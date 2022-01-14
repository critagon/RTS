using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using RTS2.Selection;

public class Movement : MonoBehaviour
{
    public NavMeshAgent navAgent;

    public void GoToDestination(Vector3 destination)
    {
        navAgent.SetDestination(destination);
    }


    public void MoveUnit(Dictionary<int, GameObject> selectedTable, bool playerSelected)
    {

        if (playerSelected == true) //unit movement to mouse position on RMB
        {

            Debug.Log(navAgent);
            foreach (KeyValuePair<int, GameObject> pair in selectedTable)
            {
                Debug.Log(selectedTable);
            }
            
      
            
            /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHitInfo;
            // check if hit
            if (Physics.Raycast(ray, out raycastHitInfo))
            {
                foreach (KeyValuePair<int, GameObject> pair in selectedTable)
                {
                    navAgent = GetComponent<NavMeshAgent>();
                    Debug.Log(selectedTable[pair.Key]);
                    GoToDestination(raycastHitInfo.point);
                    
                }*/ 

            
        }

    }
}






/*private Vector3 GetPointUnderCursor()
    {
        Vector2 screenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        RaycastHit raycastHitInfo;

        Physics.Raycast(mouseWorldPosition, mainCamera.transform.forward, out raycastHitInfo, Mathf.Infinity, groundLayer);

        return raycastHitInfo.point;
    }



    void Awake()
    {
        mainCamera = Camera.main;
        selectionDictionary = GetComponent<SelectionDictionary>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            selectAgent.SetDestination(GetPointUnderCursor());
        }
    }



    public void MoveUnit(GameObject selection)
    {

    }


}*/
