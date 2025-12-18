using UnityEngine;

namespace Scripts.Bonus
{
    public sealed class BonusMove : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private Rigidbody _rb;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _rb.linearVelocity = -Vector3.forward * _speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            var receiver = other.GetComponentInParent<IBonusReceiver>();

            if (receiver == null)
            {
                return;
            }

            receiver.ReceiveBonus();
            Destroy(gameObject);
        }
    }
}