using System.Collections;
using UnityEngine;

public class EquipUI : MonoBehaviour
{
    [SerializeField] private float _lerpStrength = 0.01f;
    [SerializeField] private float _maxFollowDistance = 5f;
    [SerializeField] private Transform _cameraTransform;
    internal IEnumerator LerpItem(Item equippedItem)
    {
        while(equippedItem.gameObject.layer == LayerMask.NameToLayer("UI"))
        {
            Vector3 start = equippedItem.transform.position;
            Vector3 end = transform.position;
            if (Vector3.Distance(start,end) > 0.1f)
            {
                start = Vector3.ClampMagnitude(start-end,_maxFollowDistance) + end;
                equippedItem.transform.position = Vector3.Lerp(start,end,_lerpStrength);
            }
            equippedItem.transform.rotation = transform.rotation;
            yield return null;
        }
    }
}
