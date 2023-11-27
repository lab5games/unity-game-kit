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

        public static T Instance
        {
            get
            {
                lock(_instanceLock)
                {
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

        public virtual bool Persistent { get; } = true;

        private void Awake()
        {
            
        }
    }

    public abstract class Singleton : MonoBehaviour
    {
    }
}
