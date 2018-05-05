using Game.TileObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [ExecuteInEditMode]
    public class CameraController : MonoBehaviour
    {
        protected Camera m_camera;

        [SerializeField] protected float m_moveSpeedFactor = 1.0F;
        [SerializeField] protected float m_rotationSpeedFactor = 1.0F;
        [Header("Distance")]
        [SerializeField] protected float m_distance = 7.8F;
        [SerializeField] protected float m_minDistance = 5.0F;
        [SerializeField] protected float m_maxDistance = 16.0F;
        [SerializeField] protected float m_scrollSpeed = 2.0F;
        [SerializeField] protected float m_scrollAccellerationFactor = 0.5F;

        protected float m_lastDistance = 0.0F;

        Plane m_groundPlane = new Plane(Vector3.up, Vector3.zero);

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
        }

        void Update()
        {
            float horizontal = Input.GetAxis("Mouse X");
            float vertical = Input.GetAxis("Mouse Y");
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

            if(Input.GetMouseButtonDown(0))
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
                        if(!targetTile.HasObject)
                        {
                            targetTile.Install(new WallTileObject());
                        }
                        else
                        {
                            targetTile.UninstallObject();
                        }
                    }

                }
            }

            if (Input.GetMouseButton(1))
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
            }

            if(Input.GetMouseButton(2))
            {
                transform.Rotate(new Vector3(0, 1, 0), horizontal * m_rotationSpeedFactor, Space.World);
            }

            if (scrollWheel != 0 || m_lastDistance != m_distance)
            {
                m_distance = Mathf.Clamp(m_distance - scrollWheel * m_scrollSpeed * (1 + m_distance * m_scrollAccellerationFactor), m_minDistance, m_maxDistance);
                UpdateCameraPosition();
                m_lastDistance = m_distance;
            }
        }

        void UpdateCameraPosition()
        {
            Vector3 direction = new Vector3(0, 1, -1).normalized;
            m_camera.transform.localPosition = direction * m_distance;
        }
    }
}