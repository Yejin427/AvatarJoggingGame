using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuNavigation : MonoBehaviour
{
    public GameObject playMode;
    // public Transform avatar;
    
    // private Transform leftFoot;
    // private Vector3 prev;
    // private Vector3 cur;

    public TMP_InputField speedText;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playMode.SetActive(false);
        // Transform[] all = avatar.GetComponentsInChildren<Transform>();
        // foreach (Transform t in all)
        // {
        //     if (t.name.Contains("LeftFoot"))
        //     {
        //         leftFoot = t;
        //         prev = t.position;
        //         break;
        //     }
        // }

    }
    // void Update()
    // {
    //     cur = leftFoot.position;
    //     Debug.Log("speed: "+((cur - prev) / Time.deltaTime).magnitude);
    //     prev = cur;
    // }

    // Play 버튼 클릭 시 호출될 함수
    public void OnPlayButtonClick()
    {
        playMode.SetActive(true);
    }
    public void aloneButtonClick(){
        PersonalData.playMode = "alone";
        SceneManager.LoadScene("SettingsScene");
    }
    public void yesterdayButtonClick(){
        PersonalData.playMode = "yesterday";
        SceneManager.LoadScene("SettingsScene");
    }
    public void paceMakerButtonClick(){
        PersonalData.playMode = "pacemaker";
        PersonalData.paceSpeed = float.Parse(speedText.text);
        SceneManager.LoadScene("SettingsScene");
    }
    public void previewSpeed(){
        animator.speed = float.Parse(speedText.text);
    }

    // Exit 버튼 클릭 시 호출될 함수
    public void OnExitButtonClick()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 실행 중인 경우 종료
#endif
    }
}
