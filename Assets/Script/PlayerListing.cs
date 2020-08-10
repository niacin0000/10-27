using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _text;
    [SerializeField]
    private Image _image;

    public Player Player { get; private set; }

    public bool TeamRedBlu;


    private void Start()
    {
        GameObject.Find("Button - Switch").GetComponent<Button>().onClick.AddListener(
    delegate
    {
        SwitchTeam();
    });
    }

    public void SetPlayerInfo(Player player, bool RedBlu)
    {
        Player = player;
        _text.text = player.NickName;
        _image.color = new Color(0, 0, 255);
        TeamRedBlu = RedBlu;
    }

    public void SwitchTeam()
    {
            if (TeamRedBlu == true)
            {
                this.TeamRedBlu = false;
                Debug.Log("false");
                this._image.color = new Color(0, 0, 255);
            }
            else if (TeamRedBlu == false)
            {
                this.TeamRedBlu = true;
                Debug.Log("true");
                this._image.color = new Color(255, 0, 0);
            }
            else
                return;
    }
}
