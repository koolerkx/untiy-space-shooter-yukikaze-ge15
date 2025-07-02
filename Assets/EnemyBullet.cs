using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float initialSpeed = 10f;
    public int damage = 1;

    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (_rb)
        {
            _rb.AddForce(transform.up * initialSpeed, ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerControl>();
        if (player)
        {
            player.LoseLife(damage);
            Destroy(gameObject);
        }
        if (other.CompareTag("PlayerBullet"))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("VisibleArea"))
        {
            Destroy(gameObject);
        }
    }
}