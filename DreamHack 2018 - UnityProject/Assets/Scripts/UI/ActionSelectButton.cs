using Game.Acting.Actions;
using Game.UI.PopUp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ActionSelectButton : MonoBehaviour
    {
        Text m_text;

        public ActionSelectPopUp PopUp { get; private set; }
        public ActionBase Action { get; private set; }

        void Awake()
        {
            m_text = GetComponentInChildren<Text>();
        }

        public void Setup(ActionSelectPopUp popUp, ActionBase action)
        {
            PopUp = popUp;
            Action = action;

            m_text.text = action.DisplayName;
        }

        public void Choose()
        {
            if(PopUp != null && Action != null)
            {
                PopUp.Choose(Action);
            }
        }
    }
}