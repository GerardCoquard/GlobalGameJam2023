using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinePlanter : MonoBehaviour
{
    public GameObject[] leafPrefabs;

    public float leafAmount = 0.06f;
    public int numberOfBranches = 5;
    public int maxDistance = 10;

    public Material vineTopMat, vineBottomMat;

    public float branchThickness = 0.1f;
    public float branchWidth = 0.5f;
    public float angleNormal = 0f;

    List<VineTree> trees = new List<VineTree>();




    public void BuildObject()
    {

        StartCoroutine(Photo(true));
    }

    public void BuildObjectNoLeafs()
    {

        StartCoroutine(Photo(false));
    }
    IEnumerator Photo(bool leafs)
    {
        Ray ray = new Ray(UnityEditor.SceneView.lastActiveSceneView.camera.transform.position, UnityEditor.SceneView.lastActiveSceneView.camera.transform.forward);

        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {

            int rnd = Random.Range(0, 5000);
            GameObject parent = new GameObject("Vine" + rnd);
            parent.transform.position = hit.point;
            parent.transform.SetParent(transform);
            VineTree tree = new VineTree(origin: hit.point, normal: hit.normal, planter: this,numberOfBranches);
            trees.Add(tree);

            tree.Grow(rnd, leafs, maxDistance);
        }
        yield return null;
    }
}
