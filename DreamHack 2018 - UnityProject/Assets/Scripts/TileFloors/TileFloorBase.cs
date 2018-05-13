using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Game.TileFloors
{
    public abstract class TileFloorBase : TileProp
    {
        public TileFloorBase()
        {

        }

        public static TileFloorBase CreateAndParse(XElement element)
        {
            XAttribute typeAttrib = element.Attribute("type");
            if (typeAttrib == null)
                return null;

            Type classType = typeof(TileFloorBase).Assembly.GetType(typeAttrib.Value);
            if (classType != null)
            {
                TileFloorBase tileFloor = classType.Assembly.CreateInstance(classType.FullName) as TileFloorBase;
                tileFloor.Parse(element);

                return tileFloor;
            }

            return null;
        }
    }
}