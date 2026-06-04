using Godot;
using System;

public partial class ActiveStatusCondition : ActivePassiveEffect
{
    [Signal] public delegate void TurnCountChangedEventHandler(int currentTurnCount);
    [Signal] public delegate void TurnCountFinishedEventHandler();

    int _turnCount = -1;
    int turnCount
    {
        get { return _turnCount; }
        set
        {
            _turnCount = value;
            if (_turnCount <= 0)
            {
                EmitSignal(SignalName.TurnCountFinished);
            } else {
                EmitSignal(SignalName.TurnCountChanged, _turnCount);
            }
        }
    }

    BattleConsts.ElementType elementType;
    Godot.Collections.Dictionary<BattleConsts.ElementType, StatusConditionResource> _fusionStatusConditions;

    Texture2D _icon;

    public ActiveStatusCondition(StatusConditionResource statusConditionResource, int turnAmount)
    {
        _startupEffects = statusConditionResource.GetPassiveActionResource().GetStartupActions();
        _triggerEffects = statusConditionResource.GetPassiveActionResource().GetTriggerActions();
        _cleanupEffects = statusConditionResource.GetPassiveActionResource().GetCleanupActions();
        _triggerType = statusConditionResource.GetTriggerType();

        elementType = statusConditionResource.GetElementType();
        _fusionStatusConditions = statusConditionResource.GetFusionStatusConditions();

        turnCount = turnAmount;

        _icon = statusConditionResource.GetIcon();
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
        turnCount -= 1;
    }

    public int GetTurnCount() { return turnCount; }

    public StatusConditionResource GetFusionStatusCondition(BattleConsts.ElementType elementType)
    {
        if (_fusionStatusConditions.ContainsKey(elementType))
        {
            return _fusionStatusConditions[elementType];
        } else {
            return null;
        }
    }


    public Texture2D GetStatusIcon() { return _icon; }
}
