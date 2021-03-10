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

        // ���������� ������� � ������ �����
        planePrefab.transform.position = spawns[level].transform.position;
        planePrefab.transform.rotation = spawns[level].transform.rotation;
    }
    public override void CompletedGame()
    {
        throw new System.NotImplementedException();
    }
}
