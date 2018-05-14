﻿using Game.UI.PopUp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameHUD : MonoBehaviour
    {
        [SerializeField] CameraController m_cameraController;
        public CameraController CameraController { get { return m_cameraController; } }

        public FocusPanel FocusPanel { get; private set; }
        public TimePanel TimePanel { get; private set; }
        public WorkerList WorkerList { get; private set; }
        public ItemStoragePanel ItemStoragePanel { get; private set; }

        public List<PopUpWindow> OpenedPopUps { get; private set; }
        public int OpenedPausingCount { get; private set; }
        public TimeSystem.TimeMode SavedTimeMode { get; private set; }
        public bool HasInputFocus { get { return OpenedPopUps.Count > 0; } }

        void Awake()
        {
            FocusPanel = GetComponentInChildren<FocusPanel>();
            TimePanel = GetComponentInChildren<TimePanel>();
            WorkerList = GetComponentInChildren<WorkerList>();
            ItemStoragePanel = GetComponentInChildren<ItemStoragePanel>();

            OpenedPopUps = new List<PopUpWindow>();
        }

        /// <summary>
        /// Called by <see cref="ApplicationState"/>
        /// </summary>
        public void Setup(bool newWorld)
        {
            WorkerList.Setup(WorldController.Instance.MainState);
            FocusPanel.Setup();
            TimePanel.Setup();
            ItemStoragePanel.Setup();

            if(newWorld)
            {
                ControlsPopUp.Create(this).Open();
            }
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
            if (popUp.PausesGame)
            {
                if (OpenedPausingCount == 0)
                {
                    SavedTimeMode = WorldController.Instance.MainState.TimeSystem.CurrentMode;
                    WorldController.Instance.MainState.TimeSystem.CurrentMode = TimeSystem.TimeMode.PAUSE;
                }
                OpenedPausingCount++;
            }

            return true;
        }

        public void OnPopUpClosed(PopUpWindow popUp)
        {
            OpenedPopUps.Remove(popUp);
            if (popUp.PausesGame)
            {
                OpenedPausingCount--;
                if(OpenedPausingCount == 0)
                {
                    WorldController.Instance.MainState.TimeSystem.CurrentMode = SavedTimeMode;
                }
            }
        }

        public bool HandleInput()
        {
            if (HasInputFocus)
            {
                if (Input.GetMouseButton(0))
                {
                    for(int i = OpenedPopUps.Count -1; i >= 0 ; --i)
                    {
                        PopUpWindow window = OpenedPopUps[i];
                        if (!window.IsMouseOver && window.ShouldCloseOnFocusLost)
                        {
                            window.CloseWindow();
                            if(!window.IsOpen)
                                OpenedPopUps.Remove(window);
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}