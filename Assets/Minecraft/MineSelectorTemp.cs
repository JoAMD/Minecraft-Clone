using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSelectorTemp : MonoBehaviour
{
    public Camera playerFPSCam;
    [SerializeField]
    private Vector3 minePos;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckMining();
        }
    }

    private void CheckMining()
    {
        Ray ray = playerFPSCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            minePos = hit.collider.transform.position;
            MiningGroundBehaviour.sRef.MineAtPos(minePos, hit.normal);
            Destroy(hit.collider.gameObject);
        }
    }

}
