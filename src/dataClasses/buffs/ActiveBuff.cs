using Godot;
using System;

public partial class ActiveBuff : ActivePassiveEffect
{
    [Signal] public delegate void TurnCountChangedEventHandler(int currentTurnCount);
    [Signal] public delegate void BuffFinishedEventHandler(ActiveBuff activeBuff);

    BuffResource _buffResource;
    int _turnCount = -1;
    int turnCount
    {
        get { return _turnCount; }
        set
        {
            _turnCount = value;
            if (_turnCount <= 0)
            {
                EmitSignal(SignalName.BuffFinished, this);
            } else {
                EmitSignal(SignalName.TurnCountChanged, _turnCount);
            }
        }
    }
    
    bool _isPermanent = false;

    Texture2D _icon;

    public ActiveBuff(BuffResource buffResource, int turnAmount, bool isPermanent)
    {
        _buffResource = buffResource;

        _startupEffects = buffResource.GetPassiveActionResource().GetStartupActions();
        _triggerEffects = buffResource.GetPassiveActionResource().GetTriggerActions();
        _cleanupEffects = buffResource.GetPassiveActionResource().GetCleanupActions();
        _triggerType = buffResource.GetTriggerType();

        turnCount = turnAmount;
        _isPermanent = isPermanent;

        _icon = buffResource.GetIcon();
    }

    public void AddTurn(int newTurnCount)
    {
        turnCount += newTurnCount;
    }

    public void SetTurn(int newTurnCount)
    {
        turnCount = newTurnCount;
    }

    public void DecrementTurn()
    {
        if (!_isPermanent) turnCount -= 1;
    }

    public int GetTurnCount() { return turnCount; }

    public void SetIsPermanent(bool isPermanent) { _isPermanent = isPermanent; }

    // USED FOR TYPE CHECKING
    public BuffResource GetBuffResource() { return _buffResource; }

    public Texture2D GetBuffIcon() { return _icon; }
}
