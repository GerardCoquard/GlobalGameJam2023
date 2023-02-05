using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class VineTree{
    public VinePlanter planter;

    public float branchThickness => planter.branchThickness;
    public float branchWidth => planter.branchWidth;

    public float LeafAmount => planter.leafAmount;

    public GameObject LeafPrefab => planter.leafPrefabs[Random.Range(0, planter.leafPrefabs.Length)];

    public Vector3 origin, normal;

    List<VineBranch> branches = new List<VineBranch>();

    public VineTree(Vector3 origin, Vector3 normal, VinePlanter planter, int numberOfBranches) {
        this.origin = origin;
        this.normal = normal;

        this.planter = planter;

        for (int i = 0; i < numberOfBranches; i++)
        {
            VineBranch branch = new VineBranch(this);
            branches.Add(branch);
        }
    }

    public void Redraw() {
        foreach (VineBranch branch in branches)
        {
            branch.Redraw();
        }
    }

    public void Grow(int rnd, bool leafs, int maxDistance) {
        foreach(VineBranch branch in branches) {
            branch.Grow(rnd, leafs,maxDistance);
        }
    }

    public void DrawGizmos() {
        foreach(VineBranch branch in branches) {
            branch.DrawGizmos();
        }
    }
}