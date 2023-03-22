using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestWin : EditorWindow
{
    private MainGeneration _main;
    private Editor _mainWindow;
    [MenuItem("Window/MainGenerator")]

    public static void Show()
    {
        EditorWindow.GetWindow<TestWin>();
    }

    public void OnGUI()
    {
        _main = (MainGeneration)EditorGUILayout.ObjectField(_main, typeof(MainGeneration));
        if (_main != null)
        {
            _mainWindow = Editor.CreateEditor(_main);
            _mainWindow.OnInspectorGUI();
        }
    }
}
