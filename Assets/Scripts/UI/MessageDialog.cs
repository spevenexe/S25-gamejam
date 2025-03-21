using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDialog : MonoBehaviour
{
    [SerializeField] private string[] messages;

    private void Awake()
    {
        if (messages == null || messages.Length == 0) Debug.LogWarning($"No messages found at {gameObject}.");
    }

    public void AddMessages() => AnnouncmentBox.EnqueueMessage(messages);
}
