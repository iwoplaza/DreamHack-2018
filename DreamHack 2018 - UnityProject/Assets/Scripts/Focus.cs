using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Focus
    {
        public delegate void FocusHandler(FocusTarget focusTarget);
        public FocusTarget Current { get; private set; }

        private FocusHandler m_focusGainHandlers;
        private FocusHandler m_focusLossHandlers;

        public void On(FocusTarget newTarget)
        {
            if (Current != null)
            {
                Current.OnFocusLost();
                if (m_focusLossHandlers != null)
                    m_focusLossHandlers(Current);
            }

            Current = newTarget;
            if (newTarget != null)
            {
                Current.OnFocusGained();
                if (m_focusGainHandlers != null)
                    m_focusGainHandlers(Current);
            }
        }

        public void RegisterEventHandler(EventType type, FocusHandler handler)
        {
            switch (type)
            {
                case EventType.FOCUS_GAIN:
                    m_focusGainHandlers += handler;
                    break;
                case EventType.FOCUS_LOSS:
                    m_focusLossHandlers += handler;
                    break;
            }
        }

        public enum EventType
        {
            FOCUS_GAIN, FOCUS_LOSS
        }
    }
}