using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lab5Games
{
    public abstract class Singleton<T> : Singleton where T : MonoBehaviour
    {
        static T _instance = null;
        static bool _initlializing = false;
        static readonly object _instanceLock = new object();

        public virtual bool Persistent { get; } = true;

        public static T Instance
        {
            get
            {
                lock(_instanceLock)
                {
                    // do nothing if currently quitting 
                    if (Quitting)
                        return null; 

                    // instance already found?
                    if (_instance != null)
                        return _instance;

                    _initlializing = true;

                    // search for and in-scene instance of T
                    var allInstances = FindObjectsByType<T>(FindObjectsSortMode.None);

                    // found exactly one?
                    if(allInstances.Length == 1)
                    {
                        Debug.Log($"Found exactly 1 {typeof(T)}");
                        _instance = allInstances[0];
                    } // found none?
                    else if(allInstances.Length == 0)
                    {
                        Debug.Log($"Found exactly no {typeof(T)}");
                        _instance = new GameObject($"[Singleton] {typeof(T)}").AddComponent<T>();
                    } // multiple found?
                    else
                    {
                        Debug.Log($"Found exactly {allInstances.Length} {typeof(T)}");
                        _instance = allInstances[0];

                        // destroy the duplicates
                        for(int indx=1; indx<allInstances.Length; indx++)
                        {
                            Debug.LogError($"Destroying duplicate {typeof(T)} on {allInstances[indx].gameObject.name}");
                            Destroy(allInstances[indx].gameObject);
                        }
                    }

                    _initlializing = false;
                    return _instance;
                }
            }
        }

        static void ConstructIfNeeded(Singleton<T> inInstance)
        {
            lock(_instanceLock)
            {
                // only construct if the instance is null and is not being initialized
                if(_instance == null && !_initlializing)
                {
                    Debug.Log($"ConstructIfNeeded run for {typeof(T)}");
                    _instance = inInstance as T;
                }
                else if(_instance != null && !_initlializing)
                {
                    Debug.LogError($"Destroying duplicate {typeof(T)} on {inInstance.gameObject.name}");
                    Destroy(inInstance.gameObject);
                }
            }
        }

        private void Awake()
        {
            ConstructIfNeeded(this);
            
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            if(Persistent)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    public abstract class Singleton : MonoBehaviour
    {
        public static bool Quitting { get; private set; } = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnBeforeSceneLoad()
        {
            Quitting = false;
        }

        private void OnApplicationQuit()
        {
            Debug.Log($"Quitting in progress");
            Quitting = true;
        }
    }
}
