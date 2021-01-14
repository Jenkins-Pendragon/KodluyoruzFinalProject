using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceChibi : MonoBehaviour
{
    public Mesh[] meshList;
    public GameObject cube;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Transform spawnPoint;
    private Animator playerAnimator; 

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    } 

    public GameObject Slice()
    {
        AnimatorClipInfo[] animationClip = playerAnimator.GetCurrentAnimatorClipInfo(0);
        Debug.Log(animationClip[0].clip.name);
        int frameNum = (int)(animationClip[0].weight * (animationClip[0].clip.length * animationClip[0].clip.frameRate));
        int currentFrame = (int)(playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime * (frameNum)) % frameNum;
        if (currentFrame >= meshList.Length) currentFrame = 0;
        GameObject obj = Instantiate(cube, spawnPoint.position, transform.rotation);
        obj.GetComponent<MeshFilter>().mesh = meshList[currentFrame];
        gameObject.SetActive(false);
        return obj;
    }    
}
