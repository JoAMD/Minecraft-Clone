using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickaxe : MonoBehaviour
{

    RaycastHit hit;
    //public PlayerController _PlayerController;
    public Transform AimAt;
    private GameObject lastHitObject = null;
    public GameObject HPBarCanvas;
    public Slider HPBar;
    //public Text BlockHpText;
    //public Text CoinsPerHitText;
    public string PickaxeName = "";
    public int PickaxeDmg = 100;
    public float Inrangee = 5;
    private float Counter = 1;
    public float PickaxeDelay = 1;

    private void Update()
    {
        MineBlock();
    }

    private void TextUI()
    {
        HPBarCanvas.SetActive(true);
        //BlockHpText.text = hit.collider.GetComponent<MiningBlock>().BlockName + " - " + hit.collider.GetComponent<MiningBlock>().CurBlockHP.ToString();
        //CoinsPerHitText.text = "Value: " + hit.collider.GetComponent<MiningBlock>().CoinsPerHit;
        HPBar.value = Counter;
        HPBar.maxValue = PickaxeDelay;
    }

    private bool MiningSpeed()
    {
        if (Counter >= PickaxeDelay)
        {
            Counter = 0;
            return true;
        }
        else
        {
            Counter += Time.deltaTime;
            return false;
        }
    }

    private bool CanCarry()
    {
        //if (_PlayerController.CurCarryWeight >= _PlayerController.MaxCarryWeight)
        {
            return false;
        }
        //else
        //{
        //    return false;
        //}
    }

    private bool BlockDead()
    {
        MiningGroundBehaviour.sRef.MineAtPos(hit.collider.transform.position, hit.normal);
        Destroy(hit.collider.gameObject);
        return true;
        //if (hit.collider.GetComponent<MiningBlock>().CurBlockHP <= 0)
        //{
        //    if (hit.collider.CompareTag("Stone") || hit.collider.CompareTag("Rock"))
        //    {
        //        hit.collider.GetComponent<MiningBlock>().MakeNewBlock();
        //        _PlayerController.MiningCoins += hit.collider.GetComponent<MiningBlock>().MaxBlockHP *= hit.collider.GetComponent<MiningBlock>().CoinsPerHit;
        //        _PlayerController.CurCarryWeight += hit.collider.GetComponent<MiningBlock>().MaxBlockHP;
        //    }
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
    }

    private void ResetTimeOnChange(GameObject nextHitObject)
    {

        if (lastHitObject != null && nextHitObject != null)
        {
            if (lastHitObject.gameObject == nextHitObject.gameObject) return;
            {
                lastHitObject = nextHitObject;
                Counter = 0;
                //Debug.Log("reset when changing stones");
                return;
            }
        }

        if (lastHitObject == null && nextHitObject != null)
        {
            lastHitObject = nextHitObject;
            Counter = 0;
            return;
        }
    }

    private void MineBlock()
    {
        //Debug.Log("counter = " + Counter);
        if (Physics.Raycast(AimAt.transform.position, AimAt.transform.forward, out hit, Inrangee))
        {
            Debug.DrawRay(AimAt.position, AimAt.transform.forward * Inrangee, Color.green);

            if (hit.collider.CompareTag("Stone") || hit.collider.CompareTag("Rock"))
            {
                TextUI();
                ResetTimeOnChange(hit.collider.gameObject);
            }
            else
            {
                HPBarCanvas.SetActive(false);
                Counter = 0;
            }

            if (Input.GetKey(KeyCode.Mouse0) && !CanCarry())
            {
                if (hit.collider.CompareTag("Stone") || hit.collider.CompareTag("Rock"))
                {
                    if (MiningSpeed())
                    {
                        //hit.collider.GetComponent<MiningBlock>().CurBlockHP -= PickaxeDmg;
                        BlockDead();
                    }
                }
            }
            else
            {
                Counter = 0;
            }
        }
        else
        {
            HPBarCanvas.SetActive(false);
            Counter = 0;
        }
    }
}
