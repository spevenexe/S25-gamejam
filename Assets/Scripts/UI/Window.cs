using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] private Vector3 _finalScale=Vector3.one;
    private Vector3 _initScale;

    void Awake()
    {
        _initScale = transform.localScale;
    }

    public void SetScale(float timerProgress)
    {
        if(timerProgress > 1 || timerProgress < float.Epsilon) return;
        transform.localScale=_initScale + timerProgress * (_finalScale-_initScale);
    }
}