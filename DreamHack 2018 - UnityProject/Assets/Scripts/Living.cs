using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class Living : FocusTarget
    {
        [SerializeField] private int m_health;

        public virtual int Health {
            get { return m_health; }
            set {
                m_health = value;
                if (m_health <= 0)
                {
                    m_health = 0;
                    OnDeath();
                }
            }
        }
        public bool Alive { get; protected set; }
        public abstract int MaxHealth { get; }

        protected override void Awake()
        {
            Alive = true;
            m_health = MaxHealth;
        }

        // Use this for initialization
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        /// <summary>
        /// This is called whenever Health reaches 0
        /// </summary>
        protected virtual void OnDeath()
        {
            Alive = false;
        }

        public void Damage(int damage, GameObject attacker)
        {
            Health -= damage;
        }
    }
}