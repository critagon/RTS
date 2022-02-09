using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    protected Transform _transform;
    protected string uid;

    protected UnitData _unitData;
    protected int _currentHealth;

    protected int upgradeLevel;
    protected List<ResourceValue> _production;

    public Unit(UnitData unitData) : this(unitData, new List<ResourceValue>() { }) { } 

    public Unit(UnitData unitData, List<ResourceValue> production)
    {
        _unitData = unitData;
        _currentHealth = unitData.HP;

        GameObject getGameObject = GameObject.Instantiate(unitData.prefab) as GameObject;
        _transform = getGameObject.transform;
        _transform.GetComponent<UnitManager>();

        uid = System.Guid.NewGuid().ToString();
        upgradeLevel = 1;
        _production = production;
    }

    public void Upgrade()
    {
        upgradeLevel += 1;
    }

    public void ProduceResources()
    {
        foreach (ResourceValue resource in _production)
            Globals.GAME_RESOURCES[resource.code].AddAmount(resource.amount);
    }

    public void SetPosition(Vector3 position)
    {
        _transform.position = position;
    }

    public virtual void PlaceCost()
    {
        _transform.GetComponent<BoxCollider>().isTrigger = false; //remove "is trigger" flag from box collider to allow for collisions with units

        foreach (ResourceValue resource in Cost) // update game resources: remove the cost of the building from each game resource
        {
            Globals.GAME_RESOURCES[resource.code].AddAmount(-resource.amount);
        }
    }

    public bool CanBuy()
    {
        return _unitData.CanBuy();
    }

    /***********************************************************************************************************************/
    public UnitData unitData { get => _unitData; }
    public string Uid { get => uid; }
    public string Code { get => _unitData.code; }

    public Transform Transform { get => _transform; }

    public List<ResourceValue> Cost { get => _unitData.cost; }
    public List<ResourceValue> Production { get => _production; }

    public int HP { get => _currentHealth; set => _currentHealth = value; }
    public int MaxHP { get => _unitData.maxHP; }

    public int Level { get => upgradeLevel; }
}

