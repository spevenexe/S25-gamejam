using System.Collections;
using UnityEngine;

public class CargoModule : BaseModule
{
    private Item [] _cargo;
    [SerializeField] private GameObject cargoGroup;
    [SerializeField] private float _joltVelocity=5f;
    [SerializeField] private float _verticalTossWeighting=2f;
    [SerializeField] private float _randomZ=.5f;

    protected override void Start()
    {
        base.Start();
        _cargo = cargoGroup.GetComponentsInChildren<Item>();
    }

    public override void BreakModule()
    {
        base.BreakModule();
        Debug.Log("breaking");
        foreach(Item c in _cargo)
        {
            Rigidbody rb = c.rb;
            float xDir = transform.position.x - c.gameObject.transform.position.x;
            float yDir = transform.position.y - c.gameObject.transform.position.y;
            Vector3 dir = new Vector3(
                xDir,
                yDir+_verticalTossWeighting,
                Random.Range(-_randomZ,_randomZ)
                ).normalized;

            rb.linearVelocity = rb.linearVelocity+dir*_joltVelocity;
        }
    }

    protected override IEnumerator PlayFixingMinigame()
    {
        yield return new WaitForSeconds(_lightFlashInterval*3);
        FixModule();
        yield break;
    }
}