using UnityEngine;

public abstract class Item : Interactable
{
    public Rigidbody rb {get; private set;}
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public string DropTooltip()
    {
        return Utils.getKeys(PInput,"Drop") + " Drop " + name;
    }
}
