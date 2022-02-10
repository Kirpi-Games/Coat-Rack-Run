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

        public void LevelIsPlaying()
        {
            GameStateManager.Instance.SetGameState(GameStateManager.Instance.GameStatePlaying);
            iOSHapticController.instance.TriggerImpactLight();
            //TinySauce.OnGameStarted(PlayerPrefs.GetLevelText().ToString());
        }

        public void LevelIsCompleted()
        {
            GameStateManager.Instance.SetGameState(GameStateManager.Instance.GameStateComplete);
            iOSHapticController.instance.TriggerNotificationSuccess();
            //TinySauce.OnGameFinished(true, PlayerPrefs.GetMoney(), PlayerPrefs.GetLevelText().ToString());
        }

        public void LevelIsFail()
        {
            GameStateManager.Instance.SetGameState(GameStateManager.Instance.GameStateFail);
            iOSHapticController.instance.TriggerNotificationError();
            //TinySauce.OnGameFinished(false, PlayerPrefs.GetMoney(), PlayerPrefs.GetLevelText().ToString());
        }
    }
}