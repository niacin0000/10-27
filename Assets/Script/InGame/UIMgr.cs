using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIMgr : MonoBehaviour
{
    public Image hpBar;
    public Image dpBar;

    private float currHP;
    private float initHP = 100.0f;
    private float currDP;
    private float initDP = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("ROBO"))
        {
            //체력 및 스태미너 바
            //currHP = GameObject.FindWithTag("ROBO").GetComponent<MoveCtrl>().currHP;
            currHP = GameObject.FindGameObjectWithTag("ROBO").GetComponent<MoveCtrl>().currHP;
            currDP = GameObject.FindGameObjectWithTag("ROBO").GetComponent<MoveCtrl>().currDP;

            hpBar.fillAmount = currHP / initHP;
            dpBar.fillAmount = currDP / initDP;
        }

    }
}
