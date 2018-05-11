﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Focus
    {
        public delegate void FocusHandler(IFocusTarget focusTarget);
        public IFocusTarget Current { get; private set; }
        public Camera PortraitCamera { get; private set; }
        public RenderTexture PortraitTexture { get; private set; }

        private FocusHandler m_focusGainHandlers;
        private FocusHandler m_focusLossHandlers;

        public Focus()
        {
            GameObject gameObject = new GameObject("PortraitCamera");
            PortraitCamera = gameObject.AddComponent<Camera>();
            PortraitTexture = new RenderTexture(256, 256, 24);
            PortraitCamera.targetTexture = PortraitTexture;
            PortraitCamera.fieldOfView = 22.0F;
        }

        public void On(IFocusTarget newTarget)
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

                if (newTarget.PortraitPivot != null)
                {
                    PortraitCamera.transform.parent = newTarget.PortraitPivot;
                    PortraitCamera.transform.localRotation = Quaternion.Euler(0, -150, 0);
                    PortraitCamera.transform.localPosition = new Vector3(0.553F, 0.661F, 0.969F);
                }

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