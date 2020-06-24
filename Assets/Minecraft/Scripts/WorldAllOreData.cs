using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldAllOreData", menuName = "ScriptableObject/WorldAllOreData", order = 1)]
public class WorldAllOreData : ScriptableObject
{
    public List<OreData> allOresData;

    public int OreFromDepthCalc(int yDepth)
    {
        for (int i = 0; i < allOresData.Count; i++)
        {
            if(allOresData[i].oreUpperDepth >= yDepth && yDepth >= allOresData[i].oreLowerDepth)
            {
                return i;
            }
        }
        return -1;
    }
}
