using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering
{
    public float angular; //rotation 0-360
    public Vector3 linear; //instantaneous velocity

    public Steering()
    {
        angular = 0f;
        linear = new Vector3();
    }
}