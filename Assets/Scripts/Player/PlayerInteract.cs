using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : PlayerSystem
{
    [SerializeField] private Color _highlightOutline = Color.yellow;
    [SerializeField] private Color _defaultOutline = Color.black;

    [SerializeField] private float _lerpStrength=0.01f;
    [SerializeField] private float _maxFollowDistance = 5f;
    [SerializeField] private float _playerRadius = 5f;
    [SerializeField] private float _interactDistance=5;
    [SerializeField] private EquipUI EquipSlot;

    [SerializeField] private PlayerCamera _pCam;
    [SerializeField] private Transform _hauledItemSlotTransform;

    void OnEnable()
    {
        player.InteractInput.performed += Use;
        player.DropInput.performed += Drop;
        player.EquipEvent += Equip;
        player.HaulEvent += Haul;
        player.DropHeavyEvent += DropHauledItem;
    }

    void OnDisable()
    {
        player.InteractInput.performed -= Use;
        player.DropInput.performed -= Drop;
        player.EquipEvent -= Equip;
        player.HaulEvent -= Haul;
        player.DropHeavyEvent -= DropHauledItem;
    }

    void Update()
    {
        player.Target?.highlight(_defaultOutline);
        // if we aren't carrying anything heavy, look for something to interact with
        if(player.HauledItem == null){
            Transform cameraTransform = _pCam.transform;
            RaycastHit[] hits = Physics.RaycastAll(cameraTransform.position,cameraTransform.forward,_interactDistance,LayerMask.GetMask("Interactable"));
            
            if(hits.Length == 0) {
                player.Target = null;
            }
            else
            {
                RaycastHit minHit = hits[0];
                float minDistance = hits[0].distance;
                foreach(RaycastHit h in hits){
                    if(h.distance < minDistance){
                        minHit = h;
                        minDistance = h.distance;
                    }
                }
                player.Target = minHit.transform.GetComponent<Interactable>();
            }
            // if we have an equipped item, don't show the prompt for another equippable item
            if (player.Target != null && player.Target.GetType() == typeof(EquippableItem) && player.EquippedItem != null)
                EquipSlot.ClearToolTip();
            else
            {
                EquipSlot.showToolTip(player.Target,player.EquippedItem);
                player.Target?.highlight(_highlightOutline);
            }
        }
        // we are carrying something heavy, interacting will drop it
        else
        {
            player.Target = player.HauledItem;
    
            player.HauledItem.rb.linearVelocity = Vector3.zero;
            Vector3 start = player.HauledItem.transform.position;
            Vector3 end = _hauledItemSlotTransform.position;
            start = Vector3.ClampMagnitude(start-end,_maxFollowDistance) + end;
            Vector3 newPos = Vector3.Lerp(start,end,_lerpStrength);
            Vector3 playerPos = transform.position;
            float dist = Vector3.Distance(newPos,playerPos);
            if (dist < _playerRadius)
                newPos+= (newPos-playerPos).normalized*(_playerRadius-dist);

            // if we want to SLerp instead of lerp (dont slerp)
            // Vector3 pivot = (start + end) * 0.5f - transform.position;
            // start-=pivot;
            // end-=pivot;
            // Vector3 newPos = Vector3.Slerp(start,end,_lerpStrength) + pivot;
            player.HauledItem.transform.position = newPos;
            EquipSlot.ClearToolTip();
        }
    }

    private void Use(InputAction.CallbackContext context)
    {
        if(player.Target == null) SFXManager.PlaySound(SFXManager.SoundType.INTERACT_FAIL,0.3f);
        else player.Target?.Interact(player);
    }

    public void Drop(InputAction.CallbackContext context)
    {
        if (player.HauledItem != null) DropHauledItem();
        else if (player.EquippedItem != null) Unequip();
    }

    internal void Haul(HeavyItem item)
    {
        Rigidbody rb = item.rb;
        rb.useGravity = false;
        rb.excludeLayers = LayerMask.GetMask("Player"); 
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.rotation = Quaternion.identity;
        player.HauledItem = item;
        player.HauledItem.CanMakeNoise = true;
    }

    public void DropHauledItem()
    {
        Rigidbody rb = player.HauledItem.rb;
        player.HauledItem.startSoundFallOff();
        player.HauledItem = null;
        rb.useGravity = true;
        rb.excludeLayers = LayerMask.GetMask("Nothing"); 
        rb.linearVelocity = Vector3.zero;
    }

    internal void Equip(EquippableItem item)
    {
        Rigidbody rb = item.rb;
        rb.useGravity = false;
        rb.excludeLayers = LayerMask.NameToLayer("Everything"); 
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.rotation = Quaternion.identity;
        rb.freezeRotation = true;
        // item.transform.SetPositionAndRotation(Vector3.zero,Quaternion.identity);
        // item.transform.rotation = Quaternion.identity;
        item.gameObject.layer = LayerMask.NameToLayer("UI");
        item.CanMakeNoise = true;

        player.EquippedItem = item;
        // player.EquippedItem.transform.SetParent(EquipSlot.transform,true);
        // StartCoroutine(EquipSlot.LerpItem(player.EquippedItem));
        StartCoroutine(EquipSlot.LerpItemToPocket(player.EquippedItem));
        EquipSlot.BottomRightText.text = player.EquippedItem.DropTooltip();
    }

    internal void Unequip()
    {
        Rigidbody rb = player.EquippedItem.rb;

        player.EquippedItem.gameObject.layer = LayerMask.NameToLayer("Interactable");
        StartCoroutine(EquipSlot.LerpItemToFloor(player.EquippedItem,_hauledItemSlotTransform));
        player.EquippedItem.startSoundFallOff();
        player.EquippedItem = null;
        EquipSlot.BottomRightText.text = "";
    }
}
