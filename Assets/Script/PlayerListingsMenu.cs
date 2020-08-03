using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content_Red;
    [SerializeField]
    private Transform _content_Blu;
    [SerializeField]
    private PlayerListing _playerListing;
    [SerializeField]
    private Button _RedTeamButton;
    [SerializeField]
    private Button _BluTeamButton;

    private List<PlayerListing> _listing = new List<PlayerListing>();
    private List<PlayerListing> _listing_Red = new List<PlayerListing>();
    private List<PlayerListing> _listing_Blu = new List<PlayerListing>();

    private bool RedOrBlu;

    private void Awake()
    {
        GetCurrentRoomPlayers();
    }

    private void Start()
    {
    }

    private void GetCurrentRoomPlayers()
    {
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
        
    }
    
    private void AddPlayerListing(Player player)
    {
        // team
        //if (_listing_Blu.Count >= _listing_Red.Count)
        //{
        //    PlayerListing listing = Instantiate(_playerListing, _content_Red);
        //    if (listing != null)
        //    {
        //        listing.SetPlayerInfo(player, true);
        //        _listing_Red.Add(listing);
        //    }
        //}
        //else
        //{
        //    PlayerListing listing = Instantiate(_playerListing, _content_Blu);
        //    if (listing != null)
        //    {
        //        listing.SetPlayerInfo(player, false);
        //        _listing_Blu.Add(listing);
        //    }
        //}
        // solo
        PlayerListing listing = Instantiate(_playerListing, _content_Red);
        if (listing != null)
        {
            listing.SetPlayerInfo(player, true);
            _listing.Add(listing);
        }
    }

    //public void ChangeTeamBlu(Player player)
    //{
    //    PlayerListing listing_R = Instantiate(_playerListing, _content_Red);
    //    PlayerListing listing_B = Instantiate(_playerListing, _content_Blu);
    //    if (listing_B != null)
    //    {
    //        listing_B.SetPlayerInfo(player, true);
    //        _listing_Blu.Remove(listing_R);
    //        _listing_Red.Add(listing_B);
    //    }
    //}
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = _listing.FindIndex(x => x.Player == otherPlayer);
        //if (index_R != -1)
        //{
        //    Destroy(_listing_Red[index_R].gameObject);
        //    _listing_Red.RemoveAt(index_R);
        //}
        //int index_B = _listing_Blu.FindIndex(x => x.Player == otherPlayer);
        //if (index_B != -1)
        //{
        //    Destroy(_listing_Red[index_B].gameObject);
        //    _listing_Red.RemoveAt(index_B);
        //}
        if (index != -1)
        {
            Destroy(_listing[index].gameObject);
            _listing.RemoveAt(index);
        }
    }
}
