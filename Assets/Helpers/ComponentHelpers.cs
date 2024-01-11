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

        public static T EnsureComponent<T>(this Component component, ref T field, bool warnIfMissing = true)
            where T : Component
        {
            if (!field)
            {
                if (warnIfMissing)
                    Debug.LogWarning($"{component.GetType().Name} ({component.name}) has a field of type {typeof(T).Name} that was not assigned!"
                        + "\nReassigning automatically, this may create a new component");
                field = EnsureComponent<T>(component);
            }
            return field;
        }
    }
}