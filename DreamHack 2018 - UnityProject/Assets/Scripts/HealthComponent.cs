using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Game
{
    public class HealthComponent
    {
        public delegate void ChangeHandler(int previousPoints, int newPoints);
        ChangeHandler m_changeHandlers;

        int m_healthPoints;
        int m_maxHealth;

        public int HealthPoints {
            get { return m_healthPoints; }
            set
            {
                if(m_healthPoints != value)
                {
                    if (m_changeHandlers != null)
                        m_changeHandlers(m_healthPoints, value);
                    m_healthPoints = value;
                }
            }
        }

        public int MaxHealth { get { return m_maxHealth; } }
        public float PercentageLeft { get { return m_maxHealth != 0 ? (float) m_healthPoints / m_maxHealth : 0; } }

        public HealthComponent(int maxHealth)
        {
            m_maxHealth = maxHealth;
            m_healthPoints = maxHealth;
        }

        public void Parse(XElement element)
        {
            XAttribute maxHealthAttrib = element.Attribute("maxHealth");
            XAttribute healthPointsAttrib = element.Attribute("healthPoints");

            if(maxHealthAttrib != null && healthPointsAttrib != null)
            {
                m_maxHealth = int.Parse(maxHealthAttrib.Value);
                m_healthPoints = int.Parse(healthPointsAttrib.Value);
            }
        }

        public void Populate(XElement element)
        {
            element.SetAttributeValue("maxHealth", m_maxHealth);
            element.SetAttributeValue("healthPoints", m_healthPoints);
        }

        public void RegisterChangeHandler(ChangeHandler handler)
        {
            if (!IsChangeHandlerRegistered(handler))
                m_changeHandlers += handler;
        }

        public void UnregisterChangeHandler(ChangeHandler handler)
        {
            m_changeHandlers -= handler;
        }

        public bool IsChangeHandlerRegistered(ChangeHandler handler)
        {
            if (m_changeHandlers != null)
                foreach (ChangeHandler h in m_changeHandlers.GetInvocationList())
                    if (h == handler) return true;
            return false;
        }

        public void SetHealthPointsNoNotify(int healthPoints)
        {
            m_healthPoints = healthPoints;
        }
    }
}