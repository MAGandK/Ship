using UnityEngine;

namespace Scripts.Background
{
    public sealed class BackgroundScroller : MonoBehaviour
    {
        [SerializeField] private float _scrollSpeed;
        [SerializeField] private float _titleSizeY;

        private Vector3 _startPosition;

        private void Start()
        {
            _startPosition = transform.position;
        }

        private void Update()
        {
            var newPosition = Mathf.Repeat(Time.time * _scrollSpeed, _titleSizeY);
            transform.position = _startPosition + Vector3.forward * newPosition;
        }
    }
}