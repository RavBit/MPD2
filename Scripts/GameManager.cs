using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public MatchSettings MatchSettings;
    public GameObject LobbyManagment;
    public GameObject HostInterface;
    public GameObject Room;
    void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one GameManager in scene");
        else
            instance = this;
#if UNITY_STANDALONE_WIN
        instance.HostInterface.SetActive(true);
        Room.SetActive(true);
        //RoomManager.instance.gameObject.SetActive(true);
#endif
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }

    #region Player Settings

    private const string PLAYER_ID_PREFIX = "Guest";

    public static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string _netID, Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
        _player.displayname = _playerID;
        instance.LobbyManagment.GetComponent<LobbyManager>().UpdateLobbyInterface();
    }
    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
        //instance.LobbyManagment.GetComponent<LobbyManager>().UpdateLobbyInterface();
    }

    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    #endregion

    #region Match Settings

    #endregion
}
