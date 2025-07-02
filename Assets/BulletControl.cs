using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public int maxDestroy = 1;
    private int _destroyCount;

    private MenuManager _menuManager;
    private Rigidbody2D _rb;

    public float initialSpeed = 10f;
    public float acceleration = 10f;

    void Start()
    {
        _destroyCount = 0;
        
        _menuManager = FindAnyObjectByType<MenuManager>();
        _rb = GetComponent<Rigidbody2D>();
        if (_rb)
        {
            _rb.AddForce(transform.up * initialSpeed, ForceMode2D.Impulse);
        }
    }

    void Update()
    {
        if (_rb)
        {
            _rb.AddForce(transform.up * acceleration);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemyComponent = other.GetComponent<Enemy2>();
            if (enemyComponent != null)
            {
                int enemyScore = enemyComponent.score;
                Destroy(other.gameObject);
                if (_menuManager != null)
                {
                    _menuManager.AddScore(enemyScore);
                }
            }

            _destroyCount++;
            _menuManager.AddKill();
            if (_destroyCount >= maxDestroy)
            {
                Destroy(gameObject);
            }
        }

        if (other.CompareTag("EnemyBullet"))
        {
            var enemyBullet = other.GetComponent<EnemyBullet>();

            if (_menuManager != null)
            {
                _menuManager.AddScore(enemyBullet.destroyScore);
            }
            
            _destroyCount++;
            if (_destroyCount >= maxDestroy)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}