using System;
using DG.Tweening;
using Scripts.Bonus;
using Scripts.Configs;
using Scripts.Game;
using Scripts.JoystickControls;
using UnityEngine;
using Zenject;

namespace Scripts.Player
{
    public sealed class PlayerController : MonoBehaviour, IInitializable, IDisposable, IDamageable, IPlayerController,
        IBonusReceiver
    {
        [SerializeField] private float _speedPlayer;
        [SerializeField] private int _healthPlayer;
        [SerializeField] private BoundaryConfig _boundary;
        [SerializeField] private PlayerShoots _playerShoots;
        [SerializeField] private Vector3 _startTransformPosition;
        [SerializeField] private float _swipeSensitivity = 0.01f;
        [SerializeField] private ShakeController _shakeController;
        private IJoystickController _joystickController;

        private Vector2 _joystickStartPosition;
        private Rigidbody _rbPlayer;
        private bool _isPointerDown = false;
        public int HealthPlayer => _healthPlayer;

        public event Action OnTakeDamage;
        public event Action OnDiedPlayer;

        [Inject]
        public void Construct(IJoystickController joystickController)
        {
            _joystickController = joystickController;
        }


        public void Initialize()
        {
            _rbPlayer = GetComponent<Rigidbody>();
            _joystickController.PointerDown += JoystickControllerOnPointerDown;
            _joystickController.PointerUp += JoystickControllerOnPointerUp;
        }

        private void JoystickControllerOnPointerDown()
        {
            _joystickStartPosition = _joystickController.Position;
            _startTransformPosition = transform.position;
            _isPointerDown = true;
        }

        private void JoystickControllerOnPointerUp()
        {
            _isPointerDown = false;
        }
        public void Dispose()
        {
            _joystickController.PointerDown -= JoystickControllerOnPointerDown;
            _joystickController.PointerUp -= JoystickControllerOnPointerUp;
        }

        private void FixedUpdate()
        {
            if (_isPointerDown)
            {
                Move();
            }

            _playerShoots.Shoot();
        }

        private void Move()
        {
            Vector2 delta = _joystickController.Position - _joystickStartPosition;

            float moveX = _startTransformPosition.x + delta.x * _swipeSensitivity;
            float moveZ = _startTransformPosition.z + delta.y * _swipeSensitivity;

            moveX = Mathf.Clamp(moveX, _boundary.XMin, _boundary.XMax);
            moveZ = Mathf.Clamp(moveZ, _boundary.ZMin, _boundary.ZMax);

            Vector3 targetPosition = new Vector3(moveX, _rbPlayer.position.y, moveZ);
            _rbPlayer.MovePosition(targetPosition);
        }

        public void TakeDamage(int amount = 1)
        {
            if (_healthPlayer > 0)
            {
                _healthPlayer -= amount;
                OnTakeDamage?.Invoke();
                _shakeController.Shake();
                Handheld.Vibrate();
            }

            if (_healthPlayer <= 0)
            {
                DiePlayer();
            }
        }

        private void DiePlayer()
        {
            DOTween.Kill(_shakeController.transform);  
            OnDiedPlayer?.Invoke();
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IDestructible>(out var destructible))
            {
                TakeDamage();
                destructible.Destroy();
            }
        }

        public void ReceiveBonus()
        {
            if (!_playerShoots.HasSpeedBonus)
            {
                _playerShoots.ApplySpeedBonus();
            }
            else if (!_playerShoots.HasDoubleBulletBonus)
            {
                _playerShoots.ApplyDoubleBulletBonus();
            }
        }
    }
}