using UnityEngine;

namespace Scripts.Configs
{
    [CreateAssetMenu(fileName = "BoundaryConfig", menuName = "Configs/BoundaryConfig")]
    public sealed class BoundaryConfig : ScriptableObject
    {
        [SerializeField] private float xMin;
        [SerializeField] private float xMax;
        [SerializeField] private float zMin;
        [SerializeField] private float zMax;

        public float XMin => xMin;
        public float XMax => xMax;
        public float ZMin => zMin;
        public float ZMax => zMax;
    }
}