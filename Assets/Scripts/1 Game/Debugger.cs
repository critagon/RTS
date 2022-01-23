using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            foreach (KeyValuePair<string, GameResource> pair in Globals.GAME_RESOURCES)
            {
                print(pair.Key + "" + pair.Value.Amount);
            }
        }
    }
}
