using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Game.TileObjects
{
    public abstract class TileObjectBase : TileProp
    {
        public TileObjectBase()
        {
        }

        public static TileObjectBase CreateAndParse(XElement element, Tile optionalTile = null)
        {
            XAttribute typeAttrib = element.Attribute("type");
            if (typeAttrib == null)
                return null;

            Type classType = typeof(TileObjectBase).Assembly.GetType(typeAttrib.Value);
            if (classType != null)
            {
                TileObjectBase tileObject = classType.Assembly.CreateInstance(classType.FullName) as TileObjectBase;
                tileObject.Parse(element, optionalTile);

                return tileObject;
            }

            return null;
        }
    }
}