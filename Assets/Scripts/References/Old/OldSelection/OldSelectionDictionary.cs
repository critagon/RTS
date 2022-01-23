using System.Collections.Generic;
using UnityEngine;

//namespace RTS2.Selection {

public class OldSelectionDictionary : MonoBehaviour
{
    #region Universal Variables 

    //Movement movement;

    public Dictionary<int, GameObject> selectedUnits = Globals.SELECTED_UNITS;

    //public bool DisablemultiSelectEnemy;
    bool playerSelected;
    bool enemySelected;

    LayerMask playerUnitLayer = 9;
    LayerMask enemyUnitLayer = 10;
    LayerMask playerBuildingLayer = 11;
    LayerMask enemyBuildingLayer = 12;

    #endregion

    private void Awake()
    {
        //movement = GetComponent<Movement>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debugg();
        }
        if (Input.GetMouseButtonUp(1))
        {
            RightClick();
        }
    }
    public void Debugg()
    {
        bool somethingSelected = false;

        foreach (KeyValuePair<int, GameObject> pair in selectedUnits)
        {
            if (pair.Value != null)
            {
                Debug.Log(selectedUnits[pair.Key]);
                somethingSelected = true;
            }
        }

        if (somethingSelected == false)
        {
            print("Nothing selected.");
        }
    }

    #region Selection

    public void AddSelected(GameObject selection)
    {

        var PlayerTag = selection.tag;
        PlayerTag = "Unit";
        
        int id = selection.GetInstanceID();
        int selectionLayer = selection.layer;

        #region Shift Select
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftControl))
        {
            if (selectionLayer == playerUnitLayer || selectionLayer == playerBuildingLayer)
            {
                if (enemySelected == true)
                {
                    DeselectAll();
                }
                if (selection.transform.Find("SelectHighlight").gameObject.activeSelf)
                {
                    Deselect1(selection, id);
                }
                else if (!selectedUnits.ContainsKey(id))
                {
                    selectedUnits.Add(id, selection);
                    selection.transform.Find("SelectHighlight").gameObject.SetActive(true);
                    playerSelected = true;
                }
            }
            if (selectionLayer == enemyUnitLayer || selectionLayer == enemyBuildingLayer)
            {
                if (playerSelected == true)
                {
                    DeselectAll();
                }
                if (selection.transform.Find("SelectHighlight").gameObject.activeSelf)
                {
                    Deselect1(selection, id);
                }
                else if (!selectedUnits.ContainsKey(id))
                {
                    selectedUnits.Add(id, selection);
                    selection.transform.Find("SelectHighlight").gameObject.SetActive(true);
                    enemySelected = true;
                }
            }
        }
        #endregion

        #region Single Select

        else if ((selectionLayer == playerUnitLayer || selectionLayer == playerBuildingLayer)
            && !selectedUnits.ContainsKey(id))
        {
            selectedUnits.Add(id, selection);
            selection.transform.Find("SelectHighlight").gameObject.SetActive(true);
            playerSelected = true;
        }

        else if ((selectionLayer == enemyUnitLayer || selectionLayer == enemyBuildingLayer)
            && !selectedUnits.ContainsKey(id))
        {
            DeselectAll();
            selectedUnits.Add(id, selection);
            selection.transform.Find("SelectHighlight").gameObject.SetActive(true);
            enemySelected = true;
        }
        #endregion
    }
    #endregion

    #region Drag Selection
    public void AddDragSelected(GameObject selection)
    {

        int id;
        id = selection.GetInstanceID();
        int selectionLayer = selection.layer;

        if (selectionLayer == playerUnitLayer)
        {
            if ((enemySelected == true)
                && (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl)))
            {
                DeselectAll();
                enemySelected = false;
            }

            if (enemySelected == false
                && !selectedUnits.ContainsKey(id))
            {
                selectedUnits.Add(id, selection);
                selection.transform.Find("SelectHighlight").gameObject.SetActive(true);
                playerSelected = true;
            }
        }

        if (selectionLayer == playerBuildingLayer
            && playerSelected == false
            && !selectedUnits.ContainsKey(id))
        {
            selectedUnits.Add(id, selection);
            selection.transform.Find("SelectHighlight").gameObject.SetActive(true);
        }


        if (selectionLayer == enemyUnitLayer
            && playerSelected == false
            && !selectedUnits.ContainsKey(id))
        {
            selectedUnits.Add(id, selection);
            selection.transform.Find("SelectHighlight").gameObject.SetActive(true);
            enemySelected = true;
        }

        if (selectionLayer == enemyBuildingLayer
           && playerSelected == false
           && !selectedUnits.ContainsKey(id))
        {
            selectedUnits.Add(id, selection);
            selection.transform.Find("SelectHighlight").gameObject.SetActive(true);
            enemySelected = true;
        }
    }
    #endregion

    #region Deselection
    public void DeselectAll()
    {
        foreach (KeyValuePair<int, GameObject> pair in selectedUnits)
        {
            if (pair.Value != null)
            {
                selectedUnits[pair.Key].transform.Find("SelectHighlight").gameObject.SetActive(false);
            }
        }
        selectedUnits.Clear();
        playerSelected = false;
        enemySelected = false;
    }

    public void Deselect1(GameObject selection, int id)
    {
        selection.transform.Find("SelectHighlight").gameObject.SetActive(false);
        selectedUnits.Remove(id);
    }
    #endregion

    public void RightClick()
    {
        //movement.MoveUnit(selectedTable, playerSelected);
    }

}

