using UnityEngine;
using UnityEditor;
using Assets.Scripts.Logic.UI.CommonUIs;

/// <summary>
/// Inspector class used to edit UITextureAnimation.
/// </summary>

[CanEditMultipleObjects]
[CustomEditor(typeof(UITextureAnimation))]
public class UITextrueAnimationInspector : Editor
{
    /// <summary>
    /// Draw the inspector widget.
    /// </summary>

    public override void OnInspectorGUI()
    {
        GUILayout.Space(3f);
        NGUIEditorTools.SetLabelWidth(80f);
        serializedObject.Update();

        NGUIEditorTools.DrawProperty("Start Frame Index", serializedObject, "startFrameIndex");
        NGUIEditorTools.DrawProperty("End Frame Index", serializedObject, "endFrameIndex");
        NGUIEditorTools.DrawProperty("Framerate", serializedObject, "mFPS");
        NGUIEditorTools.DrawProperty("Name Prefix", serializedObject, "mPrefix");
        NGUIEditorTools.DrawProperty("Loop", serializedObject, "mLoop");
        NGUIEditorTools.DrawProperty("Pixel Snap", serializedObject, "mSnap");
        NGUIEditorTools.DrawProperty("Auto Play", serializedObject, "mActive");

        serializedObject.ApplyModifiedProperties();
    }
}
