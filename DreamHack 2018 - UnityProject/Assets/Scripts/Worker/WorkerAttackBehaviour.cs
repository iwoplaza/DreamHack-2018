using System.Collections;
using System.Collections.Generic;
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

        public void Activate(Tasks.AttackTask task)
        {
            if (IsActive)
                return;

            AttackTask = task;
            IsActive = true;
            ShootCooldown = m_initialShootCooldown;
            Worker.TakeWeaponOut();

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
            Worker.Shoot();
        }

        void Update()
        {
            if (!IsActive)
                return;

            Worker.TurnTowards(AttackTask.Target.transform.position);
        }

        IEnumerator HandleShooting()
        {
            yield return new WaitForSeconds(m_initialShootCooldown);
            while (true && IsActive)
            {
                ShootCooldown = m_subsequentShootCooldown;
                yield return new WaitForSeconds(ShootCooldown);
                ShootCooldown = 0;
                Shoot();
            }
        }
    }
}