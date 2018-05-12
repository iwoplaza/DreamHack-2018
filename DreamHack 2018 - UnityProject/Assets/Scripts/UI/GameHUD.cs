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
    }
}