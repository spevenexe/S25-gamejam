using System.Collections;

public class ElectricModule : BaseModule
{
    public override void BreakModule()
    {
        
    }

    protected override IEnumerator PlayFixingMinigame()
    {
        FixModule();
        yield break;
    }
}