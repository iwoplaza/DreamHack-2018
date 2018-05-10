using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using System;

namespace Game
{
    public class TimeSystem
    {
        public float Time { get; private set; }
        public float SecondsPerDay { get; private set; }
        public int DayCount { get; private set; }
        public TimeMode CurrentMode { get; set; }

        public float DayProgress { get { return Time / SecondsPerDay; } }
        public int Hour { get { return Mathf.FloorToInt(DayProgress * 24); } }
        public int Minute { get { return Mathf.FloorToInt(((DayProgress * 24) % 1) * 60); } }
        public string TimeString { get { return Hour.ToString().PadLeft(2,'0') + ":" + Minute.ToString().PadLeft(2,'0'); } }

        public float TimeMultiplier
        {
            get
            {
                switch (CurrentMode)
                {
                    case TimeMode.PAUSE:
                        return 0;
                    case TimeMode.MULTIPLIER_2X:
                        return 2;
                    case TimeMode.MULTIPLIER_4X:
                        return 4;
                    case TimeMode.MULTIPLIER_8X:
                        return 8;
                    default:
                        return 1;
                }
            }
        }

        public TimeSystem(float time, float secperday)
        {
            Time = time;
            SecondsPerDay = secperday;
        }

        public TimeSystem() : this(0,120) {}

        public void Update()
        {
            Time += UnityEngine.Time.deltaTime * TimeMultiplier;
            if(Time >= SecondsPerDay){
                DayCount++;
                Time %= SecondsPerDay;
            }
        }

        public void Parse(XElement element)
        {
            if(element == null)
                return;
            XAttribute timeAttribute = element.Attribute("CurrentTime");
            XAttribute dayAttribute = element.Attribute("DayCount");

            if(dayAttribute != null)
                DayCount = Int32.Parse(dayAttribute.Value);
            if(timeAttribute != null)
                Time = float.Parse(timeAttribute.Value) * SecondsPerDay;
        }

        public void Populate(XElement element)
        {
            element.SetAttributeValue("CurrentTime", DayProgress);
            element.SetAttributeValue("DayCount", DayCount);
        }

        public enum TimeMode
        {
            NORMAL,
            PAUSE,
            MULTIPLIER_2X,
            MULTIPLIER_4X,
            MULTIPLIER_8X
        }
    }
}
