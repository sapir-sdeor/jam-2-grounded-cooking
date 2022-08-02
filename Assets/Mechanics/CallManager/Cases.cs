using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class Cases : ScriptableObject
{
    public int outOfPaper = 1;
    public int numberOfPrefabsToCreate;
    public Vector3[] spawnPoints;
}