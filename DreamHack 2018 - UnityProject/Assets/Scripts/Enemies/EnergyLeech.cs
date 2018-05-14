using Game.Acting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Acting.Actions;
using Game.Tasks;
using System;
using Game;
using Game.Animation;
using Game.Pathfinding;
using Game.Pathfinding.Rules;

namespace Game.Enemies
{
    public class EnergyLeech : Enemy, ISubject, IFocusTarget
    {
        public override string DisplayName { get { return "Energy Leech"; } }
        public override int MaxHealth { get { return 100; } }
        Transform IFocusTarget.PortraitPivot { get { return null; } }
        public bool IsDestroyed { get { return !Alive; } }

        public EnergyLeechVisual Visual { get; private set; }
        public bool IsWalking { get; private set; }
        public PathfindingAgent PathfindingAgent { get; private set; }

        public TilePosition MainTarget;

        public float AttackCooldown { get; private set; }
        public int AttackPower { get; private set; }

        public Living LivingAttackTarget { get; private set; }

        [SerializeField] private float m_walkSpeed;
        [SerializeField] private float m_runSpeed;
        [SerializeField] private float m_stickToGroundForce;
        [SerializeField] private float m_gravityMultiplier;

        private TileMap m_tileMap;
        private CharacterController m_characterController;
        private Vector3 m_moveDir = Vector3.zero;
        private CollisionFlags m_collisionFlags;
        private bool m_previouslyGrounded;
        private bool m_running = false;
        private bool m_recentlyClawed = false;
        Focus m_focus;

        override protected void Awake()
        {
            base.Awake();

            AttackCooldown = 0.5F;
            AttackPower = 10;
            m_characterController = GetComponent<CharacterController>();
        }

        public void Setup(TileMap tileMap)
        {
            m_tileMap = tileMap;
            Visual = GetComponent<EnergyLeechVisual>();
            PathfindingAgent = new PathfindingAgent(new EnergyLeechRule(), m_tileMap);

            OverrideAttackTarget(WorldController.Instance.MainState.MainGeneratorPosition, true);
        }

        protected override void Start()
        {
            OverrideAttackTarget(WorldController.Instance.MainState.MainGeneratorPosition, true);
        }

        List<ActionBase> ISubject.GetActionsFor(IActor actor)
        {
            List<ActionBase> actions = new List<ActionBase>();
            actions.Add(new PerformTaskAction(new AttackTask(this)));

            return actions;
        }

        void FixedUpdate()
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
            if (Visual != null)
                Visual.UpdateAnimator();
        }

        /// <summary>
        /// Side track and attack a living target.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public IEnumerator AttackBehaviour(Living target)
        {
            PathfindingAgent.GeneratePath(CurrentTile, target.CurrentTile);
            while(PathfindingAgent.CurrentStatus == PathfindingStatus.GENERATING_PATH)
            {
                yield return new WaitForEndOfFrame();
            }
            if(PathfindingAgent.CurrentStatus == PathfindingStatus.PATH_FINISHED)
            {
                OverrideAttackTarget(MainTarget,true);
                yield break;
            }
            while(!CanClawReach(target))
            {
                yield return new WaitForEndOfFrame();
            }
            PathfindingAgent.CancelPath();
            while(target.Alive && Alive)
            {
                m_recentlyClawed = false;
                ClawAt(target);
                m_recentlyClawed = true;
                yield return new WaitForSeconds(AttackCooldown);
                m_recentlyClawed = false;
            }
            if(Alive)
            {
                OverrideAttackTarget(MainTarget, true);
            }
        }

        public bool IsNearEnough(TilePosition targetPosition, int distance)
        {
            if(Mathf.RoundToInt(new Vector2(targetPosition.X-CurrentTile.X,targetPosition.Z-CurrentTile.Z).magnitude) < distance)
            {
                return true;
            }
            else
                return false;
        }

        public override void Damage(int damage, GameObject attacker)
        {
            base.Damage(damage, attacker);

            if (attacker.GetComponent<Living>() != null)
            {
                OverrideAttackTarget(attacker.GetComponent<Living>().CurrentTile, attacker);
                LivingAttackTarget = attacker.GetComponent<Living>();
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.GetComponent<Worker>() != null)
            {
                ClawAt(collision.collider.GetComponent<Worker>());
            }
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            if (m_focus != null)
                m_focus.On(null);
            WorldController.Instance.MainState.OnEnergyLeachDeath(this);
            Destroy(gameObject);
        }

        public IEnumerator AttackMainTarget()
        {
            Debug.Log("Starting to attack...");

            if (CurrentTile != MainTarget)
            {
                PathfindingAgent.GeneratePath(CurrentTile, MainTarget);
                while (PathfindingAgent.CurrentStatus == PathfindingStatus.GENERATING_PATH)
                {
                    yield return new WaitForEndOfFrame();
                }
                if (PathfindingAgent.CurrentStatus == PathfindingStatus.PATH_FINISHED)
                {
                    Debug.Log("Found path to main target");
                    OverrideAttackTarget(MainTarget, true);
                    yield break;
                }
                while (!IsNearEnough(MainTarget, 1))
                {
                    yield return new WaitForEndOfFrame();
                }
                PathfindingAgent.CancelPath();
            }
            TurnTowards(MainTarget.Vector3 + new Vector3(0.5F, 0, 0.5F));

            Tile targetTile = m_tileMap.TileAt(MainTarget);
            TileProp targetProp = targetTile.GetProp(PropType.OBJECT);
            if (targetProp.Health != null)
            {
                while (!targetProp.IsDestroyed && Alive)
                {
                    m_tileMap.TileAt(MainTarget).GetProp(PropType.OBJECT).Damage(AttackPower, this.gameObject);
                    yield return new WaitForSeconds(AttackCooldown);
                }
            }
            //LOSE??
        }

        public void OverrideAttackTarget(TilePosition targetTile, GameObject newTarget)
        {
            //PathfindingAgent.CancelPath();
            //PathfindingAgent.GeneratePath(CurrentTile,targetTile);
            //PathfindingAgent.RegisterStatusChangeHandler(StatusChangeHandler);
            StopAllCoroutines();
            if(newTarget.GetComponent<Living>() != null)
            {
                StartCoroutine(AttackBehaviour(newTarget.GetComponent<Living>()));
            }
        }

        public void OverrideAttackTarget(TilePosition targetTile, bool isMainTarget)
        {
            StopAllCoroutines();
            PathfindingAgent.CancelPath();
            PathfindingAgent.GeneratePath(CurrentTile,targetTile);
            if(isMainTarget)
            {
                Debug.Log("Trying to start trying to attack the main Target at " + targetTile);
                MainTarget = targetTile;
                StartCoroutine(AttackMainTarget());
            }
        }

        public bool CanClawReach(IAttackable target)
        {
            if (target == null)
                return false;

            Vector3 direction = target.Position - Position;
            direction.y = 0;
            float distance = direction.magnitude;
            if (distance <= 1.5F)
                return true;

            return false;
        }

        public void ClawAt(Living target)
        {
            if (!m_recentlyClawed)
            {
                TurnTowards(target.Position + new Vector3(0.5F, 0, 0.5F));
                target.Damage(AttackPower, GameObject);
            }
        }

        public void OnEnemyDead()
        {
        }

        public void TurnTowards(Vector3 position)
        {
            Vector3 difference = position - transform.position;
            transform.rotation = Quaternion.Euler(0, Mathf.Atan2(difference.x, difference.z) / Mathf.PI * 180.0F, 0);
        }

        void IFocusTarget.OnFocusGained(Focus focus)
        {
            m_focus = focus;
        }

        void IFocusTarget.OnFocusLost(Focus focus)
        {
            m_focus = null;
        }
    }
}