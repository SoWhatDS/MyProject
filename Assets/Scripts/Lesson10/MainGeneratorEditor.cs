using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(MainGeneration))]
public class MainGeneratorEditor : Editor
{
    private MainGeneration _mainGeneration;
    private string[] _AllSeedsInStache;
    public string path;

    public void OnEnable()
    {
        string path = "Assets/Resources/Seeds.txt";
        _mainGeneration = (MainGeneration)target;

        using (StreamReader sr = new StreamReader(path))
        {
            _AllSeedsInStache = sr.ReadToEnd().Split();

            for (int i = 0; i < _AllSeedsInStache.Length; i++)
            {
                _AllSeedsInStache[i] = _AllSeedsInStache[i].Replace("\r", "");
            }
        }
    }

    public override void OnInspectorGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.fontStyle = FontStyle.Bold;

        EditorGUILayout.LabelField("Параметры",style);
        EditorGUILayout.BeginVertical("box");
        _mainGeneration.LimitTasks = EditorGUILayout.IntField("Лимит задач", _mainGeneration.LimitTasks);
        _mainGeneration.XrOrigin = (GameObject)EditorGUILayout.ObjectField("XR Origin", _mainGeneration.XrOrigin, typeof(GameObject), false);
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Места респавна", style);
        for(int i = 0;i<_mainGeneration.RespawnPositions.Count;i++)
        {
            EditorGUILayout.BeginVertical("box");
            _mainGeneration.RespawnPositions[i] = (GameObject)EditorGUILayout.ObjectField("Префаб", _mainGeneration.RespawnPositions[i], typeof(GameObject), false);

            if (GUILayout.Button("Удалить элемент", GUILayout.Height(20)))
            {
                _mainGeneration.RespawnPositions.Remove(_mainGeneration.RespawnPositions[i]);
            }

            EditorGUILayout.EndVertical();
        }
        if (GUILayout.Button("Добавить точку респавна", GUILayout.Height(30)))
        {
            _mainGeneration.RespawnPositions.Add(null);
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Семена", style);
        if (_mainGeneration.Seeds.Count > 0)
        {
            for (int i = 0; i < _mainGeneration.Seeds.Count; i++)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("Овощ");
                int index = Array.IndexOf(_AllSeedsInStache, _mainGeneration.Seeds[i].Name);
                if (index != -1)
                {
                    _mainGeneration.Seeds[i].Name = _AllSeedsInStache[EditorGUILayout.Popup(Array.IndexOf(_AllSeedsInStache, _mainGeneration.Seeds[i].Name), _AllSeedsInStache)];
                }
                else
                {
                    _mainGeneration.Seeds[i].Name = _AllSeedsInStache[EditorGUILayout.Popup(0, _AllSeedsInStache)];
                }
                _mainGeneration.Seeds[i].Prefab = (GameObject)EditorGUILayout.ObjectField("Префаб", _mainGeneration.Seeds[i].Prefab, typeof(GameObject), false);
                if (GUILayout.Button("Удалить элемент", GUILayout.Height(20)))
                {
                    _mainGeneration.Seeds.Remove(_mainGeneration.Seeds[i]);
                }
                EditorGUILayout.EndVertical();
            }
        }
        else
        {
            EditorGUILayout.LabelField("Нет элементов в списке");
        }

        if (GUILayout.Button("Добавить элемент", GUILayout.Height(30)))
        {
            _mainGeneration.Seeds.Add(new Item());
        }

        EditorGUILayout.EndVertical();

        if (GUI.changed)
        {
            SetObjectDirty(_mainGeneration.gameObject);
        }
    }

    public static void SetObjectDirty(GameObject obj)
    {
        EditorUtility.SetDirty(obj);
        EditorSceneManager.MarkSceneDirty(obj.scene);
    }
}


