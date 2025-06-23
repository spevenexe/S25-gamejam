using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ending sequence. Ship breaks down.
/// </summary>
public class EndGame : MonoBehaviour
{

    public List<BaseModule> Modules;

    void Awake()
    {
        Modules = new();
    }

    void Start()
    {
        StartCoroutine(StartEjectionSequenceHelper());
    }

    private IEnumerator StartEjectionSequenceHelper()
    {
        foreach (BaseModule b in Modules)
        {
            AnnouncmentBox.EnqueueMessage($"{b.name} compromised. exit module".ToUpper());
            b.EjectModule();
            yield return new WaitForSeconds(b.EjectTime);
        }

        // screen shake

        // crash
        yield return new WaitForSeconds(5f);
        LevelLoader.Instance.LoadNext(LevelLoader.TransitionType.CUT);
    }
}
