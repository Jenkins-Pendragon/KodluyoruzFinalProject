using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

public class BakeMesh : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    private Mesh mesh;
    private int num = 0;
    public Animator playerAnimator;


    private void Update()
    {
        //AnimatorClipInfo[] animationClip = playerAnimator.GetCurrentAnimatorClipInfo(0);
        //int currentFrame = (int)(animationClip[0].weight * (animationClip[0].clip.length * animationClip[0].clip.frameRate));
        //Debug.Log(((int)(playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime * (currentFrame))) % currentFrame);        
    }

    [Button]
    private void GetMesh()
    {
        mesh = new Mesh();
        skinnedMeshRenderer.BakeMesh(mesh);
        mesh.name = "taunt";
        mesh.RecalculateNormals();
        AssetDatabase.CreateAsset(mesh, "Assets/Mesh/taunt" + num + ".asset");
        AssetDatabase.SaveAssets();
        num++;
    }

    [Button]
    private void ResetNum()
    {
        num = 0;
    }
}
