using Game.UI.PopUp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameHUD : MonoBehaviour
    {
        public FocusPanel FocusPanel { get; private set; }
        public TimePanel TimePanel { get; private set; }

        public List<PopUpWindow> OpenedPopUps { get; private set; }
        public bool HasInputFocus { get { return OpenedPopUps.Count > 0; } }

        void Awake()
        {
            FocusPanel = GetComponentInChildren<FocusPanel>();
            TimePanel = GetComponentInChildren<TimePanel>();

            OpenedPopUps = new List<PopUpWindow>();
        }

        public bool DoesPopUpOfTypeExist(Type type)
        {
            foreach (PopUpWindow window in OpenedPopUps)
            {
                if(window.GetType() == type)
                {
                    return true;
                }
            }

            return false;
        }

        public bool OnPopUpOpened(PopUpWindow popUp)
        {
            if(popUp.IsSingluar)
            {
                if(DoesPopUpOfTypeExist(popUp.GetType()))
                {
                    return false;
                }
            }

            OpenedPopUps.Add(popUp);

            return true;
        }

        public void OnPopUpClosed(PopUpWindow popUp)
        {
            OpenedPopUps.Remove(popUp);
        }

        public bool HandleInput()
        {
            if (HasInputFocus)
            {
                if (Input.GetMouseButton(0))
                {
                    foreach (PopUpWindow window in OpenedPopUps)
                    {
                        if (window.IsMouseOver && window.ShouldCloseOnFocusLost)
                        {
                            window.CloseWindow();
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}