using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class EnvironmentControlComponent : MonoBehaviour
    {
        bool m_initialized = false;

        void Update()
        {
            if(m_initialized)
            {
                UpdateComponent();
            }
        }

        public void Initialize()
        {
            Debug.Log(GetType().ToString() + " initialized.");
            InitializeComponent();
            m_initialized = true;
        }

        public abstract void InitializeComponent();
        public abstract void UpdateComponent();
    }
}
