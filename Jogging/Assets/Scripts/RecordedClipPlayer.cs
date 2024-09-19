using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecordedClipPlayer : MonoBehaviour
{
    public string animationClipPath = "Assets/Animations/RecordedAnimation.anim"; // 애니메이션 파일 경로
    private AnimationClip recordedClip;

    void Start()
    {
        if(File.Exists(animationClipPath)) {
            recordedClip = Resources.Load<AnimationClip>(Path.GetFileNameWithoutExtension(animationClipPath));
            if(recordedClip != null) {
                var anim = gameObject.AddComponent<Animation>();
                anim.AddClip(recordedClip, "RecordedAnimation");
                anim.Play("RecordedAnimation");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
