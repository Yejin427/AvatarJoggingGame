using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsInfo : MonoBehaviour
{
    public GameObject woman;
    public GameObject man;
    public TMP_Dropdown genderDropdown;
    public TMP_InputField heightInput;
    public TMP_InputField upperBodyLengthInput;
    public TMP_InputField armLengthInput;
    //public TMP_InputField shoulderLengthInput;
    public Animator animator;

    const float womanHeight = 175.0f;
    const float womanArmLength = 53.8f;
    const float womanBodyLength = 53.8f;

    // Start is called before the first frame update
    void Start()
    {
        genderDropdown.onValueChanged.AddListener(delegate { onGenderToggle(); });
    }

    public void onGenderToggle()
    {
        int dropdownValue = genderDropdown.value;
        if(dropdownValue == 0) {
            woman.SetActive(false);
            man.SetActive(true);
        } else {
            woman.SetActive(true);
            man.SetActive(false);
        }
    }

    public void onSaveButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CalibrationScene");
    }
    public void onAvatarFixButtonClick()
    {
        PersonalData.height = float.Parse(heightInput.text);
        PersonalData.upperBodyLength = float.Parse(upperBodyLengthInput.text);
        PersonalData.armLength = float.Parse(armLengthInput.text);
        Transform rightUpperArm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
        Transform rightLowerArm = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
        Transform leftUpperArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        Transform leftLowerArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        rightUpperArm.localScale = new Vector3(rightUpperArm.localScale.x, rightUpperArm.localScale.y * (PersonalData.armLength / womanArmLength), rightUpperArm.localScale.z);
        rightLowerArm.localScale = new Vector3(rightLowerArm.localScale.x, rightLowerArm.localScale.y * (PersonalData.armLength / womanArmLength), rightLowerArm.localScale.z);
        leftUpperArm.localScale = new Vector3(leftUpperArm.localScale.x, leftUpperArm.localScale.y * (PersonalData.armLength / womanArmLength), leftUpperArm.localScale.z);
        leftLowerArm.localScale = new Vector3(leftLowerArm.localScale.x, leftLowerArm.localScale.y * (PersonalData.armLength / womanArmLength), leftLowerArm.localScale.z);

        //upper body
        Transform spine = animator.GetBoneTransform(HumanBodyBones.Spine);
        Transform chest = animator.GetBoneTransform(HumanBodyBones.Chest);
        Transform upperChest = animator.GetBoneTransform(HumanBodyBones.UpperChest);
        spine.localScale = new Vector3(spine.localScale.x, spine.localScale.y * (PersonalData.upperBodyLength / womanBodyLength), spine.localScale.z);
        chest.localScale = new Vector3(chest.localScale.x, chest.localScale.y * (PersonalData.upperBodyLength / womanBodyLength), chest.localScale.z);
        upperChest.localScale = new Vector3(upperChest.localScale.x, upperChest.localScale.y * (PersonalData.upperBodyLength / womanBodyLength), upperChest.localScale.z);

        //leg
        Transform leftUpperLeg = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        Transform rightUpperLeg = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
        Transform leftLowerLeg = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
        Transform rightLowerLeg = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
        leftUpperLeg.localScale = new Vector3(leftUpperLeg.localScale.x, leftUpperLeg.localScale.y * (PersonalData.height / womanHeight), leftUpperLeg.localScale.z);
        rightUpperLeg.localScale = new Vector3(rightUpperLeg.localScale.x, rightUpperLeg.localScale.y * (PersonalData.height / womanHeight), rightUpperLeg.localScale.z);
        leftLowerLeg.localScale = new Vector3(leftLowerLeg.localScale.x, leftLowerLeg.localScale.y * (PersonalData.height / womanHeight), leftLowerLeg.localScale.z);
        rightLowerLeg.localScale = new Vector3(rightLowerLeg.localScale.x, rightLowerLeg.localScale.y * (PersonalData.height / womanHeight), rightLowerLeg.localScale.z);
    }
}
