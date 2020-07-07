using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldAllOreData", menuName = "ScriptableObject/WorldAllOreData", order = 1)]
public class WorldAllOreData : ScriptableObject
{
    public List<OreData> allOresData;

    public int OreFromDepthCalc(int yDepth, out float inZoneProbabilityLayer)
    {
        //Debug.Log("yDepth" + yDepth);
        for (int i = 0; i < allOresData.Count; i++)
        {
            if(-allOresData[i].oreUpperDepth >= yDepth && yDepth >= -allOresData[i].oreLowerDepth)
            {
                inZoneProbabilityLayer = allOresData[i].inZonePercentageLayer;
                return i;
            }
        }
        //for normal stone
        inZoneProbabilityLayer = 100;
        return 0;
    }
}
