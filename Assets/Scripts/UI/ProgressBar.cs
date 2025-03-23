using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    void Awake()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        if(slider == null) slider = GetComponentInChildren<Slider>();
    }

    public void SetProgress(float progress)
    {
        progress = Mathf.Clamp(progress,0,1);
        slider.value = progress * slider.maxValue;
    }
}
