using System.Collections;

public class OxygenModule : BaseModule
{
    public void BreakOxygen()
    {
        base.BreakModule();
    }

    public override void DecrementTimer(float timerProgress)
    {
        base.DecrementTimer(timerProgress);
    }

    protected override IEnumerator PlayFixingMinigame()
    {
        FixModule();
        yield break;
    }
}