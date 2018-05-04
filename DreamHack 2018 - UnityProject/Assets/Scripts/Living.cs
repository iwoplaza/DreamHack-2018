using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class Living : MonoBehaviour
    {
        private int m_health;

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

        public virtual void Awake()
        {
            Alive = true;
        }

        // Use this for initialization
        public virtual void Start()
        {

        }

        // Update is called once per frame
        public virtual void Update()
        {

        }

        /// <summary>
        /// This is called whenever Health reaches 0
        /// </summary>
        public virtual void OnDeath()
        {
            Alive = false;
        }

        public void Damage(int damage, GameObject attacker)
        {
            Health -= damage;
        }
    }
}