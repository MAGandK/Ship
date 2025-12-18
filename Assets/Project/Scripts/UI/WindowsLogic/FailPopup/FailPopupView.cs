using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scripts.UI.WindowsLogic.FailPopup
{
    public sealed class FailPopupView : AbstractWindowView
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;

        public Button RestartButton => _restartButton;
        public Button ExitButton => _exitButton;

        public void SubscribeButton(UnityAction onRestartButtonClick,UnityAction onExitButtonClick)
        {
            _restartButton.onClick.AddListener(onRestartButtonClick);
            _exitButton.onClick.AddListener(onExitButtonClick);
        }
    }
}