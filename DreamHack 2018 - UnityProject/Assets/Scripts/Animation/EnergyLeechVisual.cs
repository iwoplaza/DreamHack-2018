using Game.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Animation
{
    [RequireComponent(typeof(EnergyLeech))]
    public class EnergyLeechVisual : MonoBehaviour
    {
        private Animator m_animator;

        public EnergyLeech EnergyLeech { get; private set; }

        void Awake()
        {
            m_animator = GetComponentInChildren<Animator>();
            EnergyLeech = GetComponent<EnergyLeech>();
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnAttack()
        {
            WorldController worldController = WorldController.Instance;
            if (WorldController.Instance.Initialised)
            {
                m_animator.SetTrigger("Attack");
            }
        }

        public void UpdateAnimator()
        {
            WorldController worldController = WorldController.Instance;
            if (WorldController.Instance.Initialised)
            {
                TimeSystem timeSystem = WorldController.Instance.MainState.TimeSystem;
                m_animator.speed = timeSystem.TimeMultiplier;
                m_animator.SetBool("Walk", EnergyLeech.IsWalking);
            }
        }
    }
}