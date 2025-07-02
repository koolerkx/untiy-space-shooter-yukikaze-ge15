using System;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    private GameObject _player;

    [SerializeField] private float moveSpeed;

    public int score = 100;

    private void Start()
    {
        _player = GameObject.Find("Player");
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
    }
}