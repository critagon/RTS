using System.Collections.Generic;
using UnityEngine;

public class SelectionFunctions : MonoBehaviour
{
    Dictionary<int, GameObject> selectedUnits = Globals.SELECTED_UNITS;

    #region Selection
    public void Select1(GameObject selection)
    {
        DeselectAll();
        int id = selection.GetInstanceID();
        if (!selectedUnits.ContainsKey(id))
        {
            Globals.SELECTED_UNITS.Add(id, selection);
        }
    }
    
    public void ShiftSelect(GameObject selection)
    {
        int id = selection.GetInstanceID();

        if (!selectedUnits.ContainsKey(id))
        {
            Globals.SELECTED_UNITS.Add(id, selection); 
        }
    }

    public void MarqueeSelect(GameObject selection)
    {
        int id = selection.GetInstanceID();

        if ((!selectedUnits.ContainsKey(id)))
        {
            Globals.SELECTED_UNITS.Add(id, selection);
        }
    }
    #endregion

    #region Deselection
    public void Deselect1(GameObject selection)
    {
        int id = selection.GetInstanceID();
        if (selectedUnits.ContainsKey(id))
        {
            selectedUnits.Remove(id);
        }
    }

    public void DeselectAll()
    {
        selectedUnits.Clear();
        //playerSelected = false;
        //enemySelected = false;
    }
    #endregion
}


