using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private EnemySO enemySO;

    [SerializeField] private Transform[] spawnPoints;

    public void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoints[0].position, Quaternion.identity);
        newEnemy.GetComponent<Enemy>().OnSpawn(enemySO, GetComponent<CoreManager>().Core.transform);
    }

    private void Start()
    {
        SpawnEnemy(enemyPrefab);
    }
}
