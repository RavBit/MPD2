using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

public class LobbyManager : NetworkBehaviour {
    public GameObject Lobbypc;
    public GameObject Explain;
    public VideoPlayer VP;
    public static LobbyManager instance;
    public delegate void LobbyToggle(bool enable);
    public delegate void UpdateEvent();
    public delegate void GameState();
    [SyncEvent]
    public static event GameState EventStartGame;
    [SyncEvent]
    public static event LobbyToggle EventonToggle;
    [SyncEvent]
    public static event UpdateEvent EventUpdateInterface;
    [SyncEvent]
    public static event LobbyToggle EventLobbyToggle;

    public GameObject IntroContainer;
    void Awake()
    {
        EventStartGame += CmdLoadMiniGame;
        MiniGameManager.EventLoadMinigame += CmdLoadMiniGame;
        if (instance != null)
            Debug.LogError("More than one LobbyManager in scene");
        else
            instance = this;
    }
    public void UpdateLobbyInterface()
    {
        Debug.Log("Lobby Interface");
        if(!isClient)
             GetComponent<LobbyInterface>().RpcUpdatePlayerList();
    }

    public void StartGame()
    {
        StartCoroutine(CorStartGame());
    }

    IEnumerator CorStartGame()
    {
        yield return new WaitForSeconds(2);
        CmdStartGame();
    }

    [Command]
    void CmdStartGame()
    {
        if (GameManager.players.Keys.Count == 0)
        {
            Debug.Log("No Players Connected");
            return;
        }
        foreach (string _playerID in GameManager.players.Keys)
        {
            if (!GameManager.players[_playerID].GetComponent<Lobby>().isReady)
            {
                Debug.Log("Not everyone is ready.");
                return;
            }
        }
        Lobbypc.SetActive(false);
        RpcClientDisableLobby();
        VP.gameObject.SetActive(true);
        VP.Play();
        /*IntroContainer.gameObject.SetActive(true);
        IntroContainer.GetComponent<RawImage>().texture = movie as MovieTexture;
        movie.loop = false;
        movie.Play();
        IntroContainer.GetComponent<AudioSource>().Play();*/
        StartCoroutine("StartTheGameCor");
    }
    IEnumerator StartTheGameCor()
    {
        yield return new WaitForSeconds(24);
        VP.Stop();
        VP.gameObject.SetActive(false);
        Explain.SetActive(true);
        yield return new WaitForSeconds(10f);
        Explain.SetActive(false);
        CmdXStartGame();
    }
    [Command]
    void CmdXStartGame()
    {
        Debug.Log("Command for game starting");
        IntroContainer.gameObject.SetActive(false);
        EventStartGame();
    }

    [ClientRpc]
    void RpcClientDisableLobby()
    {
        EventonToggle(false);
    }

    [Command]
    public void CmdLoadMiniGame()
    {
#if UNITY_STANDALONE_WIN
            int num = Random.Range(0, MiniGame_Container.instance.MiniGame.Length);
            MiniGameManager.instance.StartCorLM(num);
#endif
    }
}
