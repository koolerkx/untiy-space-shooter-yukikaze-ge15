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

    public AudioManager audioManager;

    int _currentLife;

    private float _lastBulletTime = 0f;
    public float bulletCooldown = 0.1f;

    [Header("Item")] public int healAmount = 25;
    public float duration = 10.0f;

    public bool isRepeat = false;
    public float isRepeatDuration = 10.0f;
    private float _repeatCountDown;

    public bool isThrough = false;
    public float isThroughDuration = 10.0f;
    private float _throughCountDown;
    
    public bool isScale = false;
    public float isScaleDuration = 10.0f;
    private float _scaleCountDown;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _menuManager = FindAnyObjectByType<MenuManager>();

        _currentLife = maxLife;
    }

    void Update()
    {
        Vector2 velocity = _rigidbody2D.linearVelocity;

        if (_menuManager.gameState == GameState.InGame)
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

            float now = Time.time;
            bool canShoot = (now - _lastBulletTime) >= bulletCooldown;
            bool spawnIsRepeat = isRepeat && (Input.GetKey(KeyCode.Space) || Input.GetButton("Fire1"));
            bool spawnNotRepeat = Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1");
            if ((spawnIsRepeat || spawnNotRepeat) && canShoot)
            {
                SpawnBullet();
                _lastBulletTime = now;
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

            if (Mathf.Abs(relativeAngle) < 5f)
            {
                throttleEffectSprite.transform.rotation = Quaternion.Euler(0, 0, playerAngle);
            }
            else
            {
                float clampedAngle = Mathf.Clamp(relativeAngle * -1, -25f, 25f);
                float effectAngle = playerAngle + clampedAngle;

                throttleEffectSprite.transform.rotation = Quaternion.Euler(0, 0, effectAngle);
            }
        }

        if (_repeatCountDown > 0)
        {
            _repeatCountDown -= Time.deltaTime;
        }
        else
        {
            isRepeat = false;
        }
        _menuManager.SetRepeatTimerMask(1 - (_repeatCountDown / isRepeatDuration));

        if (_throughCountDown > 0)
        {
            _throughCountDown -= Time.deltaTime;
        }
        else
        {
            isThrough = false;
        }
        _menuManager.SetThroughTimerMask(1 - (_throughCountDown / isThroughDuration));
        
        if (_scaleCountDown > 0)
        {
            _scaleCountDown -= Time.deltaTime;
        }
        else
        {
            isScale = false;
        }
        _menuManager.SetScaleTimerMask(1 - (_scaleCountDown / isScaleDuration));
    }

    void SpawnBullet()
    {
        if (!bulletPrefab) return;
        Vector3 spawnPos = transform.position + transform.up.normalized * bulletSpawnOffset;
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, transform.rotation);
        var bulletControl = bullet.GetComponent<BulletControl>();
        if (bulletControl)
        {
            if(isScale)
            {
                bullet.transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
            }
            if(isThrough)
            {
                bulletControl.maxDestroy = 99;
            }
        }
    }

    public void LoseLife(int damage = 1)
    {
        audioManager.damage.Play();
        _currentLife -= damage;
        if (_currentLife > 0)
        {
            _menuManager.SetHpBar(Math.Max(_currentLife, 0) / (float)maxLife);
        }
        else
        {
            _menuManager.EndGame();
            Destroy(gameObject);
        }
    }

    public void HealLife(int amount)
    {
        _currentLife = Math.Min(_currentLife + amount, maxLife);

        _menuManager.SetHpBar(Math.Min(_currentLife, maxLife) / (float)maxLife);
    }

    public void SetRepeat(bool value)
    {
        isRepeat = value;
        _repeatCountDown = isRepeatDuration;
    }
    
    public void SetThrough(bool value)
    {
        isThrough = value;
        _throughCountDown = isThroughDuration;
    }
    
    public void SetScale(bool value)
    {
        isScale = value;
        _scaleCountDown = isScaleDuration;
    }

    public void ApplyItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Heal:
                HealLife(healAmount);
                break;
            case ItemType.Repeat:
                SetRepeat(true);
                break;
            case ItemType.Scale:
                SetScale(true);
                break;
            case ItemType.Through:
                SetThrough(true);
                break;
            case ItemType.None:
                break;
        }
    }
}