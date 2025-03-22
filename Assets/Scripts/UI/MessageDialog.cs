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

    void Start()
    {
        for(int i = 0; i < messages.Length; i++)
        {
            string m = messages[i];
            if(m.Length == 0) m+=" ";
        }
    }

    public void AddMessages() => AnnouncmentBox.EnqueueMessage(messages);
}
