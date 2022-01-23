using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]

public class UnitManager : MonoBehaviour
{
    #region Setup
    #region Variables
    protected virtual Unit Unit { get; set; } //? why get set?

    Dictionary<int, GameObject> selectedUnits = Globals.SELECTED_UNITS;
    public int id;

    GameObject hoverObject;

    public virtual bool IsABuilding() //return false than override in building manager? how does that work? doesn't work :(
    {
        if (GetComponent<BuildingManager>()) return true;
        else return false;
    }

    protected virtual bool IsActive() //protected? virtual? what is a "variable method" called?
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
        if (IsActive() == false) return false; //require IsABuilding too?
        else return true;
    }

    public bool isPlacingBuilding;
   
    Transform healthbarCanvas; //should I do transform or gameobject?
    GameObject _healthbar;

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
        id = gameObject.GetInstanceID();
    }

    public void Initialize(Unit unit)
    {
        boxCollider = GetComponent<BoxCollider>(); //why is this necessary?
        Unit = unit; //what is this for?
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

    bool isHoveringOver;

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

    void UpdateUnitDisplay()
    {
        //HoverCircle
        if (isHoveringOver && IsDrawingMarquee() == false && isPlacingBuilding == false && IsABuilding() == false)
        {
            transform.Find("HoverCircle").gameObject.SetActive(true);
        }

        if (!isHoveringOver)
        {
            transform.Find("HoverCircle").gameObject.SetActive(false);
        }

        //SelectionCircle
        if (selectedUnits.ContainsKey(id))
        {
            transform.Find("SelectionCircle").gameObject.SetActive(true);
        }

        if (!selectedUnits.ContainsKey(id))
        {
            transform.Find("SelectionCircle").gameObject.SetActive(false);
        }

        //Healthbar
        if (selectedUnits.ContainsKey(id) || isHoveringOver && !IsABuilding())
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
        if (!selectedUnits.ContainsKey(id) && !isHoveringOver)
        {
            if (_healthbar != null)
            {
                Destroy(_healthbar);
                _healthbar = null;
            }
        }
    }
    #endregion

    #region Selection Functions
    public void Select1(GameObject selection)
    {
        DeselectAll();
        if (!selectedUnits.ContainsKey(id))
        {
            Globals.SELECTED_UNITS.Add(id, selection);
        }

        EventManager.TriggerTypedEvent("SelectUnit", new CustomEventData(Unit));
    }

    public void ShiftSelect(GameObject selection)
    {
        if (!selectedUnits.ContainsKey(id))
        {
            Globals.SELECTED_UNITS.Add(id, selection);
        }

        EventManager.TriggerTypedEvent("SelectUnit", new CustomEventData(Unit));
    }

    public void MarqueeSelect(GameObject selection)
    {
        if ((!selectedUnits.ContainsKey(id)))
        {
            Globals.SELECTED_UNITS.Add(id, selection);
        }

        EventManager.TriggerTypedEvent("SelectUnit", new CustomEventData(Unit));
    }

    public void Deselect1(GameObject selection)
    {
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
    }
    #endregion
}