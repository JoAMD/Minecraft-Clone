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

    public GroundBlockData[,,] worldData;
    // Important! This indicates the number of blocks and not unity units
    [Header("Important! This indicates the number of blocks and not unity units")]
    public int worldSizeX = 10;
    public int worldSizeY = 10;
    public int worldSizeZ = 10;

    public Transform[] initGroundBlocksHolder;

    /// <summary>
    /// Probablytop right, have to test
    /// </summary>
    public Transform originAndGround;
    private float originAndGroundYPos;

    public WorldAllOreData allOresData;
    public bool isDoneLoadingBlocks = false;

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
                    //Debug.Log(i + " " + j + " " + k + "\n"); // will cause non responsive editor!!
                }
            }
        }

        int[] idxOriginGnd = GetIdx(originAndGround.position);
        Vector3 pos;
        for (int i = 0; i < worldSizeX; i++)
        {
            for (int j = 0; j < worldSizeZ; j++)
            {
                pos = GetPos(new int[] { i, idxOriginGnd[1], j });
                worldData[i, idxOriginGnd[1], j].SetPosForInitCubesOnly(pos);

                worldData[i, idxOriginGnd[1], j].rend = Instantiate(_prefabGroundCube, pos, Quaternion.identity, transform).GetComponent<Renderer>();
                worldData[i, idxOriginGnd[1], j].rend.enabled = false;
                worldData[i, idxOriginGnd[1], j].isSpawned = true;
                worldData[i, idxOriginGnd[1], j].isMinedBefore = false;
                //Debug.Log(i + " " + j + " " + k + "\n");
            }
        }

        //for (int i = 0; i < initGroundBlocksHolder.Length; i++)
        //{
        //    for (int j = 0; j < initGroundBlocksHolder[i].childCount; j++)
        //    {
        //        int[] idx = GetIdx(initGroundBlocksHolder[i].GetChild(j).position);
        //        worldData[idx[0], idx[1], idx[2]].SetPosForInitCubesOnly(initGroundBlocksHolder[i].GetChild(j).position);
        //    }
        //}

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
        }

        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //}
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
            //Debug.Log("idxX = " + idx[0] + "; idxY = " + idx[1] + "; idxZ = " + idx[2]);
            if(worldData[idx[0], idx[1], idx[2]].isMinedBefore)
            {
                continue;
            }
            if (!worldData[idx[0], idx[1], idx[2]].isHollow)
            {
                continue;
            }
            int oreIdx = new OreGeneration(allOresData).GenerateOre(posToSpawn.y);
            //Debug.Log("idx = " + oreIdx);

            _prefabGroundCube = allOresData.allOresData[oreIdx].orePrefab;

            worldData[idx[0], idx[1], idx[2]].rend = Instantiate(_prefabGroundCube, posToSpawn, Quaternion.identity, transform).GetComponent<Renderer>();
            worldData[idx[0], idx[1], idx[2]].isMinedBefore = false;
            worldData[idx[0], idx[1], idx[2]].isHollow = false;
            worldData[idx[0], idx[1], idx[2]].isSpawned = true;
        }
    }



    public int[] GetIdx(Vector3 pos)
    {
        int x, y, z;
        x = (int)pos.x;
        y = (int)pos.y;
        z = (int)pos.z;

        x += worldSizeX / 2 - 1;
        y += worldSizeY - 1;
        z += worldSizeZ / 2 - 1;

        x /= groundSizeX;
        y /= groundSizeY;
        z /= groundSizeZ;

        return new int[] { x, y, z };
    }

    private Vector3 GetPos(int[] idx)
    {
        Vector3 pos = new Vector3(idx[0] * groundSizeX, idx[1] * groundSizeY, idx[2] * groundSizeZ);

        pos.x += -(worldSizeX / 2 - 1);
        pos.y += -(worldSizeY - 1);
        pos.z += -(worldSizeZ / 2 - 1);

        return pos;
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
