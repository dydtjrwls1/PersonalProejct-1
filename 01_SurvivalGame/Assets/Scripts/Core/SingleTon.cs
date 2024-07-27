using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleTon<T> : MonoBehaviour where T : Component
{
    // private bool isInitialized = false;

    private static bool isShutdown = false;

    static T instance = null;

    public static T Instance
    {
        get
        {
            if (isShutdown)
            {
                Debug.LogWarning("게임 종료 도중 요구받음");
                return null;
            }

            if (instance == null)
            {
                T singleton = FindAnyObjectByType<T>();
                if (singleton == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = $"{typeof(T)}_Singleton";
                    singleton = obj.AddComponent<T>();
                }

                instance = singleton;
                DontDestroyOnLoad(instance);
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(instance);
        } else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode != LoadSceneMode.Additive)
        {
            OnInitialize();
        }
    }

    private void OnApplicationQuit()
    {
        isShutdown = true;
    }

    protected virtual void OnInitialize()
    {

    }
}
