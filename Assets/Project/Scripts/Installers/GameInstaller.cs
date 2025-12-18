using Scripts.Game;
using Scripts.JoystickControls;
using Scripts.Player;
using Scripts.UI;
using Scripts.UI.UIControllers;
using Scripts.UI.WindowsLogic;
using Scripts.UI.WindowsLogic.FailPopup;
using Scripts.UI.WindowsLogic.GameWindow;
using Scripts.UI.WindowsLogic.PausePopup;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Scripts.Installers
{
    public sealed class GameInstaller : MonoInstaller
    {
        [SerializeField] private AudioMixerGroup _audioMixerGroup;
        private PlayerController _player;
        private FailPopupView _failPopupView;
        private GameWindowView _gameWindowView;
        private PausePopupView _pausePopupView;
        
        public override void InstallBindings()
        {
            Container.Bind<IJoystickController>().To<Joystick>().FromComponentInHierarchy().AsSingle().NonLazy();
            
            BindPlayer();
            BindUi();
            BindAudio();
            BindWindows();
            BindInit();
            BindKillCounter();

        }

        public void BindPlayer()
        {
            Container.BindInterfacesAndSelfTo<PlayerController>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
        }

        private void BindUi()
        {
            Container.Bind<IUIController>().To<UIController>().AsSingle().NonLazy();
        }

        private void BindWindows()
        {
            BindWindow<GameWindowController, GameWindowView>();
            BindWindow<FailPopupController, FailPopupView>();
            BindWindow<PausePopupController, PausePopupView>();
        }

        private void BindInit()
        {
            Container.Bind<UiInitializer>().AsSingle();
        }

        private void BindKillCounter()
        {
            Container.Bind<KillCounter>().AsSingle().NonLazy();
        }

        private void BindAudio()
        {
            Container.Bind<AudioMixerGroup>().FromInstance(_audioMixerGroup).AsSingle();
        }
        
        private void BindWindow<TController, TWindowView>()
            where TController : IWindowController
            where TWindowView : IWindowView
        {
            Container.Bind(typeof(IWindowController), typeof(IInitializable), typeof(TController))
                .To<TController>()
                .AsSingle()
                .NonLazy();

            Container.Bind<TWindowView>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
        }
    }
}