using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.PopUp
{
    public abstract class PopUpWindow : MonoBehaviour
    {
        [SerializeField] protected Button m_closeButton;
        protected GameHUD m_gameHud;

        public abstract bool IsSingluar { get; }

        void Start()
        {
            m_gameHud = GetComponentInParent<GameHUD>();
            if(m_gameHud == null)
            {
                Debug.LogError("Tried to create a PopUp window that isn't a part of the GameHUD.");
                Destroy(gameObject);
                return;
            }

            if(!m_gameHud.OnPopUpOpened(this))
            {
                Destroy(gameObject);
                return;
            }
        }

        public virtual void CloseWindow() {
            m_gameHud.OnPopUpClosed(this);
            Destroy(gameObject);
        }
    }
}