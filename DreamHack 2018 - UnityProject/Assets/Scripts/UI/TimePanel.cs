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
        [SerializeField]
        Button m_pauseButton;
        [SerializeField]
        Button m_normalButton;
        [SerializeField]
        Button m_2xButton;
        [SerializeField]
        Button m_4xButton;

        [Header("Default colors")]
        public ColorBlock m_defaultColors;
        [Header("Selected colors")]
        public ColorBlock m_selectedColors;

        void Start()
        {
            ResetColours();

            TimeSystem timeSystem = WorldController.Instance.MainState.TimeSystem;
            if (timeSystem != null)
            {
                Button button = GetButtonForMode(timeSystem.CurrentMode);
                if (button != null)
                    button.colors = m_selectedColors;
            }
        }

        void Update()
        {
            m_timeText.text = WorldController.Instance.MainState.TimeSystem.TimeString;
            m_dayText.text = "Day " + WorldController.Instance.MainState.TimeSystem.DayCount.ToString();
        }

        void ChooseMode(TimeSystem.TimeMode mode)
        {
            TimeSystem timeSystem = WorldController.Instance.MainState.TimeSystem;
            if (timeSystem != null)
                timeSystem.CurrentMode = mode;
        }

        Button GetButtonForMode(TimeSystem.TimeMode mode)
        {
            switch (mode)
            {
                case TimeSystem.TimeMode.PAUSE:
                    return m_pauseButton;
                case TimeSystem.TimeMode.NORMAL:
                    return m_normalButton;
                case TimeSystem.TimeMode.MULTIPLIER_2X:
                    return m_2xButton;
                case TimeSystem.TimeMode.MULTIPLIER_4X:
                    return m_4xButton;
            }
            return null;
        }

        void ResetColours(Button button)
        {
            button.colors = m_defaultColors;
        }

        void ResetColours()
        {
            ResetColours(m_pauseButton);
            ResetColours(m_normalButton);
            ResetColours(m_2xButton);
            ResetColours(m_4xButton);
        }

        public void ChooseNormalMode()
        {
            ChooseMode(TimeSystem.TimeMode.NORMAL);
            ResetColours();
            m_normalButton.colors = m_selectedColors;
        }

        public void Pause()
        {
            ChooseMode(TimeSystem.TimeMode.PAUSE);
            ResetColours();
            m_pauseButton.colors = m_selectedColors;
        }

        public void Choose2xMode()
        {
            ChooseMode(TimeSystem.TimeMode.MULTIPLIER_2X);
            ResetColours();
            m_2xButton.colors = m_selectedColors;
        }

        public void Choose4xMode()
        {
            ChooseMode(TimeSystem.TimeMode.MULTIPLIER_4X);
            ResetColours();
            m_4xButton.colors = m_selectedColors;
        }
    }
}