﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGeneration : MonoBehaviour
{
    public int LimitTasks = 3;
    public GameObject XrOrigin;
    public List<GameObject> RespawnPositions;
    public List<string> SeedsNames;
    public List<GameObject> SeedsPrefabs;
    public List<Item> Seeds;
    public List<int> CurrentTasks;
    public List<int> DoneTasks;
}

[Serializable]
public class Item
{
    public string Name;
    public GameObject Prefab;
}
