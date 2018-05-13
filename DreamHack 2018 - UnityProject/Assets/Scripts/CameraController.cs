using Game.TileObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;
using Game.Tasks;
using Game.Acting;
using Game.Acting.Actions;
using Game.Scene;
using Game.Building;
using Game.Utility;
using Game.UI;

namespace Game
{
    public class CameraController : MonoBehaviour
    {
        public Camera Camera { get; private set; }

        [SerializeField] protected GameHUD m_gameHud;
        [SerializeField] protected float m_moveSpeedFactor = 1.0F;
        [SerializeField] protected float m_moveSpeedZoomInfluence = 2.0F;
        [SerializeField] protected float m_rotationSpeedFactor = 1.0F;
        [SerializeField] protected float m_cameraTurnIncrement = 45.0F;
        [SerializeField] protected float m_focusFollowSpeed = 0.7F;
        [Header("Distance")]
        [SerializeField] protected float m_distance = 9F;
        [SerializeField] protected float m_minDistance = 5.0F;
        [SerializeField] protected float m_maxDistance = 16.0F;
        [SerializeField] protected float m_defaultFocusDistance = 9F;
        [SerializeField] protected float m_scrollSpeed = 2.0F;
        [SerializeField] protected float m_scrollAccellerationFactor = 0.5F;

        protected float m_lastDistance = 0.0F;
        protected float m_targetDistance = 9F;
        protected Plane m_groundPlane = new Plane(Vector3.up, Vector3.zero);
        protected IFocusTarget m_focusTarget = null;

        protected float m_yOrientation = 0.0F;
        protected float m_nextYOrientation = 0.0F;
        protected float m_yRotationProgress = 1.0F;

        void Awake()
        {
            Camera = GetComponentInChildren<Camera>();
            if(Camera == null)
            {
                Debug.LogError("No camera was found as a part of the CameraController.");
                Destroy(gameObject);
            }

            m_targetDistance = m_distance;
        }

        void Start()
        {
            UpdateCameraPosition();

            if (WorldController.Instance != null)
            {
                WorldController.Instance.MainState.Focus.RegisterEventHandler(Focus.EventType.FOCUS_GAIN, OnFocusGained);
                WorldController.Instance.MainState.Focus.RegisterEventHandler(Focus.EventType.FOCUS_REGAIN, OnFocusRegained);
                WorldController.Instance.MainState.Focus.RegisterEventHandler(Focus.EventType.FOCUS_LOSS, OnFocusLost);

                if(WorldController.Instance.MainState.TileMap != null)
                {
                    TileMap tileMap = WorldController.Instance.MainState.TileMap;
                    transform.position = new Vector3(tileMap.Width / 2, 0, tileMap.Height / 2);
                }
            }
        }

        TilePosition GetTilePositionAtMouse()
        {
            //Create a ray from the Mouse click position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Initialise the enter variable
            float enter = 0.0f;

            if (m_groundPlane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                TilePosition tilePosition = TilePosition.FromWorldPosition(hitPoint);

                return tilePosition;
            }

            return null;
        }

