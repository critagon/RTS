using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float trueMaxSpeed; //Max speed of slowest unit 
    public float maxAccel;

    public float orientation;
    public float rotation;
    public Vector3 velocity;
    protected Steering steering;

    public float maxRotation = 45f;
    public float maxAngularAccel = 45f;

    void Start()
    {
        velocity = Vector3.zero;
        steering = new Steering();
        trueMaxSpeed = maxSpeed;
    }


    public void SetSteering(Steering steering, float weight)
    {
        this.steering.linear += (weight * steering.linear);
        this.steering.angular += (weight * steering.angular);
    }

    
    //change the transform based on the last frame's steering
    public virtual void Update()
    {
        Vector3 displacement = velocity * Time.deltaTime;
        displacement.y = 0;

        orientation += rotation * Time.deltaTime;

        //limit orientation betweeen 0 and 360
        if (orientation < 0f)
        {
            orientation += 360f;
        }
        else if (orientation > 360f)
        {
            orientation -= 360f;
        }

        transform.Translate(displacement, Space.World);
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.up, orientation);
    }

    public virtual void LateUpdate()
    {
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;
        if (velocity.magnitude < maxSpeed)
        {
            velocity.Normalize();
            velocity = velocity * maxSpeed;
        }
        if (steering.linear.magnitude == 0.0f)
        {
            velocity = Vector3.zero;
        }

        steering = new Steering();
    }

    //used for speed matching when travelling in groups
    public void SpeedReset()
    {

    }
    

    //update movement for the next frame

}
