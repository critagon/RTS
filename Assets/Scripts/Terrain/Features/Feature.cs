using System.Collections.Generic;
using UnityEngine;

public class Feature
{
    protected Transform _transform;
    protected string uid;

    protected FeatureData _featureData;
    protected int _currentHealth;

    public Feature(FeatureData featureData)
    {
        _featureData = featureData;
        _currentHealth = featureData.HP;

        GameObject getGameObject = GameObject.Instantiate(featureData.prefab) as GameObject;
        _transform = getGameObject.transform;

        uid = System.Guid.NewGuid().ToString();
    }

    public void SetPosition(Vector3 position)
    {
        _transform.position = position;
    }

    public virtual void Place()
    {
        _transform.GetComponent<BoxCollider>().isTrigger = false; //remove "is trigger" flag from box collider to allow for collisions with units

        /*foreach (ResourceValue resource in Cost) // update game resources: remove the cost of the building from each game resource
        {
            Globals.GAME_RESOURCES[resource.code].AddAmount(-resource.amount);
        }*/
    }

    /***********************************************************************************************************************/
    public FeatureData featureData { get => _featureData; }
    public string Uid { get => uid; }
    public string Code { get => _featureData.code; }

    public Transform Transform { get => _transform; }

    public int HP { get => _currentHealth; set => _currentHealth = value; }
    public int MaxHP { get => _featureData.maxHP; }
}

