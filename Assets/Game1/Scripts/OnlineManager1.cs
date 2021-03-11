using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class OnlineManager1 : MonoBehaviourPunCallbacks
{
    public static OnlineManager1 onlineManager;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelLostPlayer1 = null;

    [Header("Generators")]
    [SerializeField] GameObject[] generators = null;
    
    [Header("Score")]
    int score1 = 0;
    [SerializeField] Text score1Text = null;
    int score2 = 0;
    [SerializeField] Text score2Text = null;

    [Header("Player")]
    GameObject player;
    Photon.Realtime.Player[] players;
    public int playerNumber = 0;

    [Header("Sounds")]
    [SerializeField] AudioSource hurtSound = null;
    [SerializeField] AudioSource coinSound = null;

    private void Awake()
    {
        onlineManager = this;
    }

    public void StartGame()
    {
        photonView.RPC("EnableGenerators", RpcTarget.MasterClient);

        photonView.RPC("CleanScore", RpcTarget.All, true);
        photonView.RPC("CleanScore", RpcTarget.All, false);

        InstantiatePlayer();

        panelMenu.SetActive(false);
    }

    /// <summary>
    /// Function that returns the player to the original position after dying.
    /// </summary>
    public void Respawn()
    {
        player = null;

        photonView.RPC("PlaySound", RpcTarget.All, "hurt");

        StartCoroutine(WaitForRespawn());
    }

    void InstantiatePlayer()
    {
        if (player != null)
        {
            return;
        }

        switch (playerNumber)
        {
            case 1:
                player = PhotonNetwork.Instantiate("1Player1Server", new Vector2(-6.3f, -5.4f), Quaternion.identity);
                photonView.RPC("CleanScore", RpcTarget.All, true);
                break;

            case 2:
                player = PhotonNetwork.Instantiate("1Player2Server", new Vector2(6.3f, -5.4f), Quaternion.identity);
                photonView.RPC("CleanScore", RpcTarget.All, false);
                break;
        }
    }

    [PunRPC] void EnableGenerators()
    {
        for (int i = 0; i < generators.Length; i++)
        {
            generators[i].SetActive(true);
        }
    }

    [PunRPC] void PlaySound(string soundToPlay)
    {
        switch (soundToPlay)
        {
            case "coin":
                coinSound.Play();
                break;
            case "hurt":
                hurtSound.Play();
                break;
        }
    }

    public void Scored(bool isPlayer1)
    {
        photonView.RPC("UpdateScore", RpcTarget.All, isPlayer1);
    }

    public void DestroyCoin(int coin)
    {
        photonView.RPC("DestroyCoinServer", RpcTarget.MasterClient, coin);
    }

    [PunRPC] void DestroyCoinServer(int coin)
    {
        if (PhotonView.Find(coin) != null)
        {
            PhotonNetwork.Destroy(PhotonView.Find(coin));
        }
    }

    [PunRPC] void UpdateScore(bool isPlayer1)
    {
        photonView.RPC("PlaySound", RpcTarget.All, "coin");

        if (isPlayer1)
        {
            score1 += 1;
            score1Text.text = "Score: " + score1;
        }

        else 
        {
            score2 += 1;
            score2Text.text = "Score: " + score2;
        }
    }

    [PunRPC] void CleanScore(bool isPlayer1)
    {
        if (isPlayer1)
        {
            score1 = 0;
            score1Text.text = "Score: " + score1;
        }

        else
        {
            score2 = 0;
            score2Text.text = "Score: " + score2;
        }
    }

    /// <summary>
    /// Function that pauses and resumes the game.
    /// </summary>
    public void PauseGame()
    {
        if (!panelPause.activeSelf)
        {
            panelPause.SetActive(true);
        }
        
        else if (panelPause.activeSelf)
        {
            panelPause.SetActive(false);
        }
    }

    IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(2);
        {
            InstantiatePlayer();
        }
    }

    public override void OnJoinedRoom()
    {
        players = PhotonNetwork.PlayerList;

        playerNumber = players.Length;

        PhotonNetwork.NickName = playerNumber.ToString();

        StartGame();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (otherPlayer.NickName == "1")
        {
            PhotonNetwork.LeaveRoom();

            panelLostPlayer1.SetActive(true);
        }

        else
        {
            photonView.RPC("CleanScore", RpcTarget.All, false);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        GameManager1.manager.LoadGame(1);
    }
}
