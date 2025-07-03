using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    private GameObject _player;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private float moveSpeed;

    public int score = 100;

    public int damage = 1;
    
    [Header("Bullet Settings")]
    [SerializeField, Range(1, 10)] private int bulletPerShoot = 3;
    [SerializeField, Range(0.1f, 1.0f)] private float bulletPerShootRate = 0.5f;
    [SerializeField, Range(1f, 20f)] private float shootRate = 3f;
    [SerializeField, Range(0.1f, 2f)] private float bulletSpawnOffset = 1f;

    private float _bulletTimer;
    private int _bulletsFired;
    private bool _isFiring;

    private float _shootRateTimer;

    public AudioManager audioManager;

    private void Start()
    {
        _player = GameObject.Find("Player");
        _isFiring = false;
        _bulletTimer = 0f;
        _bulletsFired = 0;
        _shootRateTimer = 0f;
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    void FixedUpdate()
    {
        if (_player)
        {
            Vector3 playerPosition = _player.transform.position;
            Vector2 direction = playerPosition - transform.position;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
            transform.position = Vector3.MoveTowards(
                transform.position,
                playerPosition,
                moveSpeed * Time.deltaTime
            );
        }

        _shootRateTimer += Time.fixedDeltaTime;
        if (!_isFiring && _shootRateTimer >= shootRate)
        {
            _isFiring = true;
            _bulletsFired = 0;
            _bulletTimer = 0f;
            _shootRateTimer = 0f;
        }

        if (_isFiring && bulletPrefab)
        {
            _bulletTimer += Time.fixedDeltaTime;
            if (_bulletsFired < bulletPerShoot && _bulletTimer >= bulletPerShootRate)
            {
                Vector3 spawnPos = transform.position + transform.up.normalized * bulletSpawnOffset;
                Instantiate(bulletPrefab, spawnPos, transform.rotation);
                _bulletsFired++;
                _bulletTimer = 0f;
            }

            if (_bulletsFired >= bulletPerShoot)
            {
                _isFiring = false;
                _shootRateTimer = 0;
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerControl>();
        if (player)
        {
            audioManager.kill.Play();
            player.LoseLife(damage);
            Destroy(gameObject);
        }
    }
}