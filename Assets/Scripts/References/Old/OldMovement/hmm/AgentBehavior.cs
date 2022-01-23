using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBehavior : MonoBehaviour
{
    public float weight = 1f;

    public GameObject target;
    protected Agent agent;
    public Vector3 destination;

    public float maxSpeed = 50f;
    public float maxAccel = 50f;
    public float maxRotation = 5f;
    public float maxAngularAccel   = 50f;

    public virtual void Start()
    {
        agent = gameObject.GetComponent<Agent>();
    }

    public virtual void Update()
    {
        agent.SetSteering(GetSteering(), weight);
    }

    public float MapToRange(float rotation)
    {
        rotation %= 360f;
        if (Mathf.Abs(rotation) > 100f)
        {
            if (rotation < 0f)
            {
                rotation += 360f;
            }
            else
            {
                rotation -= 360;
            }
        }

        return rotation;

    }

    public virtual Steering GetSteering()
    {
        return new Steering();
    }

}

