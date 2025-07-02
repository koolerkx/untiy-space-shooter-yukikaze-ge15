using UnityEngine;

public class BackgroundControl : MonoBehaviour
{
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");
    public float scrollSpeed = 0.5f;
    private Material _mat;
    private Vector2 _offset = Vector2.zero;

    private Rigidbody2D _playerRb;

    void Start()
    {
        _mat = GetComponent<SpriteRenderer>().material;
        _playerRb = FindAnyObjectByType<PlayerControl>().GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_playerRb)
        {
            Vector2 playerVelocity = _playerRb.linearVelocity;
            _offset += playerVelocity * (-1f * scrollSpeed * Time.deltaTime);
        }

        _mat.SetTextureOffset(MainTex, _offset);
    }
}