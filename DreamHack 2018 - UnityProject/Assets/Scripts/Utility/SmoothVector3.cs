using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utility
{
    public class SmoothVector3
    {
        Vector3 m_start;
        Vector3 m_end;
        float m_progress;

        public float Speed { get; set; }

        public Vector3 Value
        {
            get
            {
                return m_start + (m_end - m_start) * m_progress;
            }

            set
            {
                m_start = Value;
                m_end = value;
                m_progress = 0;
            }
        }

        public void Update(float deltaTime)
        {
            if (m_progress < 1)
            {
                m_progress += deltaTime * Speed;
                if (m_progress > 1)
                    m_progress = 1;
            }
        }

        public SmoothVector3(Vector3 vec)
        {
            m_start = new Vector3(vec.x, vec.y, vec.z);
            m_end = new Vector3(vec.x, vec.y, vec.z);
            m_progress = 1;
        }

        public SmoothVector3() : this(Vector3.zero) {}
    }
}