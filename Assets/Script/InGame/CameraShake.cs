using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviourPunCallbacks
{
    public float m_force = 0f;
    public Vector3 m_offset;

    Quaternion m_originRot;

    // Start is called before the first frame update
    public void Start()
    {
        m_originRot = transform.rotation;
    }

    // Update is called once per frame
    public void Update()
    {
        if (GameObject.Find("Player1(Clone)") != null && GameObject.Find("Player1(Clone)").GetComponent<MoveCtrl>().t_Damage)
        {
            StopAllCoroutines();
            StartCoroutine(ShakeCoroutine());

            Invoke("Timer1", 0.1f);
        }
        else if (GameObject.Find("Player2(Clone)") != null && GameObject.Find("Player2(Clone)").GetComponent<MoveCtrl>().t_Damage)
        {
            StopAllCoroutines();
            StartCoroutine(ShakeCoroutine());

            Invoke("Timer2", 0.1f);
        }
        else if (GameObject.Find("Player3(Clone)") != null && GameObject.Find("Player3(Clone)").GetComponent<MoveCtrl>().t_Damage)
        {
            StopAllCoroutines();
            StartCoroutine(ShakeCoroutine());

            Invoke("Timer3", 0.1f);
        }
        else if (GameObject.Find("Player4(Clone)") != null && GameObject.Find("Player4(Clone)").GetComponent<MoveCtrl>().t_Damage)
        {
            StopAllCoroutines();
            StartCoroutine(ShakeCoroutine());

            Invoke("Timer4", 0.1f);
        }
        else
            return;
    }

    public void Timer1()
    {
        StopAllCoroutines();
        StartCoroutine(Reset());

        GameObject.Find("Player1(Clone)").GetComponent<MoveCtrl>().t_Damage = false;
    }
    public void Timer2()
    {
        StopAllCoroutines();
        StartCoroutine(Reset());

        GameObject.Find("Player2(Clone)").GetComponent<MoveCtrl>().t_Damage = false;
    }
    public void Timer3()
    {
        StopAllCoroutines();
        StartCoroutine(Reset());

        GameObject.Find("Player3(Clone)").GetComponent<MoveCtrl>().t_Damage = false;
    }
    public void Timer4()
    {
        StopAllCoroutines();
        StartCoroutine(Reset());

        GameObject.Find("Player4(Clone)").GetComponent<MoveCtrl>().t_Damage = false;
    }

    IEnumerator ShakeCoroutine()
    {
        Vector3 t_originEuler = transform.eulerAngles;
        while(true)
        {
            float t_rotX = Random.Range(-m_offset.x, m_offset.x);
            float t_rotY = Random.Range(-m_offset.y, m_offset.y);
            float t_rotZ = Random.Range(-m_offset.z, m_offset.z);

            Vector3 t_randomRot = t_originEuler + new Vector3(t_rotX, t_rotY, t_rotZ);
            Quaternion t_rot = Quaternion.Euler(t_randomRot);
            while(Quaternion.Angle(transform.rotation,t_rot)>0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, t_rot, m_force * Time.deltaTime);
                yield return null;
            }
            yield return null;
        }
    }
    IEnumerator Reset()
    {
        while(Quaternion.Angle(transform.rotation, m_originRot)>0f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, m_originRot, m_force * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);
        }
    }



}
