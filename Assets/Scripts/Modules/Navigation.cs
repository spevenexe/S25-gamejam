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

        //Interact with controller to set the ship on course to the moon

        //While navigating, set nav multiplier to 1 because the ship is on course to the moon

    } 

}
