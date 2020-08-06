using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bliiboard : MonoBehaviour
{
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
