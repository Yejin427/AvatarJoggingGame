using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentServer : MonoBehaviour
{
    private static PersistentServer instance;
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
