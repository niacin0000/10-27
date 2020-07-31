using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            int R = Random.Range(1, 4);

            if(R == 1)
            {
                if(GameObject.Find("B_Wall(Clone)"))
                {
                    GameObject[] B_Wall = GameObject.FindGameObjectsWithTag("BOSSWALL");
                    for (int i = 0; i < B_Wall.Length; i++)
                    {
                        Destroy(B_Wall[i]);
                    }
                }

                Transform[] points = GameObject.Find("BossWallSpawn_0").GetComponentsInChildren<Transform>();
                for (int i = 1; i < 4; i++)
                {
                    PhotonNetwork.Instantiate("B_Wall", points[i].position, Quaternion.identity);
                }
            }
            else if (R == 2)
            {
                if (GameObject.Find("B_Wall(Clone)"))
                {
                    GameObject[] B_Wall = GameObject.FindGameObjectsWithTag("BOSSWALL");
                    for (int i = 0; i < B_Wall.Length; i++)
                    {
                        Destroy(B_Wall[i]);
                    }
                }

                Transform[] points = GameObject.Find("BossWallSpawn_1").GetComponentsInChildren<Transform>();
                for (int i = 1; i < 4; i++)
                {
                    PhotonNetwork.Instantiate("B_Wall", points[i].position, Quaternion.identity);
                }
            }
            else if (R == 3)
            {
                if (GameObject.Find("B_Wall(Clone)"))
                {
                    GameObject[] B_Wall = GameObject.FindGameObjectsWithTag("BOSSWALL");
                    for (int i = 0; i < B_Wall.Length; i++)
                    {
                        Destroy(B_Wall[i]);
                    }
                }

                Transform[] points = GameObject.Find("BossWallSpawn_2").GetComponentsInChildren<Transform>();
                for (int i = 1; i < 4; i++)
                {
                    PhotonNetwork.Instantiate("B_Wall", points[i].position, Quaternion.identity);
                }
            }

        }


    }
}
