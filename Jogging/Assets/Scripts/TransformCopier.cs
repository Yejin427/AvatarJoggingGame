using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Animations;
using TMPro;
using ParseJson;

// Simply copy's a transform and it's children on to another same object.
// Optionally generates a free parent to arbitrarily move/rotate.
public class TransformCopier : MonoBehaviour
{
    public Transform source; // The source. Probably the transform with the Avatar component.
    public Animator animator;
    public Camera mainCamera;   //camera z축으로 이동 걸으면서 3.5씩 z증
    private float leftgroundLevel;
    private Transform leftFoot;
    private Transform rightFoot;
    private const float zIncrement = 7f;   //실제 70cm
    private const float block = 64f;    //한 블록당 64
    private int blockCnt = 0;
    public GameObject paceMaker;
    public GameObject block0;
    public GameObject block1;
    public GameObject block2;
    public GameObject block3;
    public GameObject block4;
    public GameObject block5;
    private float footTouchThreshold = 0.1f;
    private int distanceM = 0;
    private float recordTimer = 0f;
    private float footTouchTime = 0f;
    public Button startButton;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI lastRecordText;
    public GameObject exitPopup;
    public TextMeshProUGUI finalDistanceText;
    private List<StateData> recordState = new List<ParseJson.StateData>();
    private List<ParseJson.StateData> lastStateDataList;

    private void Start()
    {
        source = PersistentAvatar.Instance.GetComponent<Transform>();

        //source Avatar와 신체 길이를 맞춤
        fixAvatarScale(paceMaker.GetComponent<Animator>());
        exitPopup.SetActive(false);
        string json = PlayerPrefs.GetString("recordedState", "");
        if (!string.IsNullOrEmpty(json)) {
            ParseJson.StateDataList dataList = JsonUtility.FromJson<ParseJson.StateDataList>(json);
            lastStateDataList = dataList.stateDataList;
        }
        if(PersonalData.playMode.CompareTo("alone") == 0) { //혼자 달리기
            paceMaker.SetActive(false);
            transform.position = new Vector3(-0.76f, 1.38f, -30);
        }
        Transform[] all = source.GetComponentsInChildren<Transform>();
        foreach (Transform t in all)
        {
            if (t.name.Contains("LeftFoot"))
            {
                leftFoot = t;
                leftgroundLevel = t.position.y;
            }
            if (t.name.Contains("RightFoot"))
            {
                rightFoot = t;
            }
        }
        if(PlayerPrefs.HasKey("PreviousScore")) {
            lastRecordText.text = "Last Record: " + PlayerPrefs.GetInt("PreviousScore") + "m";
        } else {
            lastRecordText.text = "Last Record: 0m";
        }
    }

    private void fixAvatarScale(Animator anim)
    {
        Transform rightUpperArm = anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
        Transform rightLowerArm = anim.GetBoneTransform(HumanBodyBones.RightLowerArm);
        Transform leftUpperArm = anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        Transform leftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        rightUpperArm.localScale = new Vector3(rightUpperArm.localScale.x, rightUpperArm.localScale.y * (PersonalData.armLength / PersonalData.manArmLength), rightUpperArm.localScale.z);
        rightLowerArm.localScale = new Vector3(rightLowerArm.localScale.x, rightLowerArm.localScale.y * (PersonalData.armLength / PersonalData.manArmLength), rightLowerArm.localScale.z);
        leftUpperArm.localScale = new Vector3(leftUpperArm.localScale.x, leftUpperArm.localScale.y * (PersonalData.armLength / PersonalData.manArmLength), leftUpperArm.localScale.z);
        leftLowerArm.localScale = new Vector3(leftLowerArm.localScale.x, leftLowerArm.localScale.y * (PersonalData.armLength / PersonalData.manArmLength), leftLowerArm.localScale.z);
        
        //upper body
        Transform spine = anim.GetBoneTransform(HumanBodyBones.Spine);
        Transform chest = anim.GetBoneTransform(HumanBodyBones.Chest);
        Transform upperChest = anim.GetBoneTransform(HumanBodyBones.UpperChest);
        spine.localScale = new Vector3(spine.localScale.x, spine.localScale.y * (PersonalData.upperBodyLength / PersonalData.manBodyLength), spine.localScale.z);
        chest.localScale = new Vector3(chest.localScale.x, chest.localScale.y * (PersonalData.upperBodyLength / PersonalData.manBodyLength), chest.localScale.z);
        upperChest.localScale = new Vector3(upperChest.localScale.x, upperChest.localScale.y * (PersonalData.upperBodyLength / PersonalData.manBodyLength), upperChest.localScale.z);

        //leg
        Transform leftUpperLeg = anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        Transform rightUpperLeg = anim.GetBoneTransform(HumanBodyBones.RightUpperLeg);
        Transform leftLowerLeg = anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
        Transform rightLowerLeg = anim.GetBoneTransform(HumanBodyBones.RightLowerLeg);
        leftUpperLeg.localScale = new Vector3(leftUpperLeg.localScale.x, leftUpperLeg.localScale.y * (PersonalData.height / PersonalData.manHeight), leftUpperLeg.localScale.z);
        rightUpperLeg.localScale = new Vector3(rightUpperLeg.localScale.x, rightUpperLeg.localScale.y * (PersonalData.height / PersonalData.manHeight), rightUpperLeg.localScale.z);
        leftLowerLeg.localScale = new Vector3(leftLowerLeg.localScale.x, leftLowerLeg.localScale.y * (PersonalData.height / PersonalData.manHeight), leftLowerLeg.localScale.z);
        rightLowerLeg.localScale = new Vector3(rightLowerLeg.localScale.x, rightLowerLeg.localScale.y * (PersonalData.height / PersonalData.manHeight), rightLowerLeg.localScale.z);
    }
    private void Update()
    {
        
    }

    IEnumerator Play(){
        recordTimer = Time.time;
        Vector3? prevpos = null;
        Vector3? curpos = null;
        int foot = 0;   //0 왼발 1 오른발
        float stopTimer = 0f;
        float walkTimer = 0f;
        float speed = 1;
        while (true) {
            if (leftFoot != null && rightFoot != null) {
                // 왼쪽 발과 오른쪽 발의 현재 높이 계산
                float leftFootHeight = leftFoot.position.y - leftgroundLevel;
                float rightFootHeight = rightFoot.position.y - leftgroundLevel;
                // 두 발이 다 땅에 닿는지 체크
                if(animator.GetBool("walking") && leftFootHeight < footTouchThreshold && rightFootHeight < footTouchThreshold) {
                    stopTimer += Time.deltaTime;
                    if(stopTimer > 0.5f) { 
                        stopTimer = 0f;
                        prevpos = null; curpos = null;
                        speed = 1;
                        animator.SetBool("walking", false);
                        animator.speed = 1;
                        transform.position = new Vector3(transform.position.x, 1.38f, transform.position.z);
                        transform.rotation = Quaternion.Euler(0,0,0);
                        addState("stopping", Time.time, 1);
                    } else {
                        walking(speed);
                    }
                }
                else if(!animator.GetBool("walking") && (leftFootHeight >= footTouchThreshold || rightFootHeight >= footTouchThreshold)) {
                    if(leftFootHeight >= footTouchThreshold) {
                        foot = 0;
                        prevpos = leftFoot.position;
                    } else {
                        foot = 1;
                        prevpos = rightFoot.position;
                    }
                    animator.SetBool("walking", true);
                }
                else if(animator.GetBool("walking")) {
                    if(!curpos.HasValue) {
                        if(foot == 0) {
                            curpos = leftFoot.position;
                        }
                        else {
                            curpos = rightFoot.position;
                        }
                        speed = (((curpos - prevpos) / Time.deltaTime).Value.magnitude) / 18;
                        Debug.Log("speed: "+speed);
                        animator.speed = speed;
                        addState("walking", Time.time, speed);
                    }
                    stopTimer = 0f;
                    walking(speed);
                }
            }
            yield return null;
        }
    }

    IEnumerator paceMakerPlay()
    {
        Animator paceAni = paceMaker.GetComponent<Animator>();
        paceAni.speed = PersonalData.paceSpeed;
        paceAni.SetBool("paceWalking", true);
        while(true){
            paceMaker.transform.position += new Vector3(0, 0, PersonalData.paceSpeed * zIncrement * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator playYesterday()
    {
        Animator paceAni = paceMaker.GetComponent<Animator>();
        int pointIdx = 0;
        while(true){
            if(pointIdx < lastStateDataList.Count) {
                ParseJson.StateData stateData = lastStateDataList[pointIdx];
                // 리스트에서 각각의 상태 변경 출력
                if(Time.time - recordTimer >= float.Parse(stateData.Time)) {    //state 변경시 여기
                    pointIdx++;
                    if(stateData.State.Equals("walking")) {
                        Debug.Log("walking!!");
                        paceAni.SetBool("paceWalking", true);
                        paceAni.speed = float.Parse(stateData.Speed);
                        paceMaker.transform.position += new Vector3(0, 0, float.Parse(stateData.Speed) * zIncrement * Time.deltaTime);
                    } else if(stateData.State.Equals("stopping")){
                        Debug.Log("stopping!!");
                        paceAni.SetBool("paceWalking", false);
                        paceMaker.transform.rotation = Quaternion.Euler(0,0,0);
                        paceAni.speed = 1;
                        paceMaker.transform.position = new Vector3(paceMaker.transform.position.x, 1.38f, paceMaker.transform.position.z);
                    } else if(stateData.State.Equals("end")) {
                        paceAni.SetBool("paceWalking", false);
                        paceAni.speed = 1;
                        paceMaker.transform.rotation = Quaternion.Euler(0,0,0);
                        paceMaker.transform.position = new Vector3(paceMaker.transform.position.x, 1.38f, paceMaker.transform.position.z);
                    }
                } else {    //state 유지 시
                    if(paceAni.GetBool("paceWalking")) {
                        paceMaker.transform.position += new Vector3(0, 0, float.Parse(stateData.Speed) * zIncrement * Time.deltaTime);
                    }
                }
            }
            yield return null;
        }
    }

    private void addState(string state, float time, float speed) {
        float elapsedTime = time - recordTimer;
        
        recordState.Add(new ParseJson.StateData(state, elapsedTime.ToString(), speed.ToString()));
    }

    private void walking(float speed)
    {
        transform.position += new Vector3(0, 0, speed * zIncrement * Time.deltaTime);
        mainCamera.transform.position += new Vector3(0, 0, speed * zIncrement * Time.deltaTime);
        float distanceZ = transform.position.z + 30;
        distanceM = (int)(distanceZ * 0.25);
        //1km 이하는 m로 표시 1km이상은 km단위 표시
        if (distanceM < 1000) {
            distanceText.text = distanceM.ToString() + "m";
        } else {
            distanceText.text = (float)distanceM / 1000 + "km";
        }
        if(distanceZ >= block * blockCnt) {
            blockCnt += 1;
            GameObject newBlock;
            if((blockCnt-1) % 6 == 0){
                newBlock = Instantiate(block0) as GameObject;
            }
            else if ((blockCnt - 1) % 6 == 1){
                newBlock = Instantiate(block1) as GameObject;
            }
            else if((blockCnt - 1) % 6 == 2){
                newBlock = Instantiate(block2) as GameObject;
            }
            else if((blockCnt - 1) % 6 == 3){
                newBlock = Instantiate(block3) as GameObject;
            }
            else if ((blockCnt - 1) % 6 == 4){
                newBlock = Instantiate(block4) as GameObject;
            }
            else{
                newBlock = Instantiate(block5) as GameObject;
            }
            newBlock.transform.position = new Vector3(0, 1, block * blockCnt);
            newBlock.transform.localScale = new Vector3(4, 4, 4);
        }
    }
    public void playButtonClick(){
        startButton.gameObject.SetActive(false);
        StartCoroutine(Play());
        if(PersonalData.playMode.Equals("pacemaker")) {
            StartCoroutine(paceMakerPlay());
        } 
        else if(PersonalData.playMode.Equals("yesterday")) {

            StartCoroutine(playYesterday());
        }
    }
    public void SaveAnimationClip()
    {
        // 애니메이션 클립을 파일로 저장
        // 리스트를 JSON으로 직렬화하여 PlayerPrefs에 저장
        addState("end", Time.time, 1);
        ParseJson.StateDataList dataList = new ParseJson.StateDataList(recordState);
        string json = JsonUtility.ToJson(dataList);
        Debug.Log("json: "+json);
        PlayerPrefs.SetString("recordedState", json);
        PlayerPrefs.Save();

        PlayerPrefs.SetInt("PreviousScore", distanceM);
        PlayerPrefs.Save();
        StartCoroutine(ShowExitPopup());
    }

    IEnumerator ShowExitPopup()
    {
        finalDistanceText.text = "Final Distance: " + distanceM + "m";
        exitPopup.SetActive(true);
        yield return new WaitForSeconds(2);
        UnityEngine.SceneManagement.SceneManager.LoadScene("IntroScene");
    }
    
}
