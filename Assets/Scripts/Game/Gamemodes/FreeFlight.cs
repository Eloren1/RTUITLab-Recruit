using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFlight : Gamemode
{
    private int level;

    [SerializeField] private GameObject planePrefab;

    [SerializeField] private Transform[] spawns;

    public override void StartGame()
    {
        level = PlayerPrefs.GetInt("Level");

        SpawnPlane(planePrefab, spawns[level], Vector3.zero, 0);
    }
    public override void CompletedGame()
    {
        throw new System.NotImplementedException();
    }
}
