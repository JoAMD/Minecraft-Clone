using UnityEngine;

public class GroundBlockData
{
    public bool isMinedBefore;
    public bool isHollow;
    public int[] pos;

    public GroundBlockData(bool isMined)
    {
        this.isMinedBefore = isMined;
    }

    public GroundBlockData(bool isMinedBefore, bool isHollow)
    {
        this.isMinedBefore = isMinedBefore;
        this.isHollow = isHollow;
    }

    public GroundBlockData(bool isMinedBefore, bool isHollow, Vector3 pos)
    {
        this.isMinedBefore = isMinedBefore;
        this.isHollow = isHollow;

        this.pos = new int[3];
        this.pos[0] = (int)pos.x;
        this.pos[0] = (int)pos.y;
        this.pos[0] = (int)pos.z;
    }

    //Use only for init cubes since isHollow is also set
    public void SetPosForInitCubesOnly(Vector3 pos)
    {
        isHollow = false;
        this.pos = new int[3];
        this.pos[0] = (int)pos.x;
        this.pos[0] = (int)pos.y;
        this.pos[0] = (int)pos.z;
    }

    public Vector3 BlockPosVector3()
    {
        return new Vector3(pos[0], pos[1], pos[2]);
    }

    public void MineBlock()
    {
        isMinedBefore = true;
        isHollow = true;
    }

}
