using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script that handles player interaction, highlighting interactables, and adjusting tooltips.
/// </summary>
public class PlayerInteract : PlayerSystem
{
    [SerializeField] private Color _highlightOutline = Color.yellow;
    [SerializeField] private Color _defaultOutline = Color.black;

    [SerializeField] private float _lerpStrength = 0.01f;
    [SerializeField] private float _maxFollowDistance = 5f;
    [SerializeField] private float _playerRadius = 5f;
    [SerializeField] private float _interactDistance = 5;
    [SerializeField] private EquipUI EquipSlot;

    [SerializeField] private PlayerCamera _pCam;
    [SerializeField] private Transform _hauledItemSlotTransform;

    void OnEnable()
    {
        _player.InteractInput.performed += Use;
        _player.DropInput.performed += Drop;
        _player.EquipEvent += Equip;
        _player.HaulEvent += Haul;
        _player.DropHeavyEvent += DropHauledItem;
    }

    void OnDisable()
    {
        _player.InteractInput.performed -= Use;
        _player.DropInput.performed -= Drop;
        _player.EquipEvent -= Equip;
        _player.HaulEvent -= Haul;
        _player.DropHeavyEvent -= DropHauledItem;
    }

    void Update()
    {
        _player.Target?.Highlight(_defaultOutline);
        // if we aren't carrying anything heavy, look for something to interact with
        if (_player.HauledItem == null)
        {
            Transform cameraTransform = _pCam.transform;
            RaycastHit[] hits = Physics.RaycastAll(cameraTransform.position, cameraTransform.forward, _interactDistance, LayerMask.GetMask("Interactable"));

            if (hits.Length == 0)
            {
                _player.Target = null;
            }
            // if we found something...
            else
            {
                // look for the nearest object to the player. That's the interaction target
                RaycastHit minHit = hits[0];
                float minDistance = hits[0].distance;
                foreach (RaycastHit h in hits)
                {
                    if (h.distance < minDistance)
                    {
                        minHit = h;
                        minDistance = h.distance;
                    }
                }
                _player.Target = minHit.transform.GetComponent<Interactable>();
            }
            // if we have an equipped item, don't show the prompt for another equippable item
            if (_player.Target != null && _player.Target.GetType() == typeof(EquippableItem) && _player.EquippedItem != null)
                EquipSlot.ClearToolTip();
            else
            {
                EquipSlot.showToolTip(_player.Target, _player.EquippedItem);
                _player.Target?.Highlight(_highlightOutline);
            }
        }
        // we are carrying something heavy, interacting will drop it
        else
        {
            _player.Target = _player.HauledItem;

            // Lerp the heavy item to follow a spot in front of the player
            _player.HauledItem.rb.linearVelocity = Vector3.zero;
            Vector3 start = _player.HauledItem.transform.position;
            Vector3 end = _hauledItemSlotTransform.position;
            start = Vector3.ClampMagnitude(start - end, _maxFollowDistance) + end;
            Vector3 newPos = Vector3.Lerp(start, end, _lerpStrength);
            Vector3 playerPos = transform.position;
            float dist = Vector3.Distance(newPos, playerPos);
            if (dist < _playerRadius)
                newPos += (newPos - playerPos).normalized * (_playerRadius - dist);
            _player.HauledItem.transform.position = newPos;
            EquipSlot.ClearToolTip();
        }
    }

    /// <summary>
    /// Perform the interact action based on what the player is looking at.
    /// </summary>
    /// <param name="context"></param>
    private void Use(InputAction.CallbackContext context)
    {
        if (_player.Target == null) SFXManager.PlaySound(SFXManager.SoundType.INTERACT_FAIL, 0.3f);
        else _player.Target?.Interact(_player);
    }

    /// <summary>
    /// Drops the heavy item that the player is carrying. If the player is not carrying a heavy item, drops the equipped item instead. Otherwise, does nothing.
    /// </summary>
    /// <param name="context"></param>
    public void Drop(InputAction.CallbackContext context)
    {
        if (_player.HauledItem != null) DropHauledItem();
        else if (_player.EquippedItem != null) Unequip();
    }

    /// <summary>
    /// Designates a heavy item for the player to carry. Allows other functions to move and interact with this object.
    /// </summary>
    /// <param name="item">The object to haul.</param>
    internal void Haul(HeavyItem item)
    {
        Rigidbody rb = item.rb;
        rb.useGravity = false;
        rb.excludeLayers = LayerMask.GetMask("Player");
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.rotation = Quaternion.identity;
        _player.HauledItem = item;
        _player.HauledItem.CanMakeNoise = true;
    }

    /// <summary>
    /// Drops the heavy item that the player is carrying. <c> _player.HauledItem </c> is assumed to be non-null before calling.
    /// </summary>
    public void DropHauledItem()
    {
        Rigidbody rb = _player.HauledItem.rb;
        _player.HauledItem.startSoundFallOff();
        _player.HauledItem = null;
        rb.useGravity = true;
        rb.excludeLayers = LayerMask.GetMask("Nothing");
        rb.linearVelocity = Vector3.zero;
    }

    /// <summary>
    /// Equips the targeted item.
    /// </summary>
    /// <param name="item">The item to equip. Cannot be null before calling.</param>
    internal void Equip(EquippableItem item)
    {
        // freeze physics on the item while it is being held 
        Rigidbody rb = item.rb;
        rb.useGravity = false;
        rb.excludeLayers = LayerMask.NameToLayer("Everything");
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.rotation = Quaternion.identity;
        rb.freezeRotation = true;
        item.gameObject.layer = LayerMask.NameToLayer("UI");
        item.CanMakeNoise = true;

        // set the equipped item and lerp it to the pocket transform
        _player.EquippedItem = item;
        StartCoroutine(EquipSlot.LerpItemToPocket(_player.EquippedItem));
        EquipSlot.BottomRightText.text = _player.EquippedItem.DropTooltip();
    }

    /// <summary>
    /// Unequips the currently equipped item. <c>_player.EquippedItem</c> is assumed to be non-null before calling.
    /// </summary>
    internal void Unequip()
    {
        Rigidbody rb = _player.EquippedItem.rb;

        _player.EquippedItem.gameObject.layer = LayerMask.NameToLayer("Interactable");
        StartCoroutine(EquipSlot.ThrowItemToFloor(_player.EquippedItem));
        _player.EquippedItem.startSoundFallOff();
        _player.EquippedItem = null;
        EquipSlot.BottomRightText.text = "";
    }
}
