using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Worker))]
    public class WorkerAttackBehaviour : MonoBehaviour
    {
        [SerializeField] float m_initialShootCooldown = 1.0F;
        [SerializeField] float m_subsequentShootCooldown = 0.5F;

        public Worker Worker { get; private set; }
        public Tasks.AttackTask AttackTask { get; private set; }
        public bool IsActive { get; private set; }
        public float ShootCooldown { get; private set; }

        public bool CanShootYet { get { return ShootCooldown <= 0; } }

        void Awake()
        {
            Worker = GetComponent<Worker>();

            IsActive = false;
        }

        public void Parse(XElement element)
        {
            XAttribute isActiveAttrib = element.Attribute("active");
            if (isActiveAttrib != null)
                IsActive = true;
        }

        public void Populate(XElement element)
        {
            if (IsActive)
                element.SetAttributeValue("active", true);
        }

        public void Activate(Tasks.AttackTask task)
        {
            if (IsActive)
                return;

            AttackTask = task;
            IsActive = true;

            StartCoroutine(HandleShooting());
        }

        public void Deactivate()
        {
            if (!IsActive)
                return;

            IsActive = false;
            StopAllCoroutines();
            Worker.PutWeaponAway();

            AttackTask = null;
        }

        void Shoot()
        {
            Worker.Shoot(AttackTask.Target);
        }

        void Update()
        {
            if (!IsActive)
                return;
        }

        IEnumerator HandleShooting()
        {
            TimeSystem timeSystem = WorldController.Instance.MainState.TimeSystem;

            while (true)
            {
                if (Worker.CanShotReachTarget(AttackTask.Target))
                {
                    Worker.CancelMove();
                    Worker.TurnTowards(AttackTask.Target.Position);
                    Worker.TakeWeaponOut();
                    float initialCooldownLeft = m_initialShootCooldown;
                    while (initialCooldownLeft > 0)
                    {
                        initialCooldownLeft -= Time.deltaTime * timeSystem.TimeMultiplier;
                        yield return new WaitForEndOfFrame();
                    }
                    while (true && IsActive)
                    {
                        ShootCooldown = m_subsequentShootCooldown;
                        while (ShootCooldown > 0)
                        {
                            ShootCooldown -= Time.deltaTime * timeSystem.TimeMultiplier;
                            yield return new WaitForEndOfFrame();
                        }
                        Worker.TurnTowards(AttackTask.Target.Position);
                        Shoot();
                        if (!Worker.CanShotReachTarget(AttackTask.Target))
                            break;
                    }
                }
                else
                {
                    TilePosition targetPosition = TilePosition.FromWorldPosition(AttackTask.Target.Position);
                    if (Worker.MoveTarget == null || Worker.MoveTarget != targetPosition)
                        Worker.MoveTo(targetPosition);
                    yield return new WaitForSeconds(0.1F);
                }
            }
        }
    }
}