using System;
using UnityEngine;

public enum ItemType
{
    Heal,
    Scale,
    Repeat,
    Through,
    None
}

public class Item : MonoBehaviour
{
    public ItemMessage messageBox;

    [SerializeField] public ItemType type = ItemType.None;

    public float speedUpper = 1.0f; 
    public float speedLower = 3.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControl player = other.GetComponent<PlayerControl>();
            player.ApplyItem(type);

            if (messageBox != null)
            {
                MessageList panel = FindAnyObjectByType<MessageList>();
                if (panel != null)
                {
                    Instantiate(messageBox, panel.transform, false);
                }
            }
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