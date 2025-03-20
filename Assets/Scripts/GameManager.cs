using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float yBottom_box;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this); 
    }
}
