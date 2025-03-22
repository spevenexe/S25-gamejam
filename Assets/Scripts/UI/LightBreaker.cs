using System.Collections;
using UnityEngine;

// end screen. Just flash the lights
public class LightBreaker : MonoBehaviour
{
    [SerializeField] protected float _lightFlashInterval = 2f; 
    private ModuleLights moduleLights;
    void Start()
    {
        moduleLights = GetComponent<ModuleLights>();
        StartCoroutine(FlashLights());
    }

    public IEnumerator FlashLights()
    {
        yield return new WaitForSeconds(_lightFlashInterval);
        while(true)
        {
            moduleLights.TurnOff();
            yield return new WaitForSeconds(_lightFlashInterval);
            moduleLights.RestoreColor();
            yield return new WaitForSeconds(_lightFlashInterval);
        }
    }
}
