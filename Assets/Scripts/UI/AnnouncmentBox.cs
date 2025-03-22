using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class AnnouncmentBox : MonoBehaviour
{
    public static AnnouncmentBox Instance;

    private TMP_Text _announcmentBox;
    [SerializeField] private float _messageTime=3f;
    public float MessageTime {get;}
    [SerializeField] private int _maxNumMessages=1;
    public List<string> _messageQueue = new(); 

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        _announcmentBox = GetComponent<TMP_Text>();
        StartCoroutine(ShowMessages());
    }

    private IEnumerator ShowMessages()
    {
        while(true)
        {
            string message = "";
            int numMessages = Mathf.Min(_maxNumMessages,_messageQueue.Count);
            if (numMessages > 0)
            {
                for(int i = 0; i < numMessages-1; i++)
                    message+=$"{_messageQueue[i]}\n";
                message+=_messageQueue[numMessages-1];
                _messageQueue.RemoveRange(0,numMessages);
            }
            _announcmentBox.text = message;
            yield return new WaitForSeconds(_messageTime);
        }
    }

    public static void EnqueueMessage(string message)
    {
        Instance._messageQueue.Add(message);
    }
        
    public static void EnqueueMessage(string[] messages)
    {
        foreach(string msg in messages) EnqueueMessage(msg);
    }    

}
