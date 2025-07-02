using System;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public float speed = 10f;
    public int maxDestroy = 1;
    private int _destroyCount = 0;
    private MenuManager _menuManager;
    public int scorePerEnemy = 100;

    [Obsolete("Obsolete")]
    void Start()
    {
        _menuManager = FindObjectOfType<MenuManager>();
    }

    void Update()
    {
        transform.Translate(Vector3.up * (speed * Time.deltaTime));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            int enemyScore = scorePerEnemy;
            var enemyComponent = other.GetComponent<Enemy2>();
            if (enemyComponent != null)
            {
                enemyScore = enemyComponent.score;
            }
            Destroy(other.gameObject);
            if (_menuManager != null)
            {
                _menuManager.AddScore(enemyScore);
            }
            _destroyCount++;
            if (_destroyCount >= maxDestroy)
            {
                Destroy(gameObject);
            }
        }
    }
}
