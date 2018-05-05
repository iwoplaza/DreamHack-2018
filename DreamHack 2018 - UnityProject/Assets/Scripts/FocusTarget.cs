using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class FocusTarget : MonoBehaviour
    {
        public abstract string DisplayName { get; }

        public bool FocusedOn { get; private set; }

        protected virtual void Awake()
        {
            FocusedOn = false;
        }

        public virtual void OnFocusGained()
        {
            FocusedOn = true;
        }

        public virtual void OnFocusLost()
        {
            FocusedOn = false;
        }
    }
}