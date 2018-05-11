using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IAttackable
    {
        string DisplayName { get; }
        HealthComponent Health { get; }
        Vector3 Position { get; }
        GameObject GameObject { get; }
        bool IsDestroyed { get; }

        void Damage(int damage, GameObject attacker);
    }
}