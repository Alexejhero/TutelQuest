using UnityEngine;

namespace SchizoQuest.Helpers
{
    public abstract class MonoSingleton<T> : MonoBehaviour
        where T : MonoSingleton<T>
    {
        // todo attribute to configure things like DontDestroyOnLoad
        private static T _instance;
        public static T Instance => _instance ? _instance : Initialize();

        private static T Initialize()
        {
            //Debug.Log($"Initializing {typeof(T).FullName} from\n{StackTraceUtility.ExtractStackTrace()}");
            GameObject go = new(typeof(T).FullName);
            return go.AddComponent<T>();
        }

        protected virtual void Awake()
        {
            _instance = (T)this;
        }

        protected virtual void OnEnable()
        {
            _instance = (T)this;
        }
    }
}