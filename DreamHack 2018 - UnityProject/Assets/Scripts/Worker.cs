using Game.Tasks;
using UnityEngine;

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

        [SerializeField] private bool m_walking;
        [SerializeField] private float m_walkSpeed;
        [SerializeField] private float m_runSpeed;
        [SerializeField] private float m_stickToGroundForce;
        [SerializeField] private float m_gravityMultiplier;
        
        private float m_yRotation;
        private Vector3 m_moveDir = Vector3.zero;
        private CharacterController m_characterController;
        private CollisionFlags m_collisionFlags;
        private bool m_previouslyGrounded;
        private bool m_running = false;

        protected override void Awake()
        {
            base.Awake();
        }

        override protected void Start()
        {
            base.Start();

            m_characterController = GetComponent<CharacterController>();
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
            if (MoveToTarget != null)
            {
                /// TODO Change this into Path Finding behaviour.
                Vector3 target = MoveToTarget.Vector3 + new Vector3(0.5F, 0, 0.5F);
                Vector3 direction = (target - transform.position);
                float sqrDistance = direction.sqrMagnitude;

                if (sqrDistance > 0.5F)
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
                }
                else
                {
                    MoveToTarget = null;
                    m_moveDir = Vector3.zero;
                }
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
            MoveToTarget = target;
        }

        void IFocusTarget.OnFocusGained()
        {
        }

        void IFocusTarget.OnFocusLost()
        {
        }
    }
}