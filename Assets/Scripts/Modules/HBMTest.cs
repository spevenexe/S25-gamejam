using System.Collections;
using UnityEditor;
using UnityEngine;

public class HBMTest : MonoBehaviour
{
    private float _breachTimerAveragePercentage;

    public void RunTests() => StartCoroutine(RunTestsHelper());

    public IEnumerator RunTestsHelper()
    {
        for (int i = 0; i < 100; i++)
        {
            // get random percentage
            float breachTimerPercentage = Random.Range(0f, 1f);

            if (TestTimer(breachTimerPercentage))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }

            // adjust breachTimerAveragePercentage to new average
            // _breachTimerAveragePercentage = Mathf.Abs(breachTimerPercentage - 1);

            yield return null;
        }
    }


    private bool TestTimer(float breachTimerPercentage)
    {
        float newP1 = StartNewBreachTimer(breachTimerPercentage);
        float newP2 = StartNewBreachTimer2(breachTimerPercentage);

        return (newP1 - newP2) < .001f;
    }

    private float StartNewBreachTimer(float breachTimerPercentage)
    {
        // create additional variables
        float breachTimerPercentageOfAverageTimer;
        float breachTimerPercentageToAdd;
        float breachNewTimerPercentage;

        // adjust breachTimerPercentage based on breachTimerAveragePercentage
        if (breachTimerPercentage < _breachTimerAveragePercentage)
        {
            breachTimerPercentageOfAverageTimer = breachTimerPercentage / _breachTimerAveragePercentage;
            breachTimerPercentageToAdd = breachTimerPercentageOfAverageTimer * (_breachTimerAveragePercentage - breachTimerPercentage);
            breachNewTimerPercentage = breachTimerPercentage + breachTimerPercentageToAdd;
        }
        else if (breachTimerPercentage > _breachTimerAveragePercentage)
        {
            breachTimerPercentageOfAverageTimer = Mathf.Abs((breachTimerPercentage - _breachTimerAveragePercentage) / (1 - _breachTimerAveragePercentage) - 1);
            breachTimerPercentageToAdd = -(breachTimerPercentageOfAverageTimer * (breachTimerPercentage - _breachTimerAveragePercentage));
            breachNewTimerPercentage = breachTimerPercentage + breachTimerPercentageToAdd;
        }
        else
        {
            breachNewTimerPercentage = breachTimerPercentage;
        }

        return breachNewTimerPercentage;
    }

    private float StartNewBreachTimer2(float breachTimerPercentage)
    {
        // create additional variables
        float delta = breachTimerPercentage - _breachTimerAveragePercentage;
        float weight;

        if (delta < 0f)
        {
            // Skew below-average values toward average
            weight = breachTimerPercentage / _breachTimerAveragePercentage;
        }
        else if (delta > 0f)
        {
            // Skew above-average values toward average
            weight = 1f - ((breachTimerPercentage - _breachTimerAveragePercentage) / (1f - _breachTimerAveragePercentage));
        }
        else
        {
            weight = 0f;
        }

        return breachTimerPercentage - delta * weight;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(HBMTest),true)]
public class HBMTestInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        HBMTest hbm = (HBMTest) target;
        if (GUILayout.Button("Run Test", GUILayout.Width(120f)))
            hbm.RunTests();
    }
}
#endif