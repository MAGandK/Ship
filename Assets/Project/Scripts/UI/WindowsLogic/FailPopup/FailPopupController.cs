using System;

namespace Scripts.UI.WindowsLogic.FailPopup
{
    public sealed class FailPopupController : AbstractPopupController<FailPopupView>
    {
        public event Action ExitClicked;
        public event Action RestartClicked;

        private FailPopupView _failPopupView;
        
        public FailPopupController(FailPopupView view) : base(view)
        {
            _failPopupView = view;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _failPopupView.RestartButton.onClick.AddListener(OnRestartButtonClick);
            _failPopupView.ExitButton.onClick.AddListener(OnExitButtonClick);
        }

        private void OnExitButtonClick()
        {
            _uiController?.CloseLastOpenPopup();
            ExitClicked?.Invoke();
        }

        private void OnRestartButtonClick()
        {
            if (_uiController != null)
                _uiController.CloseLastOpenPopup();

            RestartClicked?.Invoke();
        }
    }
}