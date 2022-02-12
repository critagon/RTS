using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]

public class UnitManager : MonoBehaviour
{
    #region Setup
    #region Variables
    public virtual Unit Unit { get; set; }
    UnitData unitData { get; set; } //do i need to do the properties with the UnitData? Also, do I need UnitData here?

    protected Dictionary<int, GameObject> selectedUnits = Globals.SELECTED_UNITS;
    public int id;

    GameObject hoverObject;

    protected virtual bool IsActive()
    {
        return true;
    }

    protected bool IsDrawingMarquee()
    {
        if (MarqueeSelection.isDrawingMarquee == true) return true;
        else return false;
    }

    protected virtual bool CanBeSelected()
    {
        if (IsActive() == false) return false;
        else return true;
    }

    public bool isPlacingBuilding;

    protected Transform healthbarCanvas;
    protected GameObject _healthbar;

    protected BoxCollider boxCollider;
    #endregion

    #region Initialize

    /*void OnEnable()
    {
    }

    void OnDisable()
    {
    }*/

    private void Awake()
    {
        print("Unit in UM.Awake is: " + Unit);
        id = gameObject.GetInstanceID();
        /*if (Unit == null)
        {
            Initialize(Unit);
        }*/
    }

    protected virtual void Initialize(Unit unit)
    {
        boxCollider = GetComponent<BoxCollider>(); //why is this necessary?
        Unit = new Unit(unitData); 
    }

    #endregion
    #endregion

    #region Update
    void Update()
    {
        isPlacingBuilding = BuildingPlacer.isPlacingBuilding;
        Selecting();
        UpdateUnitDisplay();
    }

    protected bool isHoveringOver;

    void OnMouseEnter()
    {
        isHoveringOver = true;
        hoverObject = transform.gameObject;
    }

    void OnMouseExit()
    {
        isHoveringOver = false;
    }

    void Selecting()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            if (selectedUnits.ContainsKey(id))
            {
                foreach (KeyValuePair<int, GameObject> pair in selectedUnits)
                {
                    print(pair);
                }
            }

            if (selectedUnits.Count == 0)
            {
                print("No units selected.");
            }
        }

        if ((Inputs.LMBDown() && isHoveringOver && CanBeSelected() && isPlacingBuilding == false))
        {
            if (!(Inputs.ShiftHold() || (Inputs.CtrlHold()))) //select 1
            {
                Select1(hoverObject);
            }

            if (Inputs.ShiftHold() || (Inputs.CtrlHold()))
            {
                if (!selectedUnits.ContainsKey(id))//shift select
                    ShiftSelect(hoverObject);
                else //deselect 1
                    Deselect1(hoverObject);
            }
        }

        if ((Globals.SELECTED_UNITS.Count > 0) && !(Inputs.ShiftHold()) || (Inputs.CtrlHold())) //deselect all
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                DeselectAll();

            if (Inputs.LMBDown()) //deselct all if ray hits terrain
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

    #region Unit Display
    protected virtual void UpdateUnitDisplay()
    {
        if (selectedUnits.ContainsKey(id))
        {
            transform.Find("SelectionCircle").gameObject.SetActive(true);
            HealthbarCreate();
        }

        if (!selectedUnits.ContainsKey(id))
        {
            transform.Find("SelectionCircle").gameObject.SetActive(false);
            HealthbarDestroy();
        }
    }
    protected void HealthbarCreate()
    {
        if (_healthbar == null)
        {
            _healthbar = GameObject.Instantiate(Resources.Load("Prefabs & Scriptable Objects/UI/Healthbar")) as GameObject;
            healthbarCanvas = GameObject.Find("Healthbars").transform;
            _healthbar.transform.SetParent(healthbarCanvas);

            Healthbar healthbar = _healthbar.GetComponent<Healthbar>();
            Rect boundingBox = Utilities.GetBoundingBoxOnScreen(transform.Find("Mesh").GetComponent<Renderer>().bounds, Camera.main);
            healthbar.Initialize(transform, boundingBox.height);
            healthbar.SetPosition();

            healthbar.name = "Healthbar " + id;
        }
    }

    protected void HealthbarDestroy()
    {
        if (_healthbar != null)
        {
            Destroy(_healthbar);
            _healthbar = null;
        }
    }
    #endregion
    #endregion

    #region Selection Functions
    public void Select1(GameObject selection)
    {
        DeselectAll();
        if (!selectedUnits.ContainsKey(id))
        {
            Globals.SELECTED_UNITS.Add(id, selection);
            SelectEvent();
        }
    }

    public void ShiftSelect(GameObject selection)
    {
        if (!selectedUnits.ContainsKey(id))
        {
            Globals.SELECTED_UNITS.Add(id, selection);
            SelectEvent();
        }
    }

    public virtual void MarqueeSelect(GameObject selection)
    {
        if ((!selectedUnits.ContainsKey(id)))
        {
            Globals.SELECTED_UNITS.Add(id, selection);
            SelectEvent();
        }
    }

    public void Deselect1(GameObject selection)
    {
        if (selectedUnits.ContainsKey(id))
        {
            selectedUnits.Remove(id);
            DeselectEvent();
        }
    }

    public void DeselectAll()
    {
        if (selectedUnits.Count > 0)
        {
            selectedUnits.Clear();
            DeselectEvent();
        }
    }

    public void SelectEvent()
    {
        print("Unit in UnitManager.SelectEvent is: " + Unit); //to debug wheter there is an instance of unit
        if (Unit != null)
        {
            EventManager.TriggerTypedEvent("OnSelectUnit", new CustomEventData(Unit));
        }
    }

    public void DeselectEvent()
    {
        print("Unit in UnitManager.DeselectEvent is: " + Unit); //to debug wheter there is an instance of unit
        if (Unit != null)
        {
            EventManager.TriggerTypedEvent("OnDeselectUnit", new CustomEventData(Unit));
        }
    }
    #endregion
}


