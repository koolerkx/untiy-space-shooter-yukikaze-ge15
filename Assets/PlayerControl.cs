using System;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Bullet")] [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpawnOffset = 0.5f;

    [Header("Life and Broken State")] [SerializeField]
    public int maxLife = 4;

    [SerializeField] GameObject[] brokenStateChildObject;

    [Header("Movement")] [SerializeField] float rotationSpeed = 180f;
    [SerializeField] public float speed = 4;

    private MenuManager _menuManager;
    private Rigidbody2D _rigidbody2D;

    public GameObject throttleEffectSprite;

    int _currentLife;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _menuManager = FindAnyObjectByType<MenuManager>();

        _currentLife = maxLife;
        UpdateBroken();
    }

    void Update()
    {
        Vector2 velocity = _rigidbody2D.linearVelocity;

        if(_menuManager.gameState == GameState.InGame)
        {
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

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
            {
                SpawnBullet();
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _rigidbody2D.AddForce(input * speed);
        
        Vector3 effectScale = throttleEffectSprite.transform.localScale;
        effectScale.y = Mathf.Lerp(0f, 0.75f, input.magnitude);
        throttleEffectSprite.transform.localScale = effectScale;
        
        if (input.sqrMagnitude > 0.01f)
        {
            float inputAngle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg - 90f;
            float playerAngle = transform.eulerAngles.z;
            float relativeAngle = Mathf.DeltaAngle(playerAngle, inputAngle);
            
            if (Mathf.Abs(relativeAngle) < 5f) {
                throttleEffectSprite.transform.rotation = Quaternion.Euler(0, 0, playerAngle);
            } else {
                float clampedAngle = Mathf.Clamp(relativeAngle * -1, -25f, 25f);
                float effectAngle = playerAngle + clampedAngle;
                
                throttleEffectSprite.transform.rotation = Quaternion.Euler(0, 0, effectAngle);
            }
        }
    }

    void SpawnBullet()
    {
        if (!bulletPrefab) return;
        Vector3 spawnPos = transform.position + transform.up.normalized * bulletSpawnOffset;
        Instantiate(bulletPrefab, spawnPos, transform.rotation);
    }

    public void LoseLife(int damage = 1)
    {
        if (_currentLife > 0)
        {
            _currentLife -= damage;
            UpdateBroken();
            _menuManager.SetHpBar(Math.Max(_currentLife, 0) / (float)maxLife);
        }
        else
        {
            _menuManager.EndGame();
            Destroy(gameObject);
        }
    }

    void UpdateBroken()
    {
        for (int i = 0; i < brokenStateChildObject.Length; i++)
        {
            brokenStateChildObject[i].SetActive(i < (maxLife - _currentLife));
        }
    }
}