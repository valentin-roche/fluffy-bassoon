using Grids;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public EnemyGroupSO[] enemyGroups;

    [SerializeField] int waveValue = 10;

    public void SpawnWave(int waveValue)
    {
        int spawnedValue = 0;
        List<EnemySO> enemyList = new();

        waveValue += GetComponentInChildren<GridTransition>().layerLevel * 5;

        while(spawnedValue < waveValue)
        {
            EnemyGroupSO randomGroup = enemyGroups[Random.Range(0, enemyGroups.Length)];

            if (randomGroup.groupValue + spawnedValue > waveValue) continue;

            foreach(EnemySO enemy in randomGroup.Enemies)
            {
                enemyList.Add(enemy);
            }

            spawnedValue += randomGroup.groupValue;
        }

        SpawnManager sm = GetComponent<SpawnManager>();

        foreach(EnemySO enemy in enemyList)
        {
            sm.SpawnEnemy(enemy);
        }
    }

    //IEnumerator SpawnCoroutine(EnemySO enemy)
    //{

    //}

    public void StartWave()
    {
        SpawnWave(waveValue);
    }
}
