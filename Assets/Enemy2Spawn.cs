using UnityEngine;
using UnityEngine.Serialization;

public class Enemy2Spawn : MonoBehaviour
{
    public GameObject[] enemyPrefabs;

    public MenuManager menuManager;

    public float spawnInterval = 1f;
    public float spawnRadius = 20f;
    
    public Item[] itemPrefabs;
    public float itemSpawnInterval = 5.0f;
    public float itemSpawnRadius = 20f;

    private void FixedUpdate()
    {
        if (menuManager.gameState == GameState.InGame)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);

            Vector3 center = new Vector3(0, 0, 0);

            if (Time.time % spawnInterval < Time.fixedDeltaTime)
            {
                float angleRad = Random.Range(0f, Mathf.PI * 2f);
                Vector3 randomPosition =
                    center + new Vector3(Mathf.Cos(angleRad) * spawnRadius, Mathf.Sin(angleRad) * spawnRadius, 0);
                Instantiate(enemyPrefabs[randomIndex], randomPosition, Quaternion.identity);

                spawnInterval -= 0.001f;
            }
        }

        // Item Spawner
        if (menuManager.gameState == GameState.InGame)
        {
            int randomIndex = Random.Range(0, itemPrefabs.Length);

            Vector3 center = new Vector3(0, 0, 0);

            if (Time.time % itemSpawnInterval < Time.fixedDeltaTime)
            {
                float angleRad = Random.Range(0f, Mathf.PI * 2f);
                Vector3 randomPosition =
                    center + new Vector3(Mathf.Cos(angleRad) * itemSpawnRadius, Mathf.Sin(angleRad) * itemSpawnRadius, 0);
                Item item = Instantiate(itemPrefabs[randomIndex], randomPosition, Quaternion.identity);
                
                Vector3 toCenter = (center - randomPosition).normalized;
                
                float angleOffset = Random.Range(-30f, 30f);
                float baseAngle = Mathf.Atan2(toCenter.y, toCenter.x) * Mathf.Rad2Deg;
                float finalAngle = baseAngle + angleOffset;
                float rad = finalAngle * Mathf.Deg2Rad;
                Vector2 velocity = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                float speed = Random.Range(item.speedUpper, item.speedLower);
                
                Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
                if (rb)
                {
                    rb.linearVelocity = velocity.normalized * speed;
                }
            }
        }
    }
}