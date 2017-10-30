using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private int spawnpointId;

    public void Awake()
    {
        GetComponent<MeshRenderer>().material.color = Color.grey;
    }

    public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        Hashtable props = (Hashtable)playerAndUpdatedProps[1];

        if (!props.ContainsKey("position"))
        {
            return;
        }

        int position = (int)props["position"];

        if (position != spawnpointId)
        {
            return;
        }

        GetComponent<MeshRenderer>().material.color = FindObjectOfType<ColorPerPlayer>().Colors[spawnpointId];
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        if (!player.CustomProperties.ContainsKey("position"))
        {
            return;
        }

        int position = (int)player.CustomProperties["position"];

        if (position != spawnpointId)
        {
            return;
        }

        GetComponent<MeshRenderer>().material.color = Color.grey;
    }

    public void RestoreDefaults()
    {
        GetComponent<MeshRenderer>().material.color = Color.grey;
    }
}