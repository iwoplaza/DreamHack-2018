using Game.TileObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;

namespace Game
{
    public class CameraController : MonoBehaviour
    {
        protected Camera m_camera;

        [SerializeField] protected float m_moveSpeedFactor = 1.0F;
        [SerializeField] protected float m_rotationSpeedFactor = 1.0F;
        [SerializeField] protected float m_cameraTurnIncrement = 45.0F;
        [SerializeField] protected float m_focusFollowSpeed = 0.7F;
        [Header("Distance")]
        [SerializeField] protected float m_distance = 7.8F;
        [SerializeField] protected float m_minDistance = 5.0F;
        [SerializeField] protected float m_maxDistance = 16.0F;
        [SerializeField] protected float m_scrollSpeed = 2.0F;
        [SerializeField] protected float m_scrollAccellerationFactor = 0.5F;

        protected float m_lastDistance = 0.0F;
        protected Plane m_groundPlane = new Plane(Vector3.up, Vector3.zero);
        protected FocusTarget m_focusTarget = null;

        protected float m_yOrientation = 0.0F;
        protected float m_nextYOrientation = 0.0F;
        protected float m_yRotationProgress = 1.0F;

        void Awake()
        {
            m_camera = GetComponentInChildren<Camera>();
            if(m_camera == null)
            {
                Debug.LogError("No camera was found as a part of the CameraController.");
                Destroy(gameObject);
            }
        }

        void Start()
        {
            UpdateCameraPosition();

            if (WorldController.Instance != null)
            {
                WorldController.Instance.MainState.Focus.RegisterEventHandler(Focus.EventType.FOCUS_GAIN, OnFocusGained);
                WorldController.Instance.MainState.Focus.RegisterEventHandler(Focus.EventType.FOCUS_LOSS, OnFocusLost);
            }
        }

        void Update()
        {
            float horizontal = CrossPlatformInputManager.GetAxis("Mouse X");
            float vertical = CrossPlatformInputManager.GetAxis("Mouse Y");
            float scrollWheel = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");

            if(!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if(Physics.Raycast(ray, out hitInfo))
                {
                    if(hitInfo.collider != null)
                    {
                        FocusTarget focusTarget = hitInfo.collider.GetComponent<FocusTarget>();
                        WorldController.Instance.MainState.Focus.On(focusTarget);
                    }
                }
            }

            if(Input.GetMouseButtonDown(1))
            {
                //Create a ray from the Mouse click position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Initialise the enter variable
                float enter = 0.0f;

                if (m_groundPlane.Raycast(ray, out enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);
                    TilePosition tilePosition = TilePosition.FromWorldPosition(hitPoint);
                    Tile targetTile = WorldController.Instance.MainState.TileMap.TileAt(tilePosition);
                    if(targetTile != null)
                    {
                        if (!targetTile.HasObject)
                        {
                            Worker selectedWorker = WorldController.Instance.MainState.Focus.Current as Worker;
                            if(selectedWorker != null)
                            {
                                selectedWorker.MoveTo(tilePosition);
                            }
                        }

                        /*if(!targetTile.HasObject)
                        {
                            targetTile.Install(new WallTileObject());
                        }
                        else
                        {
                            targetTile.UninstallObject();
                        }*/
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
                position += -right * horizontal * m_moveSpeedFactor;
                position += -forward * vertical * m_moveSpeedFactor;
                transform.position = position;

                m_focusTarget = null;
            }

            if(CrossPlatformInputManager.GetButtonDown("Turn Camera Left"))
            {
                RotateBy(m_cameraTurnIncrement);
            }

            if (CrossPlatformInputManager.GetButtonDown("Turn Camera Right"))
            {
                RotateBy(-m_cameraTurnIncrement);
            }

            if (scrollWheel != 0 || m_lastDistance != m_distance)
            {
                m_distance = Mathf.Clamp(m_distance - scrollWheel * m_scrollSpeed * (1 + m_distance * m_scrollAccellerationFactor), m_minDistance, m_maxDistance);
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
                Vector3 difference = m_focusTarget.transform.position - transform.position;
                float t = Mathf.Min(Mathf.Pow(m_focusFollowSpeed, Time.deltaTime * 100), 1);
                transform.position += difference * (t);
            }
        }

        void UpdateCameraPosition()
        {
            Vector3 direction = new Vector3(0, 1, -1).normalized;
            m_camera.transform.localPosition = direction * m_distance;
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

        void OnFocusGained(FocusTarget target)
        {
            m_focusTarget = target;
        }

        void OnFocusLost(FocusTarget target)
        {
            m_focusTarget = null;
        }
    }
}