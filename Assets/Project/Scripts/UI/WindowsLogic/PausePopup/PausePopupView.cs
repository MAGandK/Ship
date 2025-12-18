using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.WindowsLogic.PausePopup
{
    public class PausePopupView : AbstractWindowView
    {
        [SerializeField] private Toggle _muteMusic;
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _exitButton;
        
        public Toggle MuteMusic => _muteMusic;
        public Button ContinueButton => _continueButton;
        public Button ExitButton => _exitButton;
        public Slider VolumeSlider => _volumeSlider;
    }
}