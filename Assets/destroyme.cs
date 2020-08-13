using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyme : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Dest", 0.8f);
    }

    private void Dest()
    {
        Destroy(gameObject);
    }
}
