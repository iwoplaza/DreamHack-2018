using Game.Acting;
using Game.Acting.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.PopUp
{
    public class ActionSelectPopUp : PopUpWindow
    {
        [SerializeField] GameObject m_buttonPrefab;
        [SerializeField] Transform m_actionsHolder;

        public override bool IsSingluar { get { return false; } }
        public override bool ShouldCloseOnFocusLost { get { return true; } }

        public ISubject Subject { get; private set; }
        public IActor Actor { get; private set; }
        public List<ActionSelectButton> ActionButtons { get; private set; }

        RectTransform m_rectTransform;

        void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
        }

        public void Populate(ISubject subject, IActor actor)
        {
            Subject = subject;
            Actor = actor;

            if (ActionButtons != null)
            {
                foreach(ActionSelectButton button in ActionButtons)
                {
                    Destroy(button.gameObject);
                }
            }

            ActionButtons = new List<ActionSelectButton>();

            List<ActionBase> actions = Subject.GetActionsFor(actor);
            foreach(ActionBase action in actions)
            {
                GameObject gameObject = Instantiate(m_buttonPrefab, m_actionsHolder);
                ActionSelectButton button = gameObject.GetComponent<ActionSelectButton>();
                button.Setup(this, action);
                ActionButtons.Add(button);
            }
            if(ActionButtons.Count <= 0)
            {
                CloseWindow();
                return;
            }
        }

        public override PopUpWindow Open()
        {
            if (base.Open() != null)
            {
                UpdateView();
                return this;
            }
            return null;
        }

        public static ActionSelectPopUp Create(GameHUD gameHud, ISubject subject, IActor actor)
        {
            GameObject prefab = Resources.PopUps.Find("ActionSelect");
            if (prefab == null)
            {
                return null;
            }
            GameObject gameObject = Instantiate(prefab, gameHud.transform);
            ActionSelectPopUp popUp = gameObject.GetComponent<ActionSelectPopUp>();
            popUp.Populate(subject, actor);
            return popUp;
        }

        void Start()
        {
            UpdateView();
        }

        protected override void Update()
        {
            UpdateView();
        }

        void UpdateView()
        {
            if (!IsOpen)
                return;

            CameraController cameraController = m_gameHud.CameraController;
            if (cameraController != null)
            {
                m_rectTransform.position = cameraController.Camera.WorldToScreenPoint(Subject.Position) + new Vector3(0, -20.0F);
            }
        }

        public void Choose(ActionBase action)
        {
            if(Actor != null)
            {
                Actor.PerformAction(action, Subject);
                CloseWindow();
            }
        }
    }
}