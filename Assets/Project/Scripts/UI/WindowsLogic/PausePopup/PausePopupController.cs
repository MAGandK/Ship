using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Scripts.UI.WindowsLogic.PausePopup
{
    public class PausePopupController : AbstractPopupController<PausePopupView>
    {
        private PausePopupView _view;
        private AudioMixerGroup _mixerGroup;
        public event Action ExitClicked;
        public PausePopupController(PausePopupView view, AudioMixerGroup audioMixerGroup) : base(view)
        {
            _view = view;
            _mixerGroup = audioMixerGroup;
        }

        public override void Initialize()
        {
            base.Initialize();

            _view.MuteMusic.onValueChanged.AddListener(OnMuteMusicChanged);
            _view.ExitButton.onClick.AddListener(OnExitButtonClick);
            _view.ContinueButton.onClick.AddListener(OnContinueButtonClick);
            _view.VolumeSlider.onValueChanged.AddListener(ChangeVolume);
            InitializeAudioControls();
        }
        
        protected override void OnShow()
        {
            base.OnShow();
            _view.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        
        protected override void OnHide()
        {
            _view.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }

        private void InitializeAudioControls()
        {
            float currentVolume;
            _mixerGroup.audioMixer.GetFloat("MasterVolume", out currentVolume);
            _view.VolumeSlider.value = Mathf.InverseLerp(-80f, 0f, currentVolume);

            _view.MuteMusic.isOn = currentVolume > -80f;
        }

        private void OnMuteMusicChanged(bool isMute)
        {
            ToggleMusic(!isMute);
        }

        private void OnContinueButtonClick()
        {
            _uiController?.CloseLastOpenPopup();
            OnHide();
        }

        private void OnExitButtonClick()
        {
            ExitClicked?.Invoke();
        }

        public void ToggleMusic(bool enabled)
        {
            if (enabled)
            {
                _mixerGroup.audioMixer.SetFloat("MusicVolume", 0);
            }
            else
            {
                _mixerGroup.audioMixer.SetFloat("MusicVolume", -80);
            }
        }

        public void ChangeVolume(float volume)
        {
            _mixerGroup.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-80, 0, volume));
            _view.MuteMusic.isOn = volume > 0f;
        }
    }
}