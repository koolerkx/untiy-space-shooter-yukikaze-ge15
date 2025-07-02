using UnityEngine;

public class Enemy2Spawn : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    
    public float spawnInterval = 1f;
    public float spawnRadius = 20f;

    private void FixedUpdate()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);

        Vector3 center = new Vector3(0, 0, 0);

        if (Time.time % spawnInterval < Time.fixedDeltaTime)
        {
            float angleRad = Random.Range(0f, Mathf.PI * 2f);
            Vector3 randomPosition =
                center + new Vector3(Mathf.Cos(angleRad) * spawnRadius, Mathf.Sin(angleRad) * spawnRadius, 0 );
            Instantiate(enemyPrefabs[randomIndex], randomPosition, Quaternion.identity);
        }
    }
}