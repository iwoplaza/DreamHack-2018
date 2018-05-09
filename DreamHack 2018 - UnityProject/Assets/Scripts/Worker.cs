using UnityEngine;
using Game.Tasks;
using Game.Pathfinding;

namespace Game
{
    [RequireComponent(typeof(CharacterController))]
    public class Worker : Living, IFocusTarget
    {
        [Header("Worker")]
        [SerializeField] protected string m_name;

        override public int MaxHealth { get { return 100; } }
        string IFocusTarget.DisplayName { get { return m_name; } }
        Vector3 IFocusTarget.Position { get { return transform.position; } }

        public TaskQueue TaskQueue { get; private set; }
        public PathfindingAgent PathfindingAgent { get; private set; }

        [SerializeField] private bool m_walking;
        [SerializeField] private float m_walkSpeed;
        [SerializeField] private float m_runSpeed;
        [SerializeField] private float m_stickToGroundForce;
        [SerializeField] private float m_gravityMultiplier;

        private TileMap m_tileMap;
        private CharacterController m_characterController;
        private Animator m_animator;
        private float m_yRotation;
        private Vector3 m_moveDir = Vector3.zero;
        private CollisionFlags m_collisionFlags;
        private bool m_previouslyGrounded;
        private bool m_running = false;

        public void Setup(TileMap tileMap)
        {
            m_tileMap = tileMap;
        }

        protected override void Awake()
        {
            base.Awake();
        }

        override protected void Start()
        {
            base.Start();

            PathfindingAgent = new PathfindingAgent(new BasicRule(), m_tileMap);
            PathfindingAgent.RegisterStatusChangeHandler(OnPathfindingStatusChanged);
            m_characterController = GetComponent<CharacterController>();
            m_animator = GetComponentInChildren<Animator>();
        }

        override protected void Update()
        {
            base.Update();

            if (!m_previouslyGrounded && m_characterController.isGrounded)
            {
                m_moveDir.y = 0f;
            }
            if (!m_characterController.isGrounded && m_previouslyGrounded)
            {
                m_moveDir.y = 0f;
            }

            m_previouslyGrounded = m_characterController.isGrounded;
        }

        private void FixedUpdate()
        {
            TilePosition nextPosition = PathfindingAgent.GetNextTile();

            if (PathfindingAgent.CurrentStatus == PathfindingStatus.HAS_PATH && nextPosition != null)
            {
                /// TODO Change this into Path Finding behaviour.
                Vector3 target = nextPosition.Vector3 + new Vector3(0.5F, 0, 0.5F);
                Vector3 direction = (target - transform.position);
                float sqrDistance = direction.sqrMagnitude;

                if (sqrDistance > 0.1F)
                {
                    direction.y = 0;
                    direction.Normalize();

                    float speed = m_running ? m_runSpeed : m_walkSpeed;

                    // get a normal for the surface that is being touched to move along it
                    RaycastHit hitInfo;
                    Physics.SphereCast(transform.position, m_characterController.radius, Vector3.down, out hitInfo,
                                       m_characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
                    direction = Vector3.ProjectOnPlane(direction, hitInfo.normal).normalized;

                    m_moveDir.x = direction.x * speed;
                    m_moveDir.z = direction.z * speed;
                    m_walking = true;

                    transform.rotation = Quaternion.Euler(0, Mathf.Atan2(m_moveDir.x, m_moveDir.z) / Mathf.PI * 180.0F, 0);
                }
                else
                {
                    CurrentTile = PathfindingAgent.PopTile();
                }
            }
            else
            {
                m_walking = false;
                m_moveDir.x = 0;
                m_moveDir.z = 0;
            }            

            if (m_characterController.isGrounded)
            {
                m_moveDir.y = -m_stickToGroundForce;
            }
            else
            {
                m_moveDir += Physics.gravity * m_gravityMultiplier * Time.fixedDeltaTime;
            }
            m_collisionFlags = m_characterController.Move(m_moveDir * Time.fixedDeltaTime);

            UpdateAnimator();
        }

        void HandleTasks()
        {
            TaskBase task = TaskQueue.CurrentTask;
            if (task != null)
            {
                if(TaskQueue.Status == TaskQueue.QueueStatus.IN_PROGRESS)
                {
                    if(task is GoToTask)
                    {
                        GoToTask goToTask = task as GoToTask;
                        MoveTo(goToTask.TargetPosition);
                    }
                    else
                    {
                        TaskQueue.CompleteTask();
                    }
                }
            }
        }

        public void UpdateAnimator()
        {
            m_animator.SetBool("Walk", m_walking);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_collisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_characterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }

        public void MoveTo(TilePosition target)
        {
            PathfindingAgent.GeneratePath(CurrentTile, target);
        }

        /// <summary>
        /// An event handler for the StatusChanged event of the PathfindingAgent.
        /// </summary>
        /// <param name="newStatus">The new status</param>
        public void OnPathfindingStatusChanged(PathfindingStatus newStatus)
        {
            if (newStatus == PathfindingStatus.PATH_FINISHED && TaskQueue.Status == TaskQueue.QueueStatus.IN_PROGRESS)
            {
                GoToTask goToTask = TaskQueue.CurrentTask as GoToTask;
                if (goToTask != null)
                {
                    TaskQueue.CompleteTask();
                }
            }
        }

        void IFocusTarget.OnFocusGained()
        {
        }

        void IFocusTarget.OnFocusLost()
        {
        }
    }
}