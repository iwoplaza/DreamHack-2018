using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.PopUp
{
    public class WorkerDetailsPopUp : PopUpWindow
    {
        public override bool IsSingluar { get { return true; } }

        public Text m_nameText;
        public Text m_ageText;

        public void Populate(Worker worker)
        {
            m_nameText.text = worker.DisplayName;
            m_ageText.text = "" + worker.Age;
        }

        public static WorkerDetailsPopUp Create(GameHUD gameHud)
        {
            GameObject prefab = Resources.PopUps.Find("WorkerDetails");
            if(prefab == null)
            {
                return null;
            }
            GameObject gameObject = Instantiate(prefab, gameHud.transform);
            WorkerDetailsPopUp popUp = gameObject.GetComponent<WorkerDetailsPopUp>();
            return popUp;
        }
    }
}