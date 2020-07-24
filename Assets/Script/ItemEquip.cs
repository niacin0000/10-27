using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ItemEquip : MonoBehaviourPunCallbacks
{
    GameObject player;
    public GameObject playerEquipPoint, playerTakeDownPoint;
    
    MoveCtrl playerFunction;

    bool isPlayerEnter = false;

    private void Onestart()
    {
        player = GameObject.FindGameObjectWithTag("ROBO");
        playerEquipPoint = GameObject.FindGameObjectWithTag("EquipPoint");
        playerTakeDownPoint = GameObject.Find("TakeDownPoint");

        playerFunction = player.GetComponent<MoveCtrl>();
    }
    
    // Update is called once per frame
    void Update()
    {
        //if (GameObject.FindGameObjectWithTag("ROBO"))
        //{
        //    Invoke("Onestart", 0);
        //}

        Debug.Log("isPicking = >" + playerFunction.isPicking);
        if (Input.GetButtonDown("Fire1") && isPlayerEnter && playerFunction.isPicking == false)
        {
            Debug.Log("들기");
            transform.SetParent(playerEquipPoint.transform);
            transform.localPosition = Vector3.zero;
            transform.rotation = new Quaternion(0, 0, 0, 0);

            isPlayerEnter = false;

            playerFunction.Pickup(this.gameObject);
        }

        if (Input.GetButtonDown("Fire3") && playerFunction.isPicking)
        {
            Debug.Log("내려놓기");

            playerFunction.Drop();
            
        }

        if(!playerFunction.isPicking)
        {
            this.GetComponent<Collider>().enabled = true;
            this.GetComponent<Rigidbody>().isKinematic = false;

        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("무언가에 닿음");
        if (other.CompareTag("ROBO"))
        {
            Debug.Log("로보에 닿음");

            player = other.gameObject;
            playerEquipPoint = player.transform.Find("ItemPoint").gameObject;
            playerTakeDownPoint = player.transform.Find("head").gameObject.gameObject.transform.Find("TakeDownPoint").gameObject;

            playerFunction = player.GetComponent<MoveCtrl>();
            isPlayerEnter = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ROBO"))
        {
            Debug.Log("로보랑 떨어짐");
            isPlayerEnter = false;
        }
    }


}
