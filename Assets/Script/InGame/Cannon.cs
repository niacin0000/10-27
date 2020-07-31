using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cannon : MonoBehaviour
{

    public float speed = 20.0f;
    private Transform tr;
    private float time;
    public Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        tr.Translate(Vector3.forward * speed * Time.deltaTime);

        time += Time.deltaTime;

        if( time > 3.0f)
        {
            Destroy(gameObject);
        }


    }
}
