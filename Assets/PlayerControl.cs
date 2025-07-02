using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 180f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpawnOffset = 0.5f; 
    [SerializeField] GameObject[] brokenStateChildObject;
    [SerializeField] int maxLife = 4;
    private Rigidbody2D _rigidbody2D;
    int _currentLife;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _currentLife = maxLife;
        UpdateLifeIcons();
    }

    void Update()
    {
        Vector2 velocity = _rigidbody2D.linearVelocity;

        if (velocity.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;

            float angle = Mathf.MoveTowardsAngle(
                transform.eulerAngles.z,
                targetAngle,
                rotationSpeed * Time.deltaTime
            );

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBullet();
        }
    }

    void SpawnBullet()
    {
        if (!bulletPrefab) return;
        Vector3 spawnPos = transform.position + transform.up.normalized * bulletSpawnOffset;
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, transform.rotation);
    }

    public void LoseLife()
    {
        if (_currentLife > 0)
        {
            _currentLife--;
            Debug.Log($"Current Life: {_currentLife}");
            UpdateLifeIcons();
        }
    }

    void UpdateLifeIcons()
    {
        for (int i = 0; i < brokenStateChildObject.Length; i++)
        {
            Debug.Log("brokenStateChildObject[i]");
            // TODO
            brokenStateChildObject[i].SetActive(i < (maxLife - _currentLife));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            LoseLife();
        }
    }
}
