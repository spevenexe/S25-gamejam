using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Color _highlightOutline = Color.yellow;
    [SerializeField] private Color _defaultOutline = Color.black;

    [SerializeField] private float _lerpStrength=0.01f;
    [SerializeField] private float _maxFollowDistance = 5f;
    [SerializeField] private float _playerRadius = 5f;
    [SerializeField] private float _interactDistance=5;
    public Interactable Target {get; private set;}
    public HeavyItem HauledItem {get; private set;}
    public EquippableItem EquippedItem {get; private set;}
    [SerializeField] private EquipUI EquipSlot;

    // currently initialized by Player.cs;
    private PlayerCamera _pCam;
    [SerializeField] private Transform _hauledItemSlotTransform;
    void Start()
    {
        _pCam = GetComponent<Player>()._playerCamera;
    }

    void Update()
    {
        Target?.highlight(_defaultOutline);
        // if we aren't carrying anything heavy, look for something to interact with
        if(HauledItem == null){
            Transform cameraTransform = _pCam.transform;
            RaycastHit[] hits = Physics.RaycastAll(cameraTransform.position,cameraTransform.forward,_interactDistance,LayerMask.GetMask("Interactable"));
            
            if(hits.Length == 0) {
                Target = null;
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
                Target = minHit.transform.GetComponent<Interactable>();
            }
            // if we have an equipped item, don't show the prompt for another equippable item
            if (Target != null && Target.GetType() == typeof(EquippableItem) && EquippedItem != null)
                EquipSlot.ClearToolTip();
            else
            {
                EquipSlot.showToolTip(Target,EquippedItem);
                Target?.highlight(_highlightOutline);
            }
        }
        // we are carrying something heavy, interacting will drop it
        else
        {
            Target = HauledItem;
    
            Vector3 start = HauledItem.transform.position;
            Vector3 end = _hauledItemSlotTransform.position;
            start = Vector3.ClampMagnitude(start-end,_maxFollowDistance) + end;
            Vector3 newPos = Vector3.Lerp(start,end,_lerpStrength);
            Vector3 playerPos = transform.position;
            float dist = Vector3.Distance(newPos,playerPos);
            if (dist < _playerRadius)
                newPos+= (newPos-playerPos).normalized*(_playerRadius-dist);

            // if we want to SLerp instead of lerp
            // Vector3 pivot = (start + end) * 0.5f - transform.position;
            // start-=pivot;
            // end-=pivot;
            // Vector3 newPos = Vector3.Slerp(start,end,_lerpStrength) + pivot;
            HauledItem.transform.position = newPos;
            EquipSlot.ClearToolTip();
        }
    }

    public void Drop()
    {
        if(HauledItem != null) DropHauledItem();
        else if(EquippedItem != null) Unequip();
    }

    internal void Haul(HeavyItem item)
    {
        Rigidbody rb = item.rb;
        rb.useGravity = false;
        rb.excludeLayers = LayerMask.GetMask("Player"); 
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.rotation = Quaternion.identity;
        HauledItem = item;
        HauledItem.CanMakeNoise = true;
    }

    public void DropHauledItem()
    {
        Rigidbody rb = HauledItem.rb;
        HauledItem.startSoundFallOff();
        HauledItem = null;
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

        EquippedItem = item;
        // EquippedItem.transform.SetParent(EquipSlot.transform,true);
        // StartCoroutine(EquipSlot.LerpItem(EquippedItem));
        StartCoroutine(EquipSlot.LerpItemToPocket(EquippedItem));
        EquipSlot.BottomRightText.text = EquippedItem.DropTooltip();
    }

    internal void Unequip()
    {
        Rigidbody rb = EquippedItem.rb;

        EquippedItem.gameObject.layer = LayerMask.NameToLayer("Interactable");
        StartCoroutine(EquipSlot.LerpItemToFloor(EquippedItem,_hauledItemSlotTransform));
        EquippedItem.startSoundFallOff();
        EquippedItem = null;
        EquipSlot.BottomRightText.text = "";
    }
}
