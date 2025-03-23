using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Item : Interactable
{
    public Rigidbody rb {get; private set;}
    [SerializeField] private InteractbleWithItem.ItemType itemName = InteractbleWithItem.ItemType.None;
    public string ItemName {get {return itemName.ToString().Replace('_',' ');}}
    public InteractbleWithItem.ItemType it {get => itemName;}

    [SerializeField] private float _clangVolume=1f;
    public float ClangVolume {get {return _clangVolume;}}
    public bool CanMakeNoise = false; // prevent crazy clashing sounds from happening at game start

    protected override void Start()
    {
        if(itemName == InteractbleWithItem.ItemType.None) Debug.LogWarning($"Item {gameObject} has no name.");
        rb = GetComponent<Rigidbody>();
    }

    public string DropTooltip()
    {
        return $"{Utils.getKeys(PInput,"Drop")} Drop {ItemName}";
    }

    protected override String UniqueToolTip(EquippableItem equippedItem) => ItemName;

    void OnCollisionEnter(Collision collision)
    {
        if (CanMakeNoise) SFXManager.PlaySound(SFXManager.SoundType.ITEM_CLANG,_clangVolume);
    }

    public void startSoundFallOff() => StartCoroutine(startSoundFallOffHelper());

    private IEnumerator startSoundFallOffHelper()
    {
        yield return new WaitForSeconds(5f);
        CanMakeNoise = false;
    }
}
