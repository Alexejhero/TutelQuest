using UnityEngine;

namespace SchizoQuest.Helpers
{
    public static class ComponentHelpers
    {
        public static T EnsureComponent<T>(this GameObject gameObject)
            where T : Component
        {
            T comp = gameObject.GetComponent<T>();
            if (!comp) comp = gameObject.AddComponent<T>();
            
            return comp;
        }

        public static T EnsureComponent<T>(this Component component)
            where T : Component
        {
            return component.gameObject.EnsureComponent<T>();
        }

        public static T EnsureComponent<T>(this Component component, ref T field)
            where T : Component
        {
            if (!field) field = EnsureComponent<T>(component);
            return field;
        }
    }
}