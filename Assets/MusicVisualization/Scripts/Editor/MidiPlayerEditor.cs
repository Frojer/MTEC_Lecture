using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MidiPlayer))]
public class MidiPlayerEditor : Editor
{
    SerializedProperty midi;
    SerializedProperty music;
    SerializedProperty audioSource;
    SerializedProperty playDelayTime;
    SerializedProperty playSpeed;

    void OnEnable()
    {
        midi = serializedObject.FindProperty("midi");
        music = serializedObject.FindProperty("music");
        audioSource = serializedObject.FindProperty("audioSource");
        playDelayTime = serializedObject.FindProperty("playDelayTime");
        playSpeed = serializedObject.FindProperty("playSpeed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(midi);
        EditorGUILayout.PropertyField(music);
        EditorGUILayout.PropertyField(audioSource);
        EditorGUILayout.PropertyField(playDelayTime);
        EditorGUILayout.PropertyField(playSpeed);

        serializedObject.ApplyModifiedProperties();

        MidiPlayer midiPlayer = (MidiPlayer)target;

        if (Application.isPlaying)
        {
            GUILayout.BeginHorizontal();

            if (midiPlayer.isPlaying)
            {
                if (GUILayout.Button("Pause"))
                {
                    midiPlayer.Pause();
                }

                EditorUtility.SetDirty(target);
            }

            else
            {
                if (midiPlayer.playTime == 0.0f)
                {
                    if (GUILayout.Button("Play"))
                    {
                        midiPlayer.Play();
                    }
                }

                else
                {
                    if (GUILayout.Button("Resume"))
                    {
                        midiPlayer.Resume();
                    }
                }
            }

            if (GUILayout.Button("Stop"))
            {
                midiPlayer.Stop();
            }

            GUILayout.EndHorizontal();

            GUILayout.HorizontalSlider(midiPlayer.playTime, 0.0f, midiPlayer.totalTime);
            EditorGUILayout.LabelField(string.Format("Time: {0:f1}sec", midiPlayer.playTime));
        }
    }
}