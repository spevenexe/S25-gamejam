using UnityEngine;

public class Navigation : MonoBehaviour
{
    [SerializeField] private float _navStartMultiplier;
    [SerializeField] private float _navEndMultiplier;
    private float _navMultiplier;

    public void SetNavMultipler(float timerProgress)
    {
        _navMultiplier = timerProgress * (_navEndMultiplier - _navStartMultiplier) + _navStartMultiplier;
    }

    // TODO Keianna
    private void Navigate()
    {

    } 
}
