using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class EquipUI : MonoBehaviour
{
    [SerializeField] private float _lerpStrength = 0.01f;
    [SerializeField] private float _pickupLerpStrength = 0.01f;
    [SerializeField] private float _maxFollowDistance = 5f;
    [SerializeField] private Vector3 _defaultRotation = new Vector3(0,60,15);
    // [SerializeField] private Transform _cameraTransform;
    [SerializeField] private TMP_Text _centerTooltipText;
    public TMP_Text BottomRightText;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float _throwStrength = 3f;

    void Start()
    {
        if(transform.parent != null && cameraTransform == null)
            cameraTransform = transform.parent;
    }

    internal void showToolTip(Interactable target, EquippableItem equippedItem)
    {
        if (target == null) 
            _centerTooltipText.text = "";
        else
            _centerTooltipText.text = target.MessageTooltip(equippedItem);
    }

    internal void ClearToolTip()
    {
        _centerTooltipText.text = "";
    }

    internal IEnumerator LerpItem(EquippableItem equippedItem)
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

    // fast lerp item to equip, then normal lerp
    internal IEnumerator LerpItemToPocket(EquippableItem item)
    {
        if(item.targetRotation != Vector3.zero) transform.localEulerAngles = item.targetRotation;
        else transform.localEulerAngles = _defaultRotation; 
        yield return LerpItemToTransform(item,transform,LayerMask.NameToLayer("UI"));
        if (item.gameObject.layer == LayerMask.NameToLayer("UI"))
        {
            // item.transform.localScale = Vector3.one;
            // item.transform.rotation = Quaternion.identity;
            item.transform.SetParent(null);
            StartCoroutine(LerpItem(item));
        }
    }
    
    private IEnumerator LerpItemToTransform(EquippableItem item, Transform target, int layer)
    {
        // item.transform.localEulerAngles = target.localEulerAngles;
        item.transform.SetParent(target,true);
        item.transform.rotation = target.rotation;
        Vector3 originalScale = item.transform.localScale;
        float minScaleX = Mathf.Min(originalScale.x,target.localScale.x);
        float minScaleY = Mathf.Min(originalScale.y,target.localScale.y);
        float minScaleZ = Mathf.Min(originalScale.z,target.localScale.z);
        float maxScaleX = Mathf.Max(originalScale.x,target.localScale.x);
        float maxScaleY = Mathf.Max(originalScale.y,target.localScale.y);
        float maxScaleZ = Mathf.Max(originalScale.z,target.localScale.z);
        while(Vector3.Distance(item.transform.position,target.position) > 0.1f)
        {
            
            Vector3 start;
            Vector3 end;

            // start = item.transform.localScale;
            // end = Vector3.one;
            // // prevent over-scaling
            // Vector3 temp = Vector3.Lerp(start,end,_pickupLerpStrength);
            // temp.x = Mathf.Clamp(temp.x,minScaleX,maxScaleX);
            // temp.y = Mathf.Clamp(temp.y,minScaleY,maxScaleY);
            // temp.z = Mathf.Clamp(temp.z,minScaleZ,maxScaleZ);
            // item.transform.localScale = temp;

            start = item.transform.position;
            end = target.position;
            item.transform.position = Vector3.Lerp(start,end,_pickupLerpStrength);

            if (item.gameObject.layer != layer) yield break;

            yield return null;
        }
    }

    internal IEnumerator LerpItemToFloor(EquippableItem item, Transform target)
    {
        item.transform.SetParent(target,true);
        Vector3 originalScale = item.transform.localScale;
        float minScaleX = Mathf.Min(originalScale.x,target.localScale.x);
        float minScaleY = Mathf.Min(originalScale.y,target.localScale.y);
        float minScaleZ = Mathf.Min(originalScale.z,target.localScale.z);
        float maxScaleX = Mathf.Max(originalScale.x,target.localScale.x);
        float maxScaleY = Mathf.Max(originalScale.y,target.localScale.y);
        float maxScaleZ = Mathf.Max(originalScale.z,target.localScale.z);
        // while( 
        // Vector3.Distance(item.transform.localScale,target.localScale) > 0.1f)
        // {
            
        //     Vector3 start;
        //     Vector3 end;

        //     start = item.transform.localScale;
        //     end = Vector3.one;
        //     // prevent over-scaling
        //     Vector3 temp = Vector3.Lerp(start,end,_pickupLerpStrength);
        //     temp.x = Mathf.Clamp(temp.x,minScaleX,maxScaleX);
        //     temp.y = Mathf.Clamp(temp.y,minScaleY,maxScaleY);
        //     temp.z = Mathf.Clamp(temp.z,minScaleZ,maxScaleZ);
        //     item.transform.localScale = temp;

        //     // start = item.transform.position;
        //     // end = target.position;
        //     // item.transform.position = Vector3.Lerp(start,end,_pickupLerpStrength);
        //     yield return null;
        // }

        // item.transform.localScale = Vector3.one;
        item.transform.SetParent(null);

        item.rb.excludeLayers = LayerMask.GetMask("Nothing"); 
        item.rb.useGravity = true;
        item.rb.freezeRotation = false;
        
        item.transform.position+=cameraTransform.forward*0.5f;
        item.rb.linearVelocity = cameraTransform.forward * _throwStrength;
        yield return null;
    }
}
