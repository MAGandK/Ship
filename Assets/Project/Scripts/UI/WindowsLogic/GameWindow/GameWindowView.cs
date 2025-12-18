using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.WindowsLogic.GameWindow
{
    public sealed class GameWindowView : AbstractWindowView
    {
        [SerializeField] private TMP_Text _countEnemyText;
        [SerializeField] private GameObject[] _playerHearts;
        [SerializeField] private Button _buttonPause;
        
        public Button PauseButton => _buttonPause;
        public void SetEnemyCount(int count)
        {
            _countEnemyText.text = count.ToString();
        }

        public void UpdateHearts(int health)
        {
            for (int i = 0; i < _playerHearts.Length; i++)
            {
                var image = _playerHearts[i].GetComponent<Image>();

                if (image != null)
                {
                    image.enabled = i < health;
                }
            }
        }
    }
}