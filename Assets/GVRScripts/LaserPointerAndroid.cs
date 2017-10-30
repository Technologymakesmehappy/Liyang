using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExitGames.SportShooting
{
    public class LaserPointerAndroid : LaserPointer
    {

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.K ))
            {
                ClickOnHitObject();
            }

        }
    }

}
