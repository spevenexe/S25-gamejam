using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Item : Interactable
{
    public Rigidbody rb {get; private set;}
    [SerializeField] private InteractbleWithItem.ItemType itemName = InteractbleWithItem.ItemType.None;
    public InteractbleWithItem.ItemType ItemName {get {return itemName;}}

    [SerializeField] private float _clangVolume=1f;
    public float ClangVolume {get {return _clangVolume;}}
    
    protected override void Start()
    {
        if(ItemName == InteractbleWithItem.ItemType.None) Debug.LogWarning($"Item {gameObject} has no name.");
        rb = GetComponent<Rigidbody>();
    }

    public string DropTooltip()
    {
        return $"{Utils.getKeys(PInput,"Drop")} Drop {ItemName}";
    }

    protected override String UniqueToolTip(EquippableItem equippedItem) => ItemName.ToString();

    void OnCollisionEnter(Collision collision)
    {
        SFXManager.PlaySound(SFXManager.SoundType.ITEM_CLANG,_clangVolume);
    }
}
