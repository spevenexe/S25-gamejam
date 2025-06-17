using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A simple game manager that handles static references and other events. 
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private PlayerInput _playerInput;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        if (_playerInput == null) Utils.getPlayer()?.GetComponent<PlayerInput>();
        Interactable.SetPI(_playerInput);
    }
}
