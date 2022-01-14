using UnityEngine;

public class BuildingManager : UnitManager
#region Universal Variables and Setup
{
    protected override Unit Unit
    {
        get { return _building; }
        set { _building = value is Building ? (Building)value : null; }
    }

    Building _building;

    int _nCollisions = 0;
    bool validPlacement;

    public bool _isActive;
    public string whatType = "";

    public Transform parent;

    BuildingData buildingData;
    UnitManager unitManager;

    private void Awake()
    {
        if (_building == null)
        {
            _isActive = true;
            whatType = "Pre-placed";
            //how to initialize pre-placed buildings?
        }

        if (parent == null)
        {
            parent = GameObject.Find("Buildings").transform; 
            transform.SetParent(parent);
        }
    }

    public void Initialize(Building building)
    {
        boxCollider = GetComponent<BoxCollider>();
        _building = building;
        _building.Transform.SetParent(parent.transform);
    }
    #endregion

    #region Placement Check

        #region Collision Check
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Terrain") return;
            _nCollisions++;
            CheckPlacement();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Terrain") return;
            _nCollisions--;
            CheckPlacement();
        }
        #endregion

    public bool CheckPlacement()
    {
        if (_building == null) return false;
        if (_building.isFixed) return false;

        validPlacement = HasValidPlacement();

        if (!validPlacement) _building.SetMaterials(Building.BuildingPlacement.INVALID);
        else _building.SetMaterials(Building.BuildingPlacement.VALID);

        return validPlacement;
    }

    public bool HasValidPlacement()
    {
        if (_nCollisions > 0) return false;

        // get 4 bottom corner positions
        Vector3 transformPosition = transform.position;
        Vector3 boxColliderCenter = boxCollider.center;
        Vector3 boxColliderSize = boxCollider.size / 2f;
        float bottomHeight = boxColliderCenter.y - boxColliderSize.y + 0.5f;
        Vector3[] bottomCorners = new Vector3[]
        {
        new Vector3(boxColliderCenter.x - boxColliderSize.x, bottomHeight, boxColliderCenter.z - boxColliderSize.z),
        new Vector3(boxColliderCenter.x - boxColliderSize.x, bottomHeight, boxColliderCenter.z + boxColliderSize.z),
        new Vector3(boxColliderCenter.x + boxColliderSize.x, bottomHeight, boxColliderCenter.z - boxColliderSize.z),
        new Vector3(boxColliderCenter.x + boxColliderSize.x, bottomHeight, boxColliderCenter.z + boxColliderSize.z)
        };
        // cast a small ray beneath the corner to check for a close ground
        // (if at least two are not valid, then placement is invalid)
        int invalidCornersCount = 0;
        foreach (Vector3 corner in bottomCorners)
        {
            if (!Physics.Raycast(transformPosition + corner, Vector3.up * -1f, 2f, Globals.GROUND_LAYER_MASK))
                invalidCornersCount++;
        }
        return invalidCornersCount < 1;
    }
    #endregion

    #region IsActive?
    protected override bool IsActive()
    {
        if (_building != null && _building.isFixed)
        {
            return true;
        }
        else return false;
    }

    private void Update()
    {

        if (parent != GameObject.Find("Buildings").transform)
        {
            parent = GameObject.Find("Buildings").transform; //set in inspector later?
            transform.SetParent(parent);
        }

        if (_building != null)
        {
            _isActive = IsActive();

            if (_building.isFixed && whatType != "Fixed")
            {
                whatType = "Fixed";
            }
            if (_building.isValid && whatType != "Valid")
            {
                whatType = "Valid";
            }
            if (_building.isInvalid && whatType != "Invalid")
            {
                whatType = "Invalid";
            }
            if (!(_building.isFixed || _building.isValid || _building.isInvalid))
            {
                whatType = "?";
            }
        }
    }
    #endregion
}






