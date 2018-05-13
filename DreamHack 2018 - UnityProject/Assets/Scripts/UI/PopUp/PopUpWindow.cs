using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.PopUp
{
    public abstract class PopUpWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected Button m_closeButton;
        protected GameHUD m_gameHud;

        public abstract bool IsSingluar { get; }
        public abstract bool ShouldCloseOnFocusLost { get; }

        public bool IsMouseOver { get; set; }
        public bool IsOpen { get; private set; }

        public virtual PopUpWindow Open()
        {
            m_gameHud = GetComponentInParent<GameHUD>();
            if (m_gameHud == null)
            {
                Debug.LogError("Tried to create a PopUp window that isn't a part of the GameHUD.");
                Destroy(gameObject);
                return null;
            }

            if (!m_gameHud.OnPopUpOpened(this))
            {
                Destroy(gameObject);
                return null;
            }

            IsOpen = true;
            return this;
        }

        protected virtual void Update() {}

        public virtual void CloseWindow() {
            m_gameHud.OnPopUpClosed(this);
            Destroy(gameObject);
            IsOpen = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Mouse enter");
            IsMouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Mouse exit");
            IsMouseOver = false;
        }
    }
}