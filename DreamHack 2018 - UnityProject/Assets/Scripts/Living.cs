using Game.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class Living : MonoBehaviour, IAttackable
    {
        public delegate void OnDamageHandler(int damage, GameObject attacker);
        OnDamageHandler m_onDamageHandlers;

        public HealthComponent Health { get; private set; }
        public TilePosition CurrentTile { get; protected set; }

        public Vector3 Position
        {
            get
            {
                return transform.position;
            }

            set
            {
                transform.position = value;
                CurrentTile = TilePosition.FromWorldPosition(value);
            }
        }
        public GameObject GameObject { get { return gameObject; } }

        public bool Alive { get; protected set; }
        public abstract string DisplayName { get; }
        public abstract int MaxHealth { get; }
        bool IAttackable.IsDestroyed { get { return !Alive; } }

        protected virtual void Awake()
        {
            Alive = true;
            Health = new HealthComponent(MaxHealth);
            Health.RegisterChangeHandler(OnHealthChange);
        }

        // Use this for initialization
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        protected virtual void OnHealthChange(int previousPoints, int newPoints)
        {
            if(previousPoints > 0 && newPoints <= 0)
            {
                OnDeath();
                Health.SetHealthPointsNoNotify(0);
            }
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
            Health.HealthPoints -= damage;
        }

        public void RegisterOnDamageHandler(OnDamageHandler handler)
        {
            if (!IsOnDamageHandlerRegistered(handler))
                m_onDamageHandlers += handler;
        }

        public void UnregisterOnDamageHandler(OnDamageHandler handler)
        {
            m_onDamageHandlers -= handler;
        }

        public bool IsOnDamageHandlerRegistered(OnDamageHandler handler)
        {
            if (m_onDamageHandlers != null)
                foreach (OnDamageHandler h in m_onDamageHandlers.GetInvocationList())
                    if (h == handler) return true;
            return false;
        }

        public void NotifyOnDamage(int damage, GameObject attacker)
        {
            if (m_onDamageHandlers != null)
                m_onDamageHandlers(damage, attacker);
        }
    }
}