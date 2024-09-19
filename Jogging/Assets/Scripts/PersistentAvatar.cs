using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentAvatar : MonoBehaviour
{
    // Start is called before the first frame update
    public static PersistentAvatar Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 특정 씬이 로드될 때 오브젝트를 파괴
        if (scene.name == "IntroScene")
        {
            Destroy(gameObject);
        }
    }
}
