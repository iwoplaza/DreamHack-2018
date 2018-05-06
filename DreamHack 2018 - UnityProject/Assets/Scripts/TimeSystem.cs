using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using System;

namespace Game{
    public class TimeSystem {

        public float Time{ get; private set; }
        public float SecondsPerDay { get; private set; }
        public int DayCount{ get; private set; }
        public float DayProgress{ get{return Time/SecondsPerDay;}}
        
        public TimeSystem(float time, float secperday){
            Time = time;
            SecondsPerDay = secperday;
        }

        public TimeSystem() : this(0,120){}

        public void Update(){
            Time += UnityEngine.Time.deltaTime;
            if(Time >= SecondsPerDay){
                DayCount++;
                Time %= SecondsPerDay;
            }
        }

        public void Parse(XElement element){
            if(element == null)
                return;
            XAttribute timeAttribute = element.Attribute("CurrentTime");
            XAttribute dayAttribute = element.Attribute("DayCount");

            if(dayAttribute != null)
                DayCount = Int32.Parse(dayAttribute.Value);
            if(timeAttribute != null)
                Time = float.Parse(timeAttribute.Value) * SecondsPerDay;
        }

        public void Populate(XElement element) {
            element.SetAttributeValue("CurrentTime", DayProgress);
            element.SetAttributeValue("DayCount", DayCount);
        }
    }
}
