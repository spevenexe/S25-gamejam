using System;

/// <summary>
/// Container interface that declares implementees must use an interation event
/// </summary>
public interface ITriggerEvent
{
    public event Action InteractionTriggers;
}