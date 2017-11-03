using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : Photon.PunBehaviour {

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            if(photonView.isMine)
            {
                PhotonView.Destroy(gameObject);
            }
        }
    }
}
