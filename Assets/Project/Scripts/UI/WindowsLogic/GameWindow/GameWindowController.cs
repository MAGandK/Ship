using Scripts.Game;
using Scripts.Player;
using Scripts.UI.UIControllers;
using Scripts.UI.WindowsLogic.PausePopup;
using UnityEngine;
using Zenject;

namespace Scripts.UI.WindowsLogic.GameWindow
{
    public sealed class GameWindowController : AbstractWindowController<GameWindowView>
    {
        private GameWindowView _gameWindowView;
        private PlayerController _player;
        private KillCounter _killCounter;
        private IUIController _uiController;

        [Inject]
        private void Construct(IUIController uiController, PlayerController playerController, KillCounter killCounter)
        {
            _uiController = uiController;
            _player = playerController;
            _killCounter = killCounter;
        }

        public GameWindowController(GameWindowView view) : base(view)
        {
            _gameWindowView = view;
        }

        public override void Initialize()
        {
            base.Initialize();
            _gameWindowView.PauseButton.onClick.AddListener(OnPauseButtonClick);
            _player.OnTakeDamage += PlayerOnOnTakeDamage;
            _killCounter.OnKillCountChanged += OnKillCountChanged;
        }

        private void OnPauseButtonClick()
        {
            _uiController.ShowWindow<PausePopupController>();
            Time.timeScale = 0f;
        }

        private void OnKillCountChanged(int count)
        {
            _gameWindowView.SetEnemyCount(count);
        }

        private void PlayerOnOnTakeDamage()
        {
            _gameWindowView.UpdateHearts(_player.HealthPlayer);
        }
    }
}