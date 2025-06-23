using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Class that handles tooltip generation and managing the equipped item.
/// </summary>
public class EquipUI : MonoBehaviour
{
    [SerializeField] private float _lerpStrength = 0.01f;
    [SerializeField] private float _pickupLerpStrength = 0.01f;
    [SerializeField] private float _maxFollowDistance = 5f;
    [SerializeField] private Vector3 _defaultRotation = new Vector3(0, 60, 15);
    [SerializeField] private TMP_Text _centerTooltipText;
    public TMP_Text BottomRightText;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float _throwStrength = 3f;

    void Start()
    {
        if (transform.parent != null && cameraTransform == null)
            cameraTransform = transform.parent;
    }

    /// <summary>
    /// Sets the center tooltip text based on what that camera is looking at.
    /// </summary>
    /// <param name="target">The interactable that the camera is looking at.</param>
    /// <param name="equippedItem">The item equipped in the player's hand. It may alter the tooltip.</param>
    internal void showToolTip(Interactable target, EquippableItem equippedItem)
    {
        if (target == null)
            ClearToolTip();
        else
            _centerTooltipText.text = target.MessageTooltip(equippedItem);
    }

    /// <summary>
    /// Empty the tooltip text.
    /// </summary>
    internal void ClearToolTip()
    {
        _centerTooltipText.text = "";
    }

    /// <summary>
    /// Moves the equipped item with the player as they move.
    /// </summary>
    /// <param name="equippedItem">The equipped item.</param>
    internal IEnumerator LerpItem(EquippableItem equippedItem)
    {
        // the layer that the item is on determines whether it is "equipped"
        while (equippedItem.gameObject.layer == LayerMask.NameToLayer("UI"))
        {
            equippedItem.rb.linearVelocity = Vector3.zero;
            Vector3 start = equippedItem.transform.position;
            Vector3 end = transform.position;
            // introduces lag between the item and movement direction, so the item appears to drag a bit before following the player
            if (Vector3.Distance(start, end) > 0.1f)
            {
                start = Vector3.ClampMagnitude(start - end, _maxFollowDistance) + end;
                equippedItem.transform.position = Vector3.Lerp(start, end, _lerpStrength);
            }
            equippedItem.transform.rotation = transform.rotation;
            yield return null;
        }
    }

    /// <summary>
    /// Lerps an item to the equip slot a fast speed, then starts the regular lerping routine, with reduced lerp speed.
    /// </summary>
    /// <param name="item">The item to lerp.</param>
    internal IEnumerator LerpItemToPocket(EquippableItem item)
    {
        if (item.targetRotation != Vector3.zero) transform.localEulerAngles = item.targetRotation;
        else transform.localEulerAngles = _defaultRotation;
        yield return LerpItemToTransform(item, transform, LayerMask.NameToLayer("UI"));
        if (item.gameObject.layer == LayerMask.NameToLayer("UI"))
        {
            item.transform.SetParent(null);
            StartCoroutine(LerpItem(item));
        }
    }

    /// <summary>
    /// Lerps an item (presumably the one to equip) to a transform, so long as it remains on a given layer.
    /// </summary>
    /// <param name="item">The item to lerp</param>
    /// <param name="target">The position to lerp to</param>
    /// <param name="layer">The layer that the item should remain on while lerping</param>
    private IEnumerator LerpItemToTransform(EquippableItem item, Transform target, int layer)
    {
        item.transform.SetParent(target, true);
        item.transform.rotation = target.rotation;
        while (Vector3.Distance(item.transform.position, target.position) > 0.1f)
        {
            item.rb.linearVelocity = Vector3.zero;
            Vector3 start;
            Vector3 end;

            start = item.transform.position;
            end = target.position;
            item.transform.position = Vector3.Lerp(start, end, _pickupLerpStrength);

            if (item.gameObject.layer != layer) yield break;

            yield return null;
        }
    }

    /// <summary>
    /// Throws an item (presumably the equipped one) to floor, resuming physics on the object.
    /// </summary>
    /// <param name="item">The equipped item to drop to the floor</param>
    /// <returns></returns>
    internal IEnumerator ThrowItemToFloor(EquippableItem item)
    {
        item.transform.SetParent(null);

        // resume physics
        item.rb.excludeLayers = LayerMask.GetMask("Nothing");
        item.rb.useGravity = true;
        item.rb.freezeRotation = false;

        // yeets the item out with some velocity from the camera
        item.transform.position += cameraTransform.forward * 0.5f;
        item.rb.linearVelocity = cameraTransform.forward * _throwStrength;
        yield return null;
    }
}
