using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TimePanel : MonoBehaviour
    {
        [SerializeField] Text m_timeText;
        [SerializeField] Text m_dayText;

        void Update()
        {
            m_timeText.text = WorldController.Instance.MainState.TimeSystem.TimeString;
            m_dayText.text = "Day " + WorldController.Instance.MainState.TimeSystem.DayCount.ToString();
        }
    }
}