using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParseJson;
public class TestAnimation : MonoBehaviour
{
    private float start = 0f;
    public Animator animator;
    private int pointIdx = 0;

    private List<ParseJson.StateData> stateDataList;
    // Start is called before the first frame update
    void Start()
    {
        start = Time.time;
        string json = PlayerPrefs.GetString("recordedState", "");
        if (!string.IsNullOrEmpty(json)) {
            ParseJson.StateDataList dataList = JsonUtility.FromJson<ParseJson.StateDataList>(json);
            stateDataList = dataList.stateDataList;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.HasKey("recordedState")) {
            // 저장된 JSON 문자열을 불러옴
            string json = PlayerPrefs.GetString("recordedState");

            // JSON 문자열을 리스트로 복원
            if(pointIdx < stateDataList.Count) {
                ParseJson.StateData stateData = stateDataList[pointIdx];
                // 리스트에서 각각의 상태 변경 시점 출력
                if(Time.time - start >= float.Parse(stateData.Time)) {
                    pointIdx++;
                    if(stateData.State == "walking") {
                        animator.SetBool("walking", true);
                    } else if(stateData.State == "stopping"){
                        animator.SetBool("walking", false);
                        transform.rotation = Quaternion.Euler(0,0,0);
                    }
                }
            }
        }
    }
}