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
    
    bool _isDebuff = false;
    bool _isPermanent = false;
    bool _hasBeenUsed = false;

    Texture2D _icon;

    public ActiveBuff(BuffResource buffResource, int turnAmount) : base(
        buffResource.GetPassiveActionResource().GetTriggerActions(),
        buffResource.GetPassiveActionResource().GetStartupActions(),
        buffResource.GetPassiveActionResource().GetCleanupActions()
    )
    {
        _buffResource = buffResource;
        _triggerType = buffResource.GetTriggerType();
        
        turnCount = turnAmount;
        
        _isDebuff = buffResource.GetIsDebuff();
        _isPermanent = buffResource.GetIsPermanent();
        _runOnce = buffResource.GetRunOnce();

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

    public void SetHasBeenUsed(bool value) { _hasBeenUsed = value; }
    public bool GetHasBeenUsed() { return _hasBeenUsed; }

    public void DecrementTurn()
    {
        if (!_isPermanent) {
            // Signal for deletion if buff has been ran and only wants to be run once
            if (_runOnce && _hasBeenRan)
            {
                turnCount = 0;
            } else {
                // Only decrement if the buff is not run once
                if (!_runOnce)
                {
                    turnCount -= 1;
                }
            }
        }
    }

    public int GetTurnCount() { return turnCount; }
    public bool GetIsDebuff() { return _isDebuff; }

    public bool GetIsPermanent() { return _isPermanent; }
    public void SetIsPermanent(bool isPermanent) { _isPermanent = isPermanent; }

    // USED FOR TYPE CHECKING
    public BuffResource GetBuffResource() { return _buffResource; }

    public Texture2D GetBuffIcon() { return _icon; }
}
