using System;
using Akali.Common;
using UnityEngine;

namespace Akali.Scripts.Core
{
    public class SwerveFollower : Singleton<SwerveFollower>
    {
        private const float FollowSpeed = 0.375f;
        private Vector3 followOffset;

        private void Start()
        {
            followOffset = Vector3.zero + transform.position;
        }

        public void LateUpdate()
        {
            if (EndgameController.Instance.endgameStart) return;
            var desiredPosition = SwerveController.Instance.transform.position + followOffset;
            var followPosition = Vector3.Lerp(transform.position, desiredPosition, FollowSpeed);
            transform.position = followPosition;
        }
    }
}