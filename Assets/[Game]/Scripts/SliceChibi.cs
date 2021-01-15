using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceChibi : MonoBehaviour
{
    public List<Mesh> runningMeshList;
    public List<Mesh> shootingMeshList;
    public List<Mesh> tauntMeshList;
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
        List<Mesh> meshList;
        AnimatorClipInfo[] animationClip = playerAnimator.GetCurrentAnimatorClipInfo(0);
        string animationName = animationClip[0].clip.name;
        int frameNum = (int)(animationClip[0].weight * (animationClip[0].clip.length * animationClip[0].clip.frameRate));
        int currentFrame = (int)(playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime * (frameNum)) % frameNum;

        if (animationName == "Running") meshList = runningMeshList;
        else if (animationName == "Shooting" || animationName == "DefaultStateShooting") meshList = shootingMeshList;
        else if(animationName == "Taunt") meshList = tauntMeshList;
        else meshList = runningMeshList;

        if (currentFrame >= meshList.Count) currentFrame = 0;
        GameObject obj = Instantiate(cube, spawnPoint.position, transform.rotation);
        obj.GetComponent<MeshFilter>().mesh = meshList[currentFrame];
        gameObject.SetActive(false);
        return obj;
    }    
}
