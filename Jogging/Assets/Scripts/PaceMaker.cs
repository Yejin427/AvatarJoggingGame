using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaceMaker : MonoBehaviour
{
    private float startTimer = 0f;
    public Animator animator;
    private int pointIdx = 0;

    private float zIncrement = 7f;

    private List<ParseJson.StateData> stateDataList;
    // Start is called before the first frame update
    void Start()
    {
        string json = PlayerPrefs.GetString("recordedState", "");
        Debug.Log("json: " + json);
        if (!string.IsNullOrEmpty(json)) {
            ParseJson.StateDataList dataList = JsonUtility.FromJson<ParseJson.StateDataList>(json);
            stateDataList = dataList.stateDataList;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(PersonalData.started) {
            if(PersonalData.playMode.Equals("yesterday")) {
                startTimer = Time.time;
                Debug.Log("Yesterday!");
                StartCoroutine(playYesterday());
            }
            else {
                animator.SetBool("paceWalking", true);
                Debug.Log("paceSpeed: "+PersonalData.paceSpeed);
                animator.speed = PersonalData.paceSpeed;
                StartCoroutine(playPaceMaker());
            }
        }
    }
    IEnumerator playYesterday()
    {
        while(true){
            if(pointIdx < stateDataList.Count) {
                ParseJson.StateData stateData = stateDataList[pointIdx];
                // 리스트에서 각각의 상태 변경   출력
                if(Time.time - startTimer >= float.Parse(stateData.Time)) {
                    pointIdx++;
                    if(stateData.State.Equals("walking")) {
                        Debug.Log("walking!!");
                        animator.SetBool("paceWalking", true);
                        animator.speed = float.Parse(stateData.Speed);
                        transform.position += new Vector3(0, 0, float.Parse(stateData.Speed) * zIncrement * Time.deltaTime);
                    } else if(stateData.State.Equals("stopping")){
                        Debug.Log("stopping!!");
                        animator.SetBool("paceWalking", false);
                        transform.rotation = Quaternion.Euler(0,0,0);
                        animator.speed = 1;
                        transform.position = new Vector3(transform.position.x, 1.38f, transform.position.z);
                    } else if(stateData.State.Equals("end")) {
                        animator.SetBool("paceWalking", false);
                        animator.speed = 1;
                        transform.rotation = Quaternion.Euler(0,0,0);
                        transform.position = new Vector3(transform.position.x, 1.38f, transform.position.z);
                    }
                }
            }
            yield return null;
        }
        
    }
    IEnumerator playPaceMaker()
    {
        while(true){
            transform.position += new Vector3(0, 0, 0.008f * PersonalData.paceSpeed * zIncrement * Time.deltaTime);
            yield return null;
        }
    }
}
