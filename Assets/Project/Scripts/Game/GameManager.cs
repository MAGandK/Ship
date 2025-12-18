using Scripts.Player;
using Scripts.UI.UIControllers;
using Scripts.UI.WindowsLogic.FailPopup;
using Scripts.UI.WindowsLogic.PausePopup;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Scripts.Game
{
    public sealed class GameManager : MonoBehaviour
    {
        private IUIController _uiController;
        private FailPopupController _failPopupController;
        private PausePopupController _pausePopupController;
        private PlayerController _playerController;

        [Inject]
        public void Construct(PlayerController playerController, IUIController uiController,
            FailPopupController failPopupController, PausePopupController pausePopupController)
        {
            _playerController = playerController;
            _uiController = uiController;
            _failPopupController = failPopupController;
            _pausePopupController = pausePopupController;
        }


        private void OnEnable()
        {
            _playerController.OnDiedPlayer += PlayerControllerOnDiedPlayer;

            _failPopupController.RestartClicked += RestartLevel;
            _failPopupController.ExitClicked += ExitGame;
            _pausePopupController.ExitClicked += ExitGame;
        }
        
        private void OnDisable()
        {
            _playerController.OnDiedPlayer -= PlayerControllerOnDiedPlayer;
            
            _failPopupController.RestartClicked -= RestartLevel;
            _failPopupController.ExitClicked -= ExitGame;
            _pausePopupController.ExitClicked += ExitGame;
        }

        private void PlayerControllerOnDiedPlayer()
        {
            Time.timeScale = 0;
            _uiController.ShowWindow<FailPopupController>();
        }

        public void RestartLevel()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void ExitGame()
        {
            Time.timeScale = 1;
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}