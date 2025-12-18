using DG.Tweening;
using UnityEngine;

namespace Scripts.Player
{
    public sealed class ShakeController : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] private float _strength = 0.25f;
        [SerializeField] private int _vibrato = 20;
        private Tween _shakeTween;

        public void Shake()
        {
            _shakeTween?.Kill();
            _shakeTween = transform.DOShakePosition(
                duration: _duration,
                strength: _strength,
                vibrato: _vibrato,
                randomness: 90,
                snapping: false,
                fadeOut: true);
        }
    }
}