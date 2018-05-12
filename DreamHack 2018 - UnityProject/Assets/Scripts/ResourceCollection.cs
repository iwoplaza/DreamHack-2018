using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ResourceCollection<T> where T : Object
    {
        public Dictionary<string, T> Collection { get; private set; }
        public string ResourceName { get; private set; }

        public ResourceCollection(string resourceName)
        {
            ResourceName = resourceName;
        }

        public ResourceCollection(string resourceName, string location)
        {
            ResourceName = resourceName;
            Load(location);
        }

        public void Load(string location)
        {
            T[] objects = UnityEngine.Resources.LoadAll<T>(location);
            Collection = new Dictionary<string, T>();
            foreach (T obj in objects)
                Collection.Add(obj.name, obj);
        }

        public T Find(string name)
        {
            if (!Collection.ContainsKey(name))
            {
                Debug.LogError("Couldn't find a " + ResourceName + " (" + name + ").");
                return null;
            }
            return Collection[name];
        }

        public bool ContainsKey(string key)
        {
            return Collection.ContainsKey(key);
        }
    }
}