using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class MidiTrackWindow : EditorWindow
{
    const float GridX = 0.5f;
    const float GridY = 30f;

    static private MidiAsset _midi;
    private Vector2 _trackListScroll;
    private Vector2 _noteAreaScroll;
    private bool[] _enableTracks;

    public static void ShowWindow(MidiAsset midi)
    {
        _midi = midi;
        EditorWindow.GetWindow(typeof(MidiTrackWindow), false, "Midi Track");
    }

    void OnGUI()
    {
        if (_midi == null)
            return;

        Rect rect;
        float titleHeight = 30f;
        float musicalScaleWidth = 60f;
        float timeHeight = 50f;
        float trackListWidth = position.width * 0.25f;

        // Draw Title Area
        rect = new Rect(0, 0, position.width, titleHeight);
        GUI.Box(rect, "");
        GUI.BeginGroup(rect);
        DrawTitleArea(rect.width, rect.height);
        GUI.EndGroup();

        // Draw Musical Scale Area
        rect = new Rect(0, titleHeight + timeHeight, musicalScaleWidth, position.height - titleHeight);
        GUI.Box(rect, "");
        GUI.BeginGroup(rect);
        DrawMusicalScaleArea(rect.width, rect.height);
        GUI.EndGroup();

        // Draw Track List Area
        rect = new Rect(position.width - trackListWidth, titleHeight, trackListWidth, position.height - titleHeight);
        GUI.Box(rect, "");
        GUILayout.BeginArea(rect);
        DrawTrackListArea();
        GUILayout.EndArea();

        // Draw Time Area
        rect = new Rect(musicalScaleWidth, titleHeight, position.width - musicalScaleWidth - trackListWidth, timeHeight);
        GUI.Box(rect, "");
        GUI.BeginGroup(rect);
        DrawTimeArea(rect.width, rect.height);
        GUI.EndGroup();

        // Draw Note Area
        rect = new Rect(musicalScaleWidth, titleHeight + timeHeight, position.width - musicalScaleWidth - trackListWidth, position.height - timeHeight - titleHeight);
        GUI.Box(rect, "");
        GUI.BeginGroup(rect);
        DrawNoteArea(rect.width, rect.height);
        GUI.EndGroup();
    }

    void DrawTitleArea(float width, float height)
    {
        Rect rect = new Rect(0, 0, width, height);
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(rect, Path.GetFileNameWithoutExtension(_midi.fileName), style);
    }

    void DrawMusicalScaleArea(float width, float height)
    {
        int sNote = (int)(_noteAreaScroll.y / GridY);
        int eNote = (int)((_noteAreaScroll.y + height) / GridY);
        float sY = -(_noteAreaScroll.y % GridY);

        Rect rect = new Rect(0, sY, width, GridY);

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;

        for (int i = sNote; i < eNote; i++)
        {
            GUI.Box(rect, "");
            GUI.Label(rect, NoteNumberToString(i), style);
            rect.y += GridY;
        }
    }

    void DrawTrackListArea()
    {
        if (_enableTracks == null)
        {
            _enableTracks = new bool[_midi.tracks.Length];

            for (int i = 0; i < _midi.tracks.Length; i++)
            {
                _enableTracks[i] = true;
            }
        }

        _trackListScroll = EditorGUILayout.BeginScrollView(_trackListScroll);

        for (int i = 0; i < _midi.tracks.Length; i++)
        {
            _enableTracks[i] = GUILayout.Toggle(_enableTracks[i], _midi.tracks[i].InstrumentName);
        }

        EditorGUILayout.EndScrollView();
    }

    void DrawTimeArea(float width, float height)
    {
        Rect areaRect = new Rect(0, 0, width, height);

        int sTime = (int)(_noteAreaScroll.x / GridX);
        int eTime = (int)((_noteAreaScroll.x + width) / GridX);

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleLeft;

        // Draw Text Area
        int sText = (int)(sTime / 100);
        int eText = (int)(eTime / 100);

        float textW = GridX * 100;
        float textH = height * 0.4f;
        float textX = -(_noteAreaScroll.x % textW);

        Rect textRect = new Rect(textX, 0, textW, textH);
        for (int i = sText; i <= eText; i++)
        {
            GUI.Label(textRect, string.Format("{0:f1}", i * 0.1f), style);
            textRect.x += textW;

        }

        // Draw Line Area
        int sLine = (int)(sTime / 10);
        int eLine = (int)(eTime / 10);
        float lineW = GridX * 10;
        float lineH = height - textH;
        float sLineX = -(_noteAreaScroll.x % lineW);

        Texture2D lineTexture = new Texture2D(1, 1);
        lineTexture.SetPixel(0, 0, Color.black);
        lineTexture.Apply();

        Rect pixelRect = new Rect(0, 0, 1, 1);
        int longLineY = (int)textH;
        int shortLineY = (int)(longLineY + lineH * 0.5f);
        int eLineY = (int)height;

        for (int i = sTime; i <= eTime; i++)
        {
            pixelRect.x = sLineX;
            if (i % 10 == 0) // Long Line
            {
                for (int j = longLineY; j <= eLineY; j++)
                {
                    pixelRect.y = j;
                    GUI.DrawTexture(pixelRect, lineTexture);
                }

                GUI.DrawTexture(pixelRect, lineTexture);
            }

            else // Short Line
            {
                for (int j = shortLineY; j <= eLineY; j++)
                {
                    pixelRect.y = j;
                    GUI.DrawTexture(pixelRect, lineTexture);
                }

                GUI.DrawTexture(pixelRect, lineTexture);
            }
            sLineX += lineW;
        }
    }

    void DrawNoteArea(float width, float height)
    {
        Rect viewRect = new Rect(0, 0, width, height);
        Rect contextRect = new Rect(0, 0, GridX * _midi.totalTime * 1000f, GridY * 128);
        _noteAreaScroll = GUI.BeginScrollView(viewRect, _noteAreaScroll, contextRect);

        Rect scrollRect = new Rect(_noteAreaScroll.x, _noteAreaScroll.y, width, height);

        for (int i = 0; i < _midi.tracks.Length; i++)
        {
            if(_enableTracks[i] == false)
                continue;

            MidiNote[] notes = _midi.tracks[i].Notes.ToArray();
            for (int j = 0; j < notes.Length; j++)
            {
                float sTime = notes[j].StartTime * _midi.pulseTime * 1000f;
                float eTime = notes[j].EndTime * _midi.pulseTime * 1000f;
                Rect noteRect = new Rect(GridX * sTime, GridY * notes[j].Number, GridX * (eTime - sTime), GridY);

                if (scrollRect.Overlaps(noteRect))
                    GUI.Box(noteRect, "");
            }
        }

        GUI.EndScrollView();
    }

    string NoteNumberToString(int number)
    {
        int index = number % 12;
        int octave = number / 12;

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