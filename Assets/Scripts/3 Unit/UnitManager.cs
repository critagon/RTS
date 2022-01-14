using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]

public class UnitManager : MonoBehaviour
{
    #region Universal Variables and Setup
    Dictionary<int, GameObject> selectedUnits = Globals.SELECTED_UNITS;

    int id;

    Transform healthbarCanvas;
    GameObject _healthbar;

    protected BoxCollider boxCollider;
    protected virtual Unit Unit { get; set; }

    protected virtual bool IsActive()
    {
        return true;
    }

    bool isDraggingMouseBox;

    private void Awake()
    {
        id = gameObject.GetInstanceID();
        healthbarCanvas = GameObject.Find("Healthbars").transform;
    }
    #endregion

    public void Initialize(Unit unit)
    {
        boxCollider = GetComponent<BoxCollider>();
        Unit = unit;
    }

    void Update()
    {
        isDraggingMouseBox = Inputs.LMBHold(); //get marqueeSelection component instead?

        #region Selection UI Display Update
        if (selectedUnits.ContainsKey(id))
        {
            transform.Find("SelectionCircle").gameObject.SetActive(true);

            if (_healthbar == null)
            {
                _healthbar = GameObject.Instantiate(Resources.Load("Prefabs/UI/Healthbar")) as GameObject;
                _healthbar.transform.SetParent(healthbarCanvas);

                Healthbar healthbar = _healthbar.GetComponent<Healthbar>();
                Rect boundingBox = Utilities.GetBoundingBoxOnScreen(transform.Find("Mesh").GetComponent<Renderer>().bounds, Camera.main);
                healthbar.Initialize(transform, boundingBox.height);
                healthbar.SetPosition();

                healthbar.name = ("Healthbar " + id);
            }
        }

        if (!selectedUnits.ContainsKey(id) && _healthbar != null)
        {
            Destroy(_healthbar);
            _healthbar = null;
        }

        if (!selectedUnits.ContainsKey(id))
        {
            transform.Find("SelectionCircle").gameObject.SetActive(false);
        }
    }
    #endregion

    #region OnMouse
    void OnMouseEnter()
    {
        if (isDraggingMouseBox == false && !GetComponent<BuildingManager>())
            transform.Find("HoverCircle").gameObject.SetActive(true);
    }

    void OnMouseExit()
    {
        transform.Find("HoverCircle").gameObject.SetActive(false);
    }
    #endregion
}


