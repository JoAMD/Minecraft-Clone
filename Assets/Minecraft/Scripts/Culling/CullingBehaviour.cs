using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingBehaviour : MonoBehaviour
{
    private int[] playerPosIdx;
    public int cullingMaxDist = 100;
    public bool isCullingEnabled = true;
    public float cullingInterval = 2;
    public Transform player;

    private void Start()
    {
        StartCoroutine(CallCullingFns());
    }

    private IEnumerator CallCullingFns()
    {
        while (isCullingEnabled)
        {
            yield return new WaitForSeconds(cullingInterval);
            CullingRun(player.transform.position);
        }
    }

    public void CullingRun(Vector3 playerPos)
    {
        playerPosIdx = MiningGroundBehaviour.sRef.GetIdx(playerPos);
        for (int i = 0; i < MiningGroundBehaviour.sRef.worldSizeX; i++)
        {
            for (int j = 0; j < MiningGroundBehaviour.sRef.worldSizeY; j++)
            {
                for (int k = 0; k < MiningGroundBehaviour.sRef.worldSizeZ; k++)
                {
                    //Debug.Log(i + " " + j + " " + k + "\n"); // will cause non responsive editor!!
                    int distFromPlayer = CalcDistFromPlayer(new int[] { i, j, k });
                    if(distFromPlayer > cullingMaxDist)
                    {
                        if (MiningGroundBehaviour.sRef.worldData[i, j, k].isSpawned) 
                        {
                            MiningGroundBehaviour.sRef.worldData[i, j, k].rend.enabled = false;
                        }
                    }
                    else
                    {
                        if (MiningGroundBehaviour.sRef.worldData[i, j, k].isSpawned)
                        {
                            MiningGroundBehaviour.sRef.worldData[i, j, k].rend.enabled = true;
                        }
                    }
                }
            }
        }
    }

    private int CalcDistFromPlayer(int[] blockPosIdx)
    {
        return Mathf.Abs(blockPosIdx[0] - playerPosIdx[0]) +
               Mathf.Abs(blockPosIdx[1] - playerPosIdx[1]) +
               Mathf.Abs(blockPosIdx[2] - playerPosIdx[2]);
    }
}
