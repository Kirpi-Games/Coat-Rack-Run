using Akali.Common;
using Akali.Scripts.Managers.StateMachine;
using UnityEngine;

public class MovementZ : Singleton<MovementZ>
{
    [Range(5, 30)] public float platformSpeed = 5f;
    
    private void Awake()
    {
        GameStateManager.Instance.GameStatePlaying.onExecute += MoveZ;
    }

    private void MoveZ()
    {
        transform.Translate(Vector3.back * platformSpeed * Time.deltaTime);
    }
}
