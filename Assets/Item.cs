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
        }
    }
}