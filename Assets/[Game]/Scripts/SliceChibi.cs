using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceChibi : MonoBehaviour
{
    public Mesh[] meshList;
    public GameObject cube;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    private Animator playerAnimator;
    private Mesh mesh;


    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Slice();
        }
    }

    public GameObject Slice()
    {
        AnimatorClipInfo[] animationClip = playerAnimator.GetCurrentAnimatorClipInfo(0);
        int frameNum = (int)(animationClip[0].weight * (animationClip[0].clip.length * animationClip[0].clip.frameRate));
        int currentFrame = (int)(playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime * (frameNum)) % frameNum;
        if (currentFrame >= meshList.Length) currentFrame = 0;
        GameObject obj = Instantiate(cube, transform.position, Quaternion.identity);
        obj.GetComponent<MeshFilter>().mesh = meshList[currentFrame];
        Destroy(gameObject);
        return obj;
    }

    public GameObject Slice2()
    {
        mesh = new Mesh();
        mesh = skinnedMeshRenderer.sharedMesh;
        mesh.RecalculateNormals();
        GameObject obj = Instantiate(cube, transform.position, Quaternion.identity);
        cube.GetComponent<MeshFilter>().mesh = mesh;
        Destroy(gameObject);
        return obj;
    }
}
