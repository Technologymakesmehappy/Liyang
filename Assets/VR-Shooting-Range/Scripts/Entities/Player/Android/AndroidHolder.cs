using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ExitGames.SportShooting
{
    public class AndroidHolder : Holder
    {

        public OptitrackStreamingClient StreamingClient;
        private Int32 RigidBodyId;

        void Start()
        {
            // If the user didn't explicitly associate a client, find a suitable default.
            if (this.StreamingClient == null)
            {
                this.StreamingClient = OptitrackStreamingClient.FindDefaultClient();

                // If we still couldn't find one, disable this component.
                if (this.StreamingClient == null)
                {
                    Debug.LogError(GetType().FullName + ": Streaming client not set, and no " + typeof(OptitrackStreamingClient).FullName + " components found in scene; disabling this component.", this);
                    this.enabled = false;
                    return;
                }
            }
        }

        void Update()
        {
            OptitrackRigidBodyState rbState = StreamingClient.GetLatestRigidBodyState(RigidBodyId);
            if (rbState != null)
            {
                this.transform.localPosition = rbState.Pose.Position;
                this.transform.localRotation = rbState.Pose.Orientation;
            }
        }

        protected override void Awake()
        {
            var photonView = GetComponent<PhotonView>();
            if (!photonView.isMine)
            {
                Destroy(this);
            }
            else
            {
                
                RigidBodyId = UserID.id * 2 + 1;
            }
        }
    }
}
