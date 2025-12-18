using Scripts.Asteroid;
using UnityEngine;

namespace Scripts.Bonus
{
    public sealed class BonusController : AsteroidController
    {
        [SerializeField] private GameObject _powerUpBonus;

        public override void Destroy()
        {
            TryActivateBonus();
            base.Destroy();
        }

        private void TryActivateBonus()
        {
            if (_powerUpBonus == null)
            {
                return;
            }

            var bonusInstance = Instantiate(_powerUpBonus, transform.position, Quaternion.identity);
            bonusInstance.SetActive(true);
        }
    }
}