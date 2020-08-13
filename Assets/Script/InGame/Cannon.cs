using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cannon : MonoBehaviourPunCallbacks
{

    public float speed = 20.0f;
    private Transform tr;
    public Text timeText;
    public GameObject explosion, staff;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();

        staff = GameObject.FindGameObjectWithTag("STAFF").gameObject;

        Invoke("DestroyMe", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        tr.Translate(Vector3.forward * speed * Time.deltaTime);

    }


    private void DestroyMe()
    {
        Destroy(gameObject);
    }


}
