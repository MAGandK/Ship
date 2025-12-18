using Scripts.Game;
using UnityEngine;

namespace Scripts.Boundary
{
    public sealed class DestroyExitObject : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IDestructible>(out var destructible))
            {
                destructible.Destroy();
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }
}
