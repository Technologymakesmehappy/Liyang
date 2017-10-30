using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExitGames.SportShooting
{
    public class AndroidShooter : Shooter
    {
        public void Update()
        {
            if (_photonView != null && _photonView.isMine && (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.K)))
            {
                ShootAttempt();
            }
        }
    }
}
