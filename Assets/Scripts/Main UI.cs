using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class MainUI : MonoBehaviour
{
    public static MainUI Instance;

    private TMP_Text _announcmentBox;
    [SerializeField] private float _messageTime=3f;
    private List<string> _messageQueue = new(); 

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
        _messageQueue.Add("test 1");
        _messageQueue.Add("test 2");
        _messageQueue.Add("test 3");
        _messageQueue.Add("test 4");
        _messageQueue.Add("test 5");
        StartCoroutine(ShowMessages());
    }

    private IEnumerator ShowMessages()
    {
        while(true)
        {
            string message = "";
            int maxNumMessages = Mathf.Min(3,_messageQueue.Count);
            if (maxNumMessages > 0)
            {
                for(int i = 0; i < maxNumMessages-1; i++)
                    message+=$"{_messageQueue[i]}\n";
                message+=_messageQueue[maxNumMessages-1];
                _messageQueue.RemoveRange(0,maxNumMessages);
            }
            _announcmentBox.text = message;
            yield return new WaitForSeconds(_messageTime);
        }
    }

    public static void EnqueueMessage(string message)
    {
        Instance._messageQueue.Add(message);
    }    

}
