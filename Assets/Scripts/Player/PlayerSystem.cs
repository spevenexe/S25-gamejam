using UnityEngine;

public abstract class PlayerSystem : MonoBehaviour
{
    protected PData player;

    protected virtual void Awake()
    {
        player = transform.root.GetComponent<PData>();
    }
}
