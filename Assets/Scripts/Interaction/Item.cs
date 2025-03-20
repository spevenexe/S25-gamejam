using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Item : Interactable
{
    public Rigidbody rb {get; private set;}
    [Serialize] protected String _itemName;
    protected override void Start()
    {
        if(_itemName == null ||_itemName.Length == 0) _itemName = name;
        rb = GetComponent<Rigidbody>();
    }

    public string DropTooltip()
    {
        return Utils.getKeys(PInput,"Drop") + " Drop " + _itemName;
    }

    protected override String UniqueToolTip() => _itemName;
}
