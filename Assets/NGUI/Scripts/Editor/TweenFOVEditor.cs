//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenFOV))]
public class TweenFOVEditor : UITweenerEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
		NGUIEditorTools.SetLabelWidth(120f);

		TweenFOV tw = target as TweenFOV;
		GUI.changed = false;

		float from = EditorGUILayout.Slider("From", tw.from, 1f, 180f);
		float to = EditorGUILayout.Slider("To", tw.to, 1f, 180f);
        bool camera2d = EditorGUILayout.Toggle("2DCamera", tw.camera2d);
		if (GUI.changed)
		{
			NGUIEditorTools.RegisterUndo("Tween Change", tw);
			tw.from = from;
			tw.to = to;
            tw.camera2d = camera2d;
			NGUITools.SetDirty(tw);
		}

		DrawCommonProperties();
	}
}
