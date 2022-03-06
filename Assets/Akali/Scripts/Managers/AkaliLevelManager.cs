using System;
using Akali.Common;
using Akali.Scripts.Managers.StateMachine;
using Akali.Scripts.ScriptableObjects;
using PlayerPrefs = Akali.Scripts.Utilities.PlayerPrefs;

namespace Akali.Scripts.Managers
{
    public class AkaliLevelManager : Singleton<AkaliLevelManager>
    {
        public LevelListScriptableObject levels;
        public LevelScriptableObject CurrentLevel => levels.GetCurrentLevel(PlayerPrefs.GetLevel());

        private void Awake()
        {
            Instantiate(CurrentLevel.levelGameObject, MovementZ.Instance.transform);
        }

        public void LevelIsPlaying()
        {
            GameStateManager.Instance.SetGameState(GameStateManager.Instance.GameStatePlaying);
            Taptic.Light();
            //TinySauce.OnGameStarted(PlayerPrefs.GetLevelText().ToString());
        }

        public void LevelIsCompleted()
        {
            GameStateManager.Instance.SetGameState(GameStateManager.Instance.GameStateComplete);
            Taptic.Success();
            //TinySauce.OnGameFinished(true, PlayerPrefs.GetMoney(), PlayerPrefs.GetLevelText().ToString());
        }

        public void LevelIsFail()
        {
            GameStateManager.Instance.SetGameState(GameStateManager.Instance.GameStateFail);
            Taptic.Failure();
            //TinySauce.OnGameFinished(false, PlayerPrefs.GetMoney(), PlayerPrefs.GetLevelText().ToString());
        }
    }
}