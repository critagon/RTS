using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Transform parent;

    void Update()
    {
        if (parent == null)
        {
            parent = GameObject.Find("Characters").transform; 
            transform.SetParent(parent);
        }
    }
}
