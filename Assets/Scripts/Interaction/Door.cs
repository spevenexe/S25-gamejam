using System.Collections;
using UnityEditor;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] public BoxCollider boxCollider;

    public void Close() => StartCoroutine(closeHelper());

    private IEnumerator closeHelper()
    {
        boxCollider.enabled = false;
        Vector3 doorInitRotation = transform.localEulerAngles;
        Vector3 doorDestRotation = doorInitRotation;
        doorDestRotation.z = 0;
        float closeTime = 3f;
        for(float timer = 0; timer < closeTime; timer+=Time.deltaTime)
        {
            timer+=Time.deltaTime;
            float t = timer/(closeTime-timer);
            transform.localEulerAngles = Vector3.Slerp(doorInitRotation,doorDestRotation,t);
            yield return null;
        }
        transform.localEulerAngles = doorDestRotation;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Door),true)]
public class DoorInspector : Editor
{
    // private SerializedProperty secondsProperty;

    // private void OnEnable()
    // {
    //     secondsProperty = serializedObject.FindProperty("SecondsToDecrement");
    // }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Door door = (Door) target;
        if(GUILayout.Button("Close",GUILayout.Width(120f)))
            door.Close();
    }
}
#endif