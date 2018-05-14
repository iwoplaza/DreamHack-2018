using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IFocusTarget
    {
        string DisplayName { get; }
        Vector3 Position { get; }
        Transform PortraitPivot { get; }
        HealthComponent Health { get; }
        bool IsDestroyed { get; }

        void OnFocusGained(Focus focus);
        void OnFocusLost(Focus focus);
    }
}