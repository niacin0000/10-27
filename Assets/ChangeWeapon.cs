using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : MonoBehaviourPunCallbacks
{
    //public GameObject Soul, Espadon, Shield, Staff, Sword;
    private bool change = true;


    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("ESPA") && change)
        {
            photonView.RPC("Change_E", RpcTarget.AllViaServer, null);
            change = false;
        }
        else if (collision.collider.CompareTag("SHIELD") && change)
        {
            photonView.RPC("Change_Shi", RpcTarget.AllViaServer, null);
            change = false;
        }
        else if (collision.collider.CompareTag("STAFF") && change)
        {
            photonView.RPC("Change_Sta", RpcTarget.AllViaServer, null);
            change = false;
        }
        else if (collision.collider.CompareTag("SWORD") && change)
        {
            photonView.RPC("Change_Swo", RpcTarget.AllViaServer, null);
            change = false;
        }

    }

    [PunRPC]
    void Change_E()
    {

        transform.Find("Soul").gameObject.SetActive(false);
        transform.Find("Espadon").gameObject.SetActive(true);
        transform.Find("Shield").gameObject.SetActive(false);
        transform.Find("Staff").gameObject.SetActive(false);
        transform.Find("Sword").gameObject.SetActive(false);

        transform.Find("Espadon").gameObject.tag = "ROBO";

        Destroy(GameObject.FindGameObjectWithTag("ESPA"));
    }
    [PunRPC]
    void Change_Shi()
    {

        transform.Find("Soul").gameObject.SetActive(false);
        transform.Find("Espadon").gameObject.SetActive(false);
        transform.Find("Shield").gameObject.SetActive(true);
        transform.Find("Staff").gameObject.SetActive(false);
        transform.Find("Sword").gameObject.SetActive(false);

        transform.Find("Shield").gameObject.tag = "ROBO";

        Destroy(GameObject.FindGameObjectWithTag("SHIELD"));
    }
    [PunRPC]
    void Change_Sta()
    {

        transform.Find("Soul").gameObject.SetActive(false);
        transform.Find("Espadon").gameObject.SetActive(false);
        transform.Find("Shield").gameObject.SetActive(false);
        transform.Find("Staff").gameObject.SetActive(true);
        transform.Find("Sword").gameObject.SetActive(false);

        transform.Find("Staff").gameObject.tag = "ROBO";

        Destroy(GameObject.FindGameObjectWithTag("STAFF"));
    }
    [PunRPC]
    void Change_Swo()
    {

        transform.Find("Soul").gameObject.SetActive(false);
        transform.Find("Espadon").gameObject.SetActive(false);
        transform.Find("Shield").gameObject.SetActive(false);
        transform.Find("Staff").gameObject.SetActive(false);
        transform.Find("Sword").gameObject.SetActive(true);

        transform.Find("Sword").gameObject.tag = "ROBO";

        Destroy(GameObject.FindGameObjectWithTag("SWORD"));
    }

}
