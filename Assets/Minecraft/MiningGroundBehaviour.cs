using System;
using UnityEngine;

public class MiningGroundBehaviour : MonoBehaviour
{

    public static MiningGroundBehaviour sRef;
    private void Awake()
    {
        sRef = this;
    }

    public GameObject _prefabGroundCube;
    public int groundSizeX = 1;
    public int groundSizeY = 1;
    public int groundSizeZ = 1;
    public CubePosition[] DIR = new CubePosition[6];

    private GroundBlockData[,,] worldData;
    // Important! This indicates the number of blocks and not unity units
    [Header("Important! This indicates the number of blocks and not unity units")]
    public int worldSizeX = 10;
    public int worldSizeY = 10;
    public int worldSizeZ = 10;

    public Transform[] initGroundBlocks;

    public Transform originAndGround;
    private float originAndGroundYPos;

    public WorldAllOreData allOresData;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        originAndGroundYPos = originAndGround.position.y;
        DIR[0] = new CubePosition(groundSizeX, 0, 0);
        DIR[1] = new CubePosition(-groundSizeX, 0, 0);
        DIR[2] = new CubePosition(0, 0, groundSizeZ);
        DIR[3] = new CubePosition(0, 0, -groundSizeZ);
        DIR[4] = new CubePosition(0, groundSizeY, 0);
        DIR[5] = new CubePosition(0, -groundSizeY, 0);
        worldData = new GroundBlockData[worldSizeX, worldSizeY, worldSizeZ];
        for (int i = 0; i < worldSizeX; i++)
        {
            for (int j = 0; j < worldSizeY; j++)
            {
                for (int k = 0; k < worldSizeZ; k++)
                {
                    worldData[i, j, k] = new GroundBlockData(false, true);
                    //Debug.Log(i + " " + j + " " + k + "\n");
                }
            }
        }


        for (int i = 0; i < initGroundBlocks.Length; i++)
        {
            int[] idx = GetIdx(initGroundBlocks[i].position);
            worldData[idx[0], idx[1], idx[2]].SetPosForInitCubesOnly(initGroundBlocks[i].position);
        }

    }

    public void MineAtPos(Vector3 groundCubePos, Vector3 normal)
    {
        //Debug.Log(groundCubePos);
        normal.x *= groundSizeX;
        normal.y *= groundSizeY;
        normal.z *= groundSizeZ;
        //Debug.Log(normal);

        int[] idx = GetIdx(groundCubePos);
        worldData[idx[0], idx[1], idx[2]].MineBlock();

        InstantiateGroundCubes(groundCubePos, normal);

    }

    private void InstantiateGroundCubes(Vector3 groundCubePos, Vector3 normalToScale)
    {
        Vector3 currPosOffset;
        int[] idx;
        for (int i = 0; i < DIR.Length; i++)
        {
            currPosOffset = DIR[i].CubeDirPosVector3();
            //player facing
            if (currPosOffset.Equals(normalToScale))
            {
                //Debug.Log("player facing");
                continue;
            }
            //For mining above ground
            Vector3 posToSpawn = groundCubePos + currPosOffset;
            if (i == 4 && posToSpawn.y > originAndGroundYPos)
            {
                continue;
            }

            //Debug.Log("i = " + i);
            //Debug.Log("DIR[i].CubePosVector3() = " + currPosOffset);


            idx = GetIdx(posToSpawn);
            if(worldData[idx[0], idx[1], idx[2]].isMinedBefore)
            {
                continue;
            }
            if (!worldData[idx[0], idx[1], idx[2]].isHollow)
            {
                continue;
            }
            int idx = new OreGeneration(allOresData).GenerateOre(posToSpawn.y);
            Debug.Log("idx = " + idx);
            _prefabGroundCube = allOresData.allOresData[idx].orePrefab;

            Instantiate(_prefabGroundCube, posToSpawn, Quaternion.identity, transform);
            worldData[idx[0], idx[1], idx[2]].isMinedBefore = false;
            worldData[idx[0], idx[1], idx[2]].isHollow = false;
        }
    }



    private int[] GetIdx(Vector3 pos)
    {
        int x, y, z;
        x = (int)pos.x;
        y = (int)pos.y;
        z = (int)pos.z;

        x += 20 - 1;
        y += 20 - 1;
        z += 20 - 1;

        x /= groundSizeX;
        y /= groundSizeY;
        z /= groundSizeZ;

        return new int[] { x, y, z };

    }

}

public class CubePosition
{
    private int x, y, z;

    public CubePosition(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 CubeDirPosVector3()
    {
        return new Vector3(x, y, z);
    }

}
