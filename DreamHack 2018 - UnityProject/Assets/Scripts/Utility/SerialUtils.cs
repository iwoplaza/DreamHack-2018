using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Game.Utility
{
    public static class SerialUtils
    {
        public static Vector3 Parse(XElement element)
        {
            XAttribute xAttrib = element.Attribute("x");
            XAttribute yAttrib = element.Attribute("y");
            XAttribute zAttrib = element.Attribute("z");

            float x = 0, y = 0, z = 0;

            if (xAttrib != null)
                float.TryParse(xAttrib.Value, out x);
            if (yAttrib != null)
                float.TryParse(yAttrib.Value, out y);
            if (zAttrib != null)
                float.TryParse(zAttrib.Value, out z);

            return new Vector3(x, y, z);
        }

        public static void Populate(Vector3 vector, XElement element)
        {
            element.SetAttributeValue("x", vector.x);
            element.SetAttributeValue("y", vector.y);
            element.SetAttributeValue("z", vector.z);
        }
    }
}