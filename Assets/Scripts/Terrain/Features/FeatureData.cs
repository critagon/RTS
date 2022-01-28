using UnityEngine;

[CreateAssetMenu(fileName = "Terrain Feature", menuName = "ScriptableObjects/Terrain Feature", order = 1)]
public class FeatureData : ScriptableObject
{
    public GameObject prefab;
    public string code;
    public string featureName;
    public string description;
    public int HP;
    public int maxHP;
}


