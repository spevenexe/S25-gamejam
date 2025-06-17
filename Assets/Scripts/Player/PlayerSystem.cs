using UnityEngine;

/// <summary>
/// Generic class that allows reference to a player data class. <c>PlayerSystems</c> on the same object will reference the same data object.
/// </summary>
[RequireComponent(typeof(PData))]
public abstract class PlayerSystem : MonoBehaviour
{
    protected PData _player;

    protected virtual void Awake()
    {
        _player = transform.root.GetComponent<PData>();
    }
}
