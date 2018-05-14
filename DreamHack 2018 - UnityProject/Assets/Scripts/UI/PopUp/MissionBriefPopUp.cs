using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.PopUp
{
    public class MissionBriefPopUp : PopUpWindow
    {
        public override bool IsSingluar { get { return true; } }
        public override bool ShouldCloseOnFocusLost { get { return false; } }
        public override bool PausesGame { get { return true; } }

        public static MissionBriefPopUp Create(GameHUD gameHud)
        {
            GameObject prefab = Resources.PopUps.Find("MissionBrief");
            if (prefab == null)
            {
                return null;
            }
            GameObject gameObject = Instantiate(prefab, gameHud.transform);
            MissionBriefPopUp popUp = gameObject.GetComponent<MissionBriefPopUp>();
            return popUp;
        }

        public void Controls()
        {
            ControlsPopUp.Create(m_gameHud).Open();
        }

        override protected void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseWindow();
            }
        }
    }
}