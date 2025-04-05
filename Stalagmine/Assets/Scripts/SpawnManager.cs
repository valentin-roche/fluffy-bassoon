using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public Transform SpawnParent;
    [Serializable]
    struct SpawnPoint
    {
        public Transform point;
        public Vector2 size;
    }

    [SerializeField] private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    public void SpawnEnemy(EnemySO enemySO)
    {
        SpawnPoint sp = spawnPoints[Random.Range(0, spawnPoints.Count)];

        Vector3 spawnPointDif = new Vector3(Random.Range(-sp.size.x, sp.size.x), 0, Random.Range(-sp.size.y, sp.size.y));

        GameObject newEnemy = Instantiate(enemySO.Prefab, sp.point.position + spawnPointDif, Quaternion.identity, SpawnParent);

        newEnemy.GetComponent<Enemy>().OnSpawn(enemySO, GetComponent<CoreManager>().Core.gameObject.transform);
    }
}
