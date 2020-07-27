using UnityEngine;

public class CullingHelper : MonoBehaviour
{
    private int[] playerPosIdx;

    protected virtual void CullFunction(bool isEnable, int i, int j, int k)
    {
        if(MiningGroundBehaviour.sRef.worldData[i, j, k].rend != null)
        {
            MiningGroundBehaviour.sRef.worldData[i, j, k].rend.enabled = isEnable;
        }
        else
        {
            Debug.Log("rend = null, ijk = " + i + " " + j + " " + k + " : isEnable = " + isEnable);
        }
    }

    protected virtual void GroundCullHelper(int i, int j, int k)
    {

    }

    protected void CullingRun(Vector3 playerPos, int cullingMaxDist, ref Transform start, ref Transform end)
    {
        bool isOnce = false;
        playerPosIdx = MiningGroundBehaviour.sRef.GetIdx(playerPos);

        Vector3 cullStartPos = playerPos;
        cullStartPos.x -= cullingMaxDist * 2;
        cullStartPos.y -= cullingMaxDist * 2;
        cullStartPos.z -= cullingMaxDist * 2;
        start.position = cullStartPos;
        Vector3 cullEndPos = playerPos;
        cullEndPos.x += cullingMaxDist * 2;
        cullEndPos.y += cullingMaxDist * 2;
        cullEndPos.z += cullingMaxDist * 2;
        end.position = cullEndPos;

        int[] startIdx = MiningGroundBehaviour.sRef.GetIdx(cullStartPos);
        int[] endIdx = MiningGroundBehaviour.sRef.GetIdx(cullEndPos);

        //Debug.Log("start = " + startIdx[0]);
        //Debug.Log("start = " + startIdx[1]);
        //Debug.Log("start = " + startIdx[2]);
        //Debug.Log("end = " + endIdx[0]);
        //Debug.Log("end = " + endIdx[1]);
        //Debug.Log("end = " + endIdx[2]);

        for (int i = Mathf.Max(0, startIdx[0]); i < Mathf.Min(endIdx[0], MiningGroundBehaviour.sRef.worldSizeX); i++)
        {
            for (int j = Mathf.Max(0, startIdx[1]); j < Mathf.Min(endIdx[1], MiningGroundBehaviour.sRef.worldSizeY); j++)
            {
                for (int k = Mathf.Max(0, startIdx[2]); k < Mathf.Min(endIdx[2], MiningGroundBehaviour.sRef.worldSizeZ); k++)
                {
                    int distFromPlayer = CalcDistFromPlayer(new int[] { i, j, k });
                    if (MiningGroundBehaviour.sRef.worldData[i, j, k].isSpawned)
                    {
                        //Debug.Log(i + " " + j + " " + k + "\n"); // will cause non responsive editor!!
                        //if (distFromPlayer > cullingMaxDist * 2.5f)
                        //{
                        //    if (isOnce)
                        //    {
                        //        return;
                        //    }
                        //    isOnce = true;
                        //    i = playerPosIdx[0] - cullingMaxDist;
                        //    j = playerPosIdx[1] - cullingMaxDist;
                        //    k = playerPosIdx[2] - cullingMaxDist;
                        //}
                        if (distFromPlayer > cullingMaxDist)
                        {
                            CullFunction(false, i, j, k);
                        }
                        else
                        {
                            CullFunction(true, i, j, k);
                        }
                    }
                    else
                    {
                        if (distFromPlayer > cullingMaxDist)
                        {

                        }
                        else
                        {
                            GroundCullHelper(i, j, k);
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
