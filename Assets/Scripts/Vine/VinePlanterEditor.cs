using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(VinePlanter))]

public class VinePlanterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        VinePlanter myScript = (VinePlanter)target;

        if (GUILayout.Button("Vine With Leafs"))
        {
            myScript.BuildObject();
        }

        if (GUILayout.Button("Vine No Leafs"))
        {
            myScript.BuildObjectNoLeafs();
        }
    }
}