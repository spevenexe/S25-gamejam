using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Item : Interactable
{
    public Rigidbody rb {get; private set;}
    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public string DropTooltip()
    {
        return Utils.getKeys(PInput,"Drop") + " Drop " + name;
    }
}
