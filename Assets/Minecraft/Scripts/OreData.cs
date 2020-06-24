using UnityEngine;

[CreateAssetMenu(fileName = "OreData", menuName = "ScriptableObject/OreData", order = 0)]
public class OreData : ScriptableObject
{
    public string oreName;
    public OreType oreType;
    public GameObject orePrefab;
    public int oreUpperDepth;
    public int oreLowerDepth;
    /// <summary>
    /// total percentage out of all ground blocks (stone, rock, ore etc) in the world map
    /// </summary>
    public float totalPercentage;
    /// <summary>
    /// percentage of the ore which can be outside the depth range of oreUpperDepth and oreLowerDepth
    /// </summary>
    public float outZonePercentage;
}
