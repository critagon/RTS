using System.Collections.Generic;
using UnityEngine;

public class SelectionFunctions : MonoBehaviour
{
   
}

/*using System.Collections.Generic;
using UnityEngine;

public class SelectionFunctions : MonoBehaviour
{
    Dictionary<int, GameObject> selectedUnits = Globals.SELECTED_UNITS;
    protected virtual Unit Unit { get; set; }

    #region Selection
    public void Select1(GameObject selection)
    {
        DeselectAll();
        int id = selection.GetInstanceID();
        if (!selectedUnits.ContainsKey(id))
        {
            Globals.SELECTED_UNITS.Add(id, selection);
        }

        EventManager.TriggerTypedEvent("SelectUnit", new CustomEventData(Unit));
    }

    public void ShiftSelect(GameObject selection)
    {
        int id = selection.GetInstanceID();

        if (!selectedUnits.ContainsKey(id))
        {
            Globals.SELECTED_UNITS.Add(id, selection);
        }

        EventManager.TriggerTypedEvent("SelectUnit", new CustomEventData(Unit));
    }

    public void MarqueeSelect(GameObject selection)
    {
        int id = selection.GetInstanceID();

        if ((!selectedUnits.ContainsKey(id)))
        {
            Globals.SELECTED_UNITS.Add(id, selection);
        }

        EventManager.TriggerTypedEvent("SelectUnit", new CustomEventData(Unit));
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

        EventManager.TriggerTypedEvent("DeselectUnit", new CustomEventData(Unit));
    }

    public void DeselectAll()
    {
        selectedUnits.Clear();
        EventManager.TriggerTypedEvent("DeselectUnit", new CustomEventData(Unit));

        //playerSelected = false;
        //enemySelected = false;
    }
    #endregion
}*/

