using System.Collections;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Class used by the missiles generator.
/// </summary>
public class MissileGenerator : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine (SpawnMissiles(NetworkManager.networkManager.isConnected));
    }

    Vector2 SpawnPosition()
    {
        int randonNumber = Random.Range(1, 4);

        switch (randonNumber)
        {
            case 1:
                return new Vector2(11f, 2.23f);
            case 2:
                return new Vector2(11f, -3.5f);
            case 3:
                return new Vector2(-11f, 2.23f);
            default:
                return new Vector2(-11f, -3.5f);
        }

    }

    /// <summary>
    /// Function that is responsible for generating missiles.
    /// </summary>
    void GenerateMissiles()
    {
        GameObject missile = ObjectPooler.SharedInstance.GetPooledObject("Game1/Missile");
        
        if (missile != null)
        {
            missile.transform.position = SpawnPosition();
            missile.transform.rotation = Quaternion.identity;
            missile.SetActive(true);
        }
    }

    void InstantiateMissiles()
    {
        PhotonNetwork.InstantiateRoomObject("1Missile", SpawnPosition(), Quaternion.identity);
    }

    /// <summary>
    /// Corroutine that calls the function to generate missiles after a few seconds.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnMissiles(bool multiplayer)
    {
        yield return new WaitForSeconds(3);

        while (true)
        {
            if (multiplayer)
            {
                InstantiateMissiles();
            }

            else
            {
                GenerateMissiles();
            }

            yield return new WaitForSeconds(Random.Range(4, 7));
        }
    }

}
