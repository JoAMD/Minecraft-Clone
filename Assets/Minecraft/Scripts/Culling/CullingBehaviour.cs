using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CullingBehaviour : CullingHelper
{
    public static CullingBehaviour sRef;
    private void Awake()
    {
        sRef = this;
    }

    public int cullingMaxDist = 20;
    public bool isCullingEnabled = true;
    public float cullingInterval = 2;
    public Transform player;

    public Transform start;
    public Transform end;

    private void Start()
    {
        StartCoroutine(CallCullingFns());
    }

    public void ChangeCullingDistance(TMP_InputField inputField)
    {
        if (int.TryParse(inputField.text, out int result))
        {
            cullingMaxDist = result;
        }
        else
        {
            Debug.LogWarning("Not an integer");
        }
    }

    private IEnumerator CallCullingFns()
    {
        while (isCullingEnabled)
        {
            yield return new WaitForSeconds(cullingInterval);
            CullingRun(player.transform.position, cullingMaxDist, ref start, ref end);
        }
    }

    public void Force_CullingRun()
    {
        CullingRun(player.transform.position, cullingMaxDist, ref start, ref end);
    }

}
