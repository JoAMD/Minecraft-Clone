﻿using System;
using UnityEngine;

public enum OreType
{
    Nil,
    Coal,
    Gold,
    Diamond
}

public class OreGeneration
{
    public WorldAllOreData allOreData;

    public OreGeneration(WorldAllOreData allOreData)
    {
        this.allOreData = allOreData ?? throw new ArgumentNullException(nameof(allOreData));
    }

    public int GenerateOre(float blockYPos)
    {
        int yPos = (int)blockYPos;
        int oreTypeIdx = allOreData.OreFromDepthCalc(yPos);
        return oreTypeIdx;
    }

}
