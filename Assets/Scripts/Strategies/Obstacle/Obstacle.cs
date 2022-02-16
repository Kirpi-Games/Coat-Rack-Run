using Akali.Scripts.Managers.StateMachine;
using UnityEngine;

namespace Strategies.Obstacle
{
    public enum ObstacleTypes : byte
    {
        Fixed,
        Rotating,
    }
    public class Obstacle : MonoBehaviour
    {
        public ObstacleTypes type;
        private const float Angle = 180;
        
        private void Start()
        {
            if (type is ObstacleTypes.Rotating)
                GameStateManager.Instance.GameStatePlaying.onExecute += Rotate;
        }
        
        private void Rotate() => transform.Rotate(Vector3.up, -Angle * Time.deltaTime, Space.Self);
    }
}
