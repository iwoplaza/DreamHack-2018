using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using System;

namespace Game{
    public class TimeSystem {

        public float m_Time{ get; private set; }
        public float m_SecondsPerDay { get; private set; }
        public int m_DayCount{ get; private set; }
        public float dayProgress{ get{return m_Time/m_SecondsPerDay;}}
        
        public TimeSystem(float time, float secperday){
            m_Time = time;
            m_SecondsPerDay = secperday;
        }

        public TimeSystem() : this(0,120){}

        public void Update(){
            m_Time += UnityEngine.Time.deltaTime;
            if(m_Time >= m_SecondsPerDay){
                m_DayCount++;
                m_Time %= m_SecondsPerDay;
            }
        }

        public void Parse(XElement element){
            if(element == null)
                return;
            XAttribute timeAttribute = element.Attribute("CurrentTime");
            XAttribute dayAttribute = element.Attribute("DayCount");

            if(dayAttribute != null)
                m_DayCount = Int32.Parse(dayAttribute.Value);
            if(timeAttribute != null)
                m_Time = float.Parse(timeAttribute.Value) * m_SecondsPerDay;
        }

        public void Populate(XElement element) {
            element.SetAttributeValue("CurrentTime", dayProgress);
            element.SetAttributeValue("DayCount", m_DayCount);
        }
    }
}
