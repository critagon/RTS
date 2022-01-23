using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        DataHandler.LoadGameData();
    }
}