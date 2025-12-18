using System;

namespace Scripts.Game
{
    public sealed class KillCounter
    {
        public event Action<int> OnKillCountChanged;
        private int _killCount;

        public void AddKill()
        {
            _killCount++;
            OnKillCountChanged?.Invoke(_killCount);
        }
    }
}