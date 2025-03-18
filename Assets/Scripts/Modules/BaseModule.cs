using System.Collections;
using UnityEngine;

public abstract class BaseModule : MonoBehaviour
{
    public bool isBroken = false;
    void Start()
    {
        
    }

    public abstract void BreakModule(Player player);
    public abstract void FixModule(Player player);
    protected abstract IEnumerator PlayFixingMinigame(Player player);
}
