using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections;

[CustomEditor(typeof(MidiEventTrigger))]
public class MidiEventTriggerEditor : Editor
{
    private bool foldout = false;
    private bool foldout2 = false;

    void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        MidiEventTrigger trigger = (MidiEventTrigger)target;

        foldout = EditorGUILayout.Foldout(foldout, "Instrument Filter");

        if (foldout)
        {
            if (GUILayout.Button("Check All"))
            {
                for (int i = 0; i < 129; i++)
                    trigger.instrumentFilter[i] = true;
            }

            if (GUILayout.Button("Clear All"))
            {
                for (int i = 0; i < 129; i++)
                    trigger.instrumentFilter[i] = false;
            }

            for (int i = 0; i < 129; i++)
            {
                bool newValue = GUILayout.Toggle(trigger.instrumentFilter[i], MidiFile.Instruments[i]);
                if (newValue != trigger.instrumentFilter[i])
                {
                    trigger.instrumentFilter[i] = newValue;

                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                }
            }
        }


        foldout2 = EditorGUILayout.Foldout(foldout2, "Instrument Filter");

        if (foldout2)
        {
            if (GUILayout.Button("Check All"))
            {
                for (int i = 0; i < 128; i++)
                    trigger.noteFilter[i] = true;
            }

            if (GUILayout.Button("Clear All"))
            {
                for (int i = 0; i < 128; i++)
                    trigger.noteFilter[i] = false;
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("C"))
            {
                for (int i = 0; i < 128; i += 12)
                    trigger.noteFilter[i] = true;
            }
            if (GUILayout.Button("C#"))
            {
                for (int i = 1; i < 128; i += 12)
                    trigger.noteFilter[i] = true;
            }
            if (GUILayout.Button("D"))
            {
                for (int i = 2; i < 128; i += 12)
                    trigger.noteFilter[i] = true;
            }
            if (GUILayout.Button("D#"))
            {
                for (int i = 3; i < 128; i += 12)
                    trigger.noteFilter[i] = true;
            }
            if (GUILayout.Button("E"))
            {
                for (int i = 4; i < 128; i += 12)
                    trigger.noteFilter[i] = true;
            }
            if (GUILayout.Button("F"))
            {
                for (int i = 5; i < 128; i += 12)
                    trigger.noteFilter[i] = true;
            }
            if (GUILayout.Button("F#"))
            {
                for (int i = 6; i < 128; i += 12)
                    trigger.noteFilter[i] = true;
            }
            if (GUILayout.Button("G"))
            {
                for (int i = 7; i < 128; i += 12)
                    trigger.noteFilter[i] = true;
            }
            if (GUILayout.Button("G#"))
            {
                for (int i = 8; i < 128; i += 12)
                    trigger.noteFilter[i] = true;
            }
            if (GUILayout.Button("A"))
            {
                for (int i = 9; i < 128; i += 12)
                    trigger.noteFilter[i] = true;
            }
            if (GUILayout.Button("A#"))
            {
                for (int i = 10; i < 128; i += 12)
                    trigger.noteFilter[i] = true;
            }
            if (GUILayout.Button("B"))
            {
                for (int i = 11; i < 128; i += 12)
                    trigger.noteFilter[i] = true;
            }
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < 128; i++)
            {
                bool newValue = GUILayout.Toggle(trigger.noteFilter[i], NoteNumberToString(i));
                if (newValue != trigger.noteFilter[i])
                {
                    trigger.noteFilter[i] = newValue;

                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                }
            }
        }
    }

    string NoteNumberToString(int number)
    {
        int index = (int)(number % 12);
        int octave = (int)(number / 12);

        switch (index)
        {
            case 0:
                return string.Format("C{0:d}", octave);

            case 1:
                return string.Format("C#{0:d}", octave);

            case 2:
                return string.Format("D{0:d}", octave);

            case 3:
                return string.Format("D#{0:d}", octave);

            case 4:
                return string.Format("E{0:d}", octave);

            case 5:
                return string.Format("F{0:d}", octave);

            case 6:
                return string.Format("F#{0:d}", octave);

            case 7:
                return string.Format("G{0:d}", octave);

            case 8:
                return string.Format("G#{0:d}", octave);

            case 9:
                return string.Format("A{0:d}", octave);

            case 10:
                return string.Format("A#{0:d}", octave);

            case 11:
                return string.Format("B{0:d}", octave);
        }

        return "Unknown";
    }
}
