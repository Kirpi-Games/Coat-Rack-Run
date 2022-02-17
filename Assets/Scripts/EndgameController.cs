using System;
using Akali.Common;
using Akali.Scripts.Managers;
using UnityEngine;

public class EndgameController : Singleton<EndgameController>
{
    private void OnTriggerEnter(Collider other)
    {
        AkaliLevelManager.Instance.LevelIsCompleted();
    }
}
