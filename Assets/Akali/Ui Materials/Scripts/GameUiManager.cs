using Akali.Common;
using Akali.Scripts.Managers.StateMachine;
using UnityEngine;

namespace Akali.Ui_Materials.Scripts
{
    public class GameUiManager : Singleton<GameUiManager>
    {
        public GameObject mainMenuTutorial;
        public GameObject playingLevel;
        public GameObject playingCoinBar;
        public GameObject background;
        public GameObject completeUi;
        public GameObject completeButton;
        public GameObject failUi;
        public GameObject failButton;
        
        private void Awake()
        {
            GameStateManager.Instance.GameStateMainMenu.onEnter += SetActiveMainMenuUi;
            GameStateManager.Instance.GameStateMainMenu.onExit += SetActiveMainMenuUi;
            GameStateManager.Instance.GameStatePlaying.onEnter += SetActivePlayingUi;
            GameStateManager.Instance.GameStatePlaying.onExit += SetActivePlayingUi;
            GameStateManager.Instance.GameStateComplete.onEnter += SetActiveCompleteUi;
            GameStateManager.Instance.GameStateComplete.onExit += SetActiveCompleteUi;
            GameStateManager.Instance.GameStateFail.onEnter += SetActiveFailUi;
            GameStateManager.Instance.GameStateFail.onExit += SetActiveFailUi;
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.GameStateMainMenu.onEnter -= SetActiveMainMenuUi;
            GameStateManager.Instance.GameStateMainMenu.onExit -= SetActiveMainMenuUi;
            GameStateManager.Instance.GameStatePlaying.onEnter -= SetActivePlayingUi;
            GameStateManager.Instance.GameStatePlaying.onExit -= SetActivePlayingUi;
            GameStateManager.Instance.GameStateComplete.onEnter -= SetActiveCompleteUi;
            GameStateManager.Instance.GameStateComplete.onExit -= SetActiveCompleteUi;
            GameStateManager.Instance.GameStateFail.onEnter -= SetActiveFailUi;
            GameStateManager.Instance.GameStateFail.onExit -= SetActiveFailUi;
        }
        
        public void SetActiveMainMenuUi()
        {
            mainMenuTutorial.SetActive(!mainMenuTutorial.activeSelf);
        }

        public void SetActivePlayingUi()
        {
            playingLevel.SetActive(!playingLevel.activeSelf);
            playingCoinBar.SetActive(!playingCoinBar.activeSelf);
        }

        private void SetActiveCompleteUi()
        {
            background.SetActive(!background.activeSelf);
            completeUi.SetActive(!completeUi.activeSelf);
            completeButton.SetActive(!completeButton.activeSelf);
        }

        private void SetActiveFailUi()
        {
            background.SetActive(!background.activeSelf);
            failUi.SetActive(!failUi.activeSelf);
            failButton.SetActive(!failButton.activeSelf);
        }
    }
}