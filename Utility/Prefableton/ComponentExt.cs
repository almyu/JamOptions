using UnityEngine;
using System.Collections.Generic;

namespace UnityEngine {

    public static class ComponentExt {

        public static T GetInstance<T>(this T component) where T : Component {
            return component.IsPrefab() ? Storage<T>.Get(component) : component;
        }

        public static bool IsPrefab<T>(this T obj) where T : Object {
            return (obj.hideFlags & HideFlags.HideInHierarchy) != 0;
        }

        private class Storage<T> where T : Component {
            public static Dictionary<T, T> instances = new Dictionary<T, T>();

            public static T Get(T prefab) {
                var instance = default(T);
                var present = instances.TryGetValue(prefab, out instance);

                if (instance) return instance;

                instance = Instantiate(prefab);

                if (present) instances[prefab] = instance;
                else instances.Add(prefab, instance);

                return instance;
            }

            public static T Instantiate(T prefab) {
                var name = prefab.name;

                var instance = Object.Instantiate(prefab);
                instance.name = name;

                return instance;
            }
        }


        public static bool CacheComponent<T>(this Component target, ref T component) where T : Component {
            if (!component) component = target.GetComponent<T>();
            return !!component;
        }
    }
}
