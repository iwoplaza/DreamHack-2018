using Game.TileObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scene
{
    public class MainGeneratorComponent : MonoBehaviour, IFocusTarget
    {
        string IFocusTarget.DisplayName { get { return "Main Generator"; } }
        Vector3 IFocusTarget.Position { get { return transform.position; } }
        Transform IFocusTarget.PortraitPivot { get { return null; } }
        HealthComponent IFocusTarget.Health { get { return MainGeneratorTileObject.Health; } }
        bool IFocusTarget.IsDestroyed { get { return MainGeneratorTileObject.IsDestroyed; } }

        public MainGeneratorTileObject MainGeneratorTileObject { get; private set; }

        Focus m_focus;

        public void Setup(MainGeneratorTileObject mainGeneratorTileObject)
        {
            MainGeneratorTileObject = mainGeneratorTileObject;
            MainGeneratorTileObject.Health.RegisterChangeHandler(OnHealthChanged);
        }

        void IFocusTarget.OnFocusGained(Focus focus)
        {
            m_focus = focus;
        }

        void IFocusTarget.OnFocusLost(Focus focus)
        {
            m_focus = null;
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnHealthChanged(int previousPoints, int currentPoints)
        {
            if(previousPoints > 0 && currentPoints <= 0)
            {
                if (m_focus != null)
                    m_focus.On(null);
            }
        }
    }
}