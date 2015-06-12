using UnityEngine;
using System.Collections;
using UnityEditor;

//[CustomEditor(typeof(Cube))]
public class CubeEditor : Editor {
    public override void OnInspectorGUI()
    {
        Cube cube = target as Cube;
        EditorGUI.BeginChangeCheck();
        cube.cubeColor = (EColor)EditorGUILayout.EnumPopup("颜色", cube.cubeColor);
        if (EditorGUI.EndChangeCheck())
        {
            SpriteRenderer sr = cube.GetComponent<SpriteRenderer>();
            if (cube.cubeColor == EColor.B)
            {
                sr.color = Color.black;
            }
            else
            {
                sr.color = Color.white;
            }
        }
    }
}
