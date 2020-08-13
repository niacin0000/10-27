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

    public void SetPlayerInfo(Player player, bool RedBlu)
    {
        Player = player;
        _text.text = player.NickName;
        TeamRedBlu = RedBlu;
    }
}
