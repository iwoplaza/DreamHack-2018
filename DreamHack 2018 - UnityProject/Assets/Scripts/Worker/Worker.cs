using UnityEngine;
using Game.Tasks;
using Game.Pathfinding;
using Game.Animation;
using Game.Acting;
using Game.Acting.Actions;

namespace Game
{
    [RequireComponent(typeof(CharacterController))]
    public class Worker : Living, IFocusTarget, IActor
    {
        [Header("Worker")]
        [SerializeField] protected string m_name;

        override public string DisplayName { get { return m_name; } }
        override public int MaxHealth { get { return 100; } }
        string IFocusTarget.DisplayName { get { return m_name; } }
        Vector3 IFocusTarget.Position { get { return transform.position; } }

        public TaskQueue TaskQueue { get; private set; }
        public PathfindingAgent PathfindingAgent { get; private set; }
        public WorkerAttackBehaviour AttackBehaviour { get; private set; }
        public WorkerVisual Visual { get; private set; }
        public bool IsWalking { get; private set; }
        public bool IsWeaponOut { get; private set; }

        [SerializeField] private float m_walkSpeed;
        [SerializeField] private float m_runSpeed;
        [SerializeField] private float m_stickToGroundForce;
        [SerializeField] private float m_gravityMultiplier;

        private TileMap m_tileMap;
        private CharacterController m_characterController;
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

            IsWeaponOut = false;
            TaskQueue = new TaskQueue();
            TaskQueue.RegisterHandler(TaskQueue.TaskEvent.CANCEL_TASK, OnTaskCancel);
        }

        override protected void Start()
        {
            base.Start();

            if (m_tileMap != null)
            {
                PathfindingAgent = new PathfindingAgent(new BasicRule(), m_tileMap);
                PathfindingAgent.RegisterStatusChangeHandler(OnPathfindingStatusChanged);
            }
            m_characterController = GetComponent<CharacterController>();
            Visual = GetComponent<WorkerVisual>();
            AttackBehaviour = GetComponent<WorkerAttackBehaviour>();
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

            PathfindingAgent.Update();
            HandleTasks();
        }

        private void FixedUpdate()
        {
            TimeSystem timeSystem = WorldController.Instance.MainState.TimeSystem;
            float timeMultiplier = Time.fixedDeltaTime * timeSystem.TimeMultiplier;
            TilePosition nextPosition = PathfindingAgent.GetNextTile();

            if (PathfindingAgent.CurrentStatus == PathfindingStatus.HAS_PATH && nextPosition != null)
            {
                /// TODO Change this into Path Finding behaviour.
                Vector3 target = nextPosition.Vector3 + new Vector3(0.5F, 0, 0.5F);
                Vector3 direction = (target - transform.position);
                direction.y = 0;
                float sqrDistance = direction.sqrMagnitude;

                if (sqrDistance > 0.1F)
                {
                    direction.Normalize();

                    float speed = m_running ? m_runSpeed : m_walkSpeed;

                    // get a normal for the surface that is being touched to move along it
                    RaycastHit hitInfo;
                    Physics.SphereCast(transform.position, m_characterController.radius, Vector3.down, out hitInfo,
                                       m_characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
                    direction = Vector3.ProjectOnPlane(direction, hitInfo.normal).normalized;

                    m_moveDir.x = direction.x * speed;
                    m_moveDir.z = direction.z * speed;
                    IsWalking = true;

                    transform.rotation = Quaternion.Euler(0, Mathf.Atan2(m_moveDir.x, m_moveDir.z) / Mathf.PI * 180.0F, 0);
                }
                else
                {
                    CurrentTile = PathfindingAgent.PopTile();
                }
            }
            else
            {
                IsWalking = false;
                m_moveDir.x = 0;
                m_moveDir.z = 0;
            }            

            if (m_characterController.isGrounded)
            {
                m_moveDir.y = -m_stickToGroundForce;
            }
            else
            {
                m_moveDir += Physics.gravity * m_gravityMultiplier * timeMultiplier;
            }
            m_collisionFlags = m_characterController.Move(m_moveDir * timeMultiplier);

            if(Visual != null)
                Visual.UpdateAnimator();
        }

        void HandleTasks()
        {
            TaskBase task = TaskQueue.CurrentTask;
            if (task != null)
            {
                if(TaskQueue.Status == TaskQueue.QueueStatus.WAITING)
                {
                    if(task is GoToTask)
                    {
                        GoToTask goToTask = task as GoToTask;
                        TaskQueue.StartTask();
                        MoveTo(goToTask.TargetPosition);
                    }
                    else if(task is AttackTask)
                    {
                        AttackTask attackTask = task as AttackTask;
                        TaskQueue.StartTask();
                        if(AttackBehaviour != null)
                            AttackBehaviour.Activate(attackTask);
                    }
                    else
                    {
                        TaskQueue.CompleteTask();
                    }
                }
                else
                {
                    task.OnUpdate();

                    if (task is AttackTask)
                    {
                        AttackTask attackTask = task as AttackTask;
                        if(attackTask.IsComplete)
                        {
                            TaskQueue.CompleteTask();
                            if (AttackBehaviour != null)
                                AttackBehaviour.Deactivate();
                        }
                    }
                }
            }
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

        public void TakeWeaponOut()
        {
            IsWeaponOut = true;
        }

        public void PutWeaponAway()
        {
            IsWeaponOut = false;
        }

        public void TurnTowards(Vector3 position)
        {
            Vector3 difference = position - transform.position;
            transform.rotation = Quaternion.Euler(0, Mathf.Atan2(difference.x, difference.z) / Mathf.PI * 180.0F, 0);
        }

        public void Shoot()
        {
            if(AttackBehaviour.CanShootYet)
            {
                if (Visual != null)
                    Visual.OnShoot();
            }
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

        public void OnTaskCancel(TaskBase task)
        {
            if(task == TaskQueue.CurrentTask)
            {
                if(task is GoToTask)
                {
                    PathfindingAgent.CancelPath();
                }
                else if (task is AttackTask)
                {
                    if (AttackBehaviour != null)
                        AttackBehaviour.Deactivate();
                }
            }
        }

        void IFocusTarget.OnFocusGained()
        {
        }

        void IFocusTarget.OnFocusLost()
        {
        }

        void IActor.PerformAction(ActionBase action, ISubject subject)
        {
            if(action is PerformTaskAction)
            {
                PerformTaskAction performTask = action as PerformTaskAction;
                TaskQueue.AddTask(performTask.TaskToPerform);
            }
        }
    }
}