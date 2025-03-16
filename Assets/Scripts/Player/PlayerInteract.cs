using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float _lerpStrength=0.6f;
    [SerializeField] private float _interactDistance=5;
    public Interactable Target {get; private set;}
    public Item HeldItem {get; private set;}
    // currently initialized by Player.cs;
    [DoNotSerialize] public PlayerCamera PCam;
    [SerializeField] private Transform _pocketTransform;
    void Update()
    {
        if(HeldItem == null){
            Transform cameraTransform = PCam.transform;
            Debug.DrawRay(cameraTransform.position,cameraTransform.forward * _interactDistance,Color.red,.1f);
            RaycastHit[] hits = Physics.RaycastAll(cameraTransform.position,cameraTransform.forward,_interactDistance,LayerMask.GetMask("Interactable"));
            
            if(hits.Length == 0) {
                Target = null;
                return;
            }
            RaycastHit minHit = hits[0];
            float minDistance = hits[0].distance;
            foreach(RaycastHit h in hits){
                if(h.distance < minDistance){
                    minHit = h;
                    minDistance = h.distance;
                }
            }
            Target = minHit.transform.GetComponent<Interactable>();
        }else{
            Target = HeldItem;
    
            Vector3 currentPos = HeldItem.transform.position;
            Vector3 targetPos = _pocketTransform.position;
            float xLerp = Mathf.Lerp(currentPos.x,targetPos.x,_lerpStrength);
            float yLerp = Mathf.Lerp(currentPos.y,targetPos.y,_lerpStrength);
            float zLerp = Mathf.Lerp(currentPos.z,targetPos.z,_lerpStrength);
            HeldItem.transform.position = new Vector3(xLerp,yLerp,zLerp);
        }
    }

    public void DropItem()
    {
        HeldItem = null;
    }

    internal void PickUp(Item item)
    {
        HeldItem = item;
    }
}
