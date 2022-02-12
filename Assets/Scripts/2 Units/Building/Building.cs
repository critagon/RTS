using System.Collections.Generic;
using UnityEngine;

public class Building : Unit
{
    #region Universal Variables
    BuildingData _buildingData;
    BuildingPlacement _placement;
    List<Material> _materials;

    BuildingManager buildingManager;
    #endregion

    public enum BuildingPlacement
    {
        VALID,
        INVALID,
        FIXED,
        CONSTRUCTING,
    };

    /*public Building(BuildingData buildingData) : this(buildingData, new List<ResourceValue>() { }) {

    } //no production */

    /*public Building(BuildingData buildingData, List<ResourceValue> production) :
        base(buildingData, production)
    {
        _buildingData = buildingData;
        buildingManager = _transform.GetComponent<BuildingManager>();

        _currentHealth = buildingData.HP;

        _materials = new List<Material>();
        foreach (Material material in _transform.Find("Mesh").GetComponent<Renderer>().materials)
        {
            _materials.Add(new Material(material));
        }
        SetMaterials();
    }*/


    public Building(BuildingData buildingData) : base(buildingData)
    {
        _buildingData = buildingData;
        buildingManager = _transform.GetComponent<BuildingManager>();

        _currentHealth = buildingData.HP;

        _materials = new List<Material>();
        foreach (Material material in _transform.Find("Mesh").GetComponent<Renderer>().materials)
        {
            _materials.Add(new Material(material));
        }
        SetMaterials();
    }

    #region SetMaterials
    public void SetMaterials() { SetMaterials(_placement); }

    public void SetMaterials(BuildingPlacement placement)
    {
        List<Material> materials;
        if (placement == BuildingPlacement.VALID)
        {
            Material refMaterial = Resources.Load("Assets/Materials/ValidPlacement") as Material;
            materials = new List<Material>();
            for (int i = 0; i < _materials.Count; i++)
            {
                materials.Add(refMaterial);
            }
        }
        else if (placement == BuildingPlacement.INVALID)
        {
            Material refMaterial = Resources.Load("Assets/Materials/InvalidPlacement") as Material;
            materials = new List<Material>();
            for (int i = 0; i < _materials.Count; i++)
            {
                materials.Add(refMaterial);
            }
        }
        else if (placement == BuildingPlacement.FIXED)
        {
            materials = _materials;
        }
        else
        {
            return;
        }
        _transform.Find("Mesh").GetComponent<Renderer>().materials = materials.ToArray();
    }
    #endregion

    public override void Place()
    {
        base.Place();
        _placement = BuildingPlacement.FIXED;
        SetMaterials();
    }

    #region CheckValidPlacement
    public void CheckValidPlacement()
    {
        if (_placement == BuildingPlacement.FIXED)
        {
            return;
        }

        _placement = buildingManager.CheckPlacement()
         ? BuildingPlacement.VALID : BuildingPlacement.INVALID;
    }
    #endregion

    #region Get
    /*=======================================================================*/
    public bool HasValidPlacement { get => _placement == BuildingPlacement.VALID; }
    public bool isInvalid { get => _placement == BuildingPlacement.INVALID; }
    public bool isValid { get => _placement == BuildingPlacement.VALID; }
    public bool isFixed { get => _placement == BuildingPlacement.FIXED; }
    public bool isConstructing { get => _placement == BuildingPlacement.CONSTRUCTING; }
    #endregion

    public int DataIndex
    {
        get
        {
            for (int i = 0; i < Globals.BUILDING_DATA.Length; i++)
            {
                if (Globals.BUILDING_DATA[i].code == _buildingData.code)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}