        void Update()
        {
            float horizontal = CrossPlatformInputManager.GetAxis("Mouse X");
            float vertical = CrossPlatformInputManager.GetAxis("Mouse Y");
            float scrollWheel = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");

            TileMapComponent tileMapComponent = WorldController.Instance.MainState.TileMap.Component;
            tileMapComponent.UpdateViewpoint(transform.position);

            TilePosition tilePositionAtMouse = GetTilePositionAtMouse();
            if (tilePositionAtMouse != null && !EventSystem.current.IsPointerOverGameObject())
                tileMapComponent.HoverOver(tilePositionAtMouse);
            else
                tileMapComponent.HoverOver(null);


            if (!m_gameHud.HandleInput())
            {
                if (WorldController.Instance.Mode == PlayMode.BUILD_MODE)
                {
                    HandleBuildModeControls();
                }
                else
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            RaycastHit hitInfo;

                            if (Physics.Raycast(ray, out hitInfo))
                            {
                                if (hitInfo.collider != null)
                                {
                                    IFocusTarget focusTarget = hitInfo.collider.GetComponent<IFocusTarget>();
                                    WorldController.Instance.MainState.Focus.On(focusTarget);
                                }
                            }
                        }

                        if (Input.GetMouseButtonDown(1))
                        {
                            if (!HandleSubjectClick())
                            {
                                if (tilePositionAtMouse != null)
                                {
                                    Tile targetTile = WorldController.Instance.MainState.TileMap.TileAt(tilePositionAtMouse);
                                    if (targetTile != null)
                                    {
                                        Worker selectedWorker = WorldController.Instance.MainState.Focus.Current as Worker;
                                        if (selectedWorker != null)
                                        {
                                            selectedWorker.TaskQueue.AddTask(new GoToTask(tilePositionAtMouse));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (Input.GetMouseButton(2))
                {
                    Vector3 forward = transform.forward;
                    forward.y = 0;
                    forward.Normalize();
                    Vector3 right = transform.right;
                    right.y = 0;
                    right.Normalize();

                    Vector3 position = transform.position;
                    position += -right * horizontal * m_moveSpeedFactor * (1 + m_distance * m_moveSpeedZoomInfluence);
                    position += -forward * vertical * m_moveSpeedFactor * (1 + m_distance * m_moveSpeedZoomInfluence);
                    transform.position = position;

                    m_focusTarget = null;
                }

                if (CrossPlatformInputManager.GetButtonDown("Turn Camera Left"))
                {
                    RotateBy(m_cameraTurnIncrement);
                }

                if (CrossPlatformInputManager.GetButtonDown("Turn Camera Right"))
                {
                    RotateBy(-m_cameraTurnIncrement);
                }

                if (CrossPlatformInputManager.GetButtonDown("Build Mode Toggle"))
                {
                    WorldController.Instance.ToggleBuildMode();
                }
            }

            if(scrollWheel != 0)
            {
                m_targetDistance = Mathf.Clamp(m_targetDistance - scrollWheel * m_scrollSpeed * (1 + m_targetDistance * m_scrollAccellerationFactor), m_minDistance, m_maxDistance);
            }
            m_distance += (m_targetDistance - m_distance) * 0.3F;

            if (m_lastDistance != m_distance)
            {
                UpdateCameraPosition();
                m_lastDistance = m_distance;
            }

            if(m_yRotationProgress < 1.0F)
            {
                m_yRotationProgress += Time.deltaTime * m_rotationSpeedFactor;
                if (m_yRotationProgress > 1.0F)
                    m_yRotationProgress = 1.0F;
            }

            transform.rotation = Quaternion.Euler(0, CalculateCurrentYOrientation(), 0);

            if (m_focusTarget != null)
            {
                float t = Mathf.Min(m_focusFollowSpeed, 1);
                transform.position += (m_focusTarget.Position - transform.position) * t;
            }
        }

        bool HandleSubjectClick()
        {
            IActor actor = WorldController.Instance.MainState.Focus.Current as IActor;
            if (actor == null)
                return false;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo) &&
                hitInfo.collider != null)
            {
                ISubject subject = hitInfo.collider.GetComponent<ISubject>();
                if (subject != null)
                {
                    UI.PopUp.ActionSelectPopUp.Create(m_gameHud, subject, actor).Open();
                    /*List<ActionBase> actions = subject.GetActionsFor(actor);
                    if (actions.Count > 0)
                    {
                        actor.PerformAction(actions[0], subject);
                    }*/
                    return true;
                }
            }

            return false;
        }

        void HandleBuildModeControls()
        {
            BuildModeManager buildModeManager = WorldController.Instance.MainState.BuildModeManager;

            if (CrossPlatformInputManager.GetButtonDown("Rotate Prop Left"))
            {
                buildModeManager.RotatePropLeft();
            }

            if (CrossPlatformInputManager.GetButtonDown("Rotate Prop Right"))
            {
                buildModeManager.RotatePropRight();
            }

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                TilePosition tilePositionAtMouse = GetTilePositionAtMouse();
                if (tilePositionAtMouse != null)
                {
                    buildModeManager.SetCursorPosition(tilePositionAtMouse);

                    if (Input.GetMouseButton(0))
                    {
                        buildModeManager.Place();
                    }

                    if(Input.GetMouseButton(1))
                    {
                        Tile targetTile = WorldController.Instance.MainState.TileMap.TileAt(tilePositionAtMouse);
                        if (targetTile != null)
                        {
                            targetTile.Uninstall(PropType.OBJECT);
                            targetTile.Uninstall(PropType.FLOOR);
                        }
                    }
                }
            }
        }

        void UpdateCameraPosition()
        {
            Vector3 direction = new Vector3(0, 1, -1).normalized;
            Camera.transform.localPosition = direction * m_distance;
        }

        void RotateBy(float amount)
        {
            m_yOrientation = CalculateCurrentYOrientation();
            m_nextYOrientation += amount;
            m_yRotationProgress = 0.0F;
        }

        float CalculateCurrentYOrientation()
        {
            float t = 1 - Mathf.Pow(1 - m_yRotationProgress, 3);
            return Mathf.LerpAngle(m_yOrientation, m_nextYOrientation, t);
        }

        void OnFocusGained(IFocusTarget target)
        {
            m_focusTarget = target;
            m_targetDistance = m_defaultFocusDistance;
        }

        void OnFocusLost(IFocusTarget target)
        {
            m_focusTarget = null;
        }

        void OnFocusRegained(IFocusTarget target)
        {
            m_focusTarget = target;
            m_targetDistance = m_defaultFocusDistance;
        }
    }
}