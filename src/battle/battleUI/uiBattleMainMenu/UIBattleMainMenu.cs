using Godot;
using System;

public partial class UIBattleMainMenu : UIBattleMenuBase
{
    [Export] UseableSkillResource attackAction;
    [Export] UseableSkillResource passAction;
    [Export] UseableSkillResource escapeAction;

    [Signal] public delegate void ActionSelectedEventHandler(UseableSkillResource selectedAction);
    [Signal] public delegate void SkillMenuRequestedEventHandler();
    [Signal] public delegate void ItemMenuRequestedEventHandler();

    bool _enableSkillMenu = true;
    bool _enableItemMenu = true;

    public override void _Ready()
    {
        SetProcessInput(false);
    }

    public override void _Input(InputEvent @event)
    {
        // Attack
        if (@event.IsActionPressed("AButton"))
        {
            EmitSignal(SignalName.ActionSelected, attackAction);
        }

        // Pass
        if (@event.IsActionPressed("BButton"))
        {
            EmitSignal(SignalName.ActionSelected, passAction);
        }

        // Items
        if (@event.IsActionPressed("YButton"))
        {
            if (_enableItemMenu)
            {
                EmitSignal(SignalName.ItemMenuRequested);
            } else {
                GD.Print("No Items");
            }
        }

        // Skills
        if (@event.IsActionPressed("XButton"))
        {
            if (_enableSkillMenu)
            {
                EmitSignal(SignalName.SkillMenuRequested);
            } else {
                GD.Print("No Skills");
            }
        }

        // Escape
        if (@event.IsActionPressed("RButton"))
        {
            EmitSignal(SignalName.ActionSelected, escapeAction);
        }
    }

    public void Setup(bool enableSkillMenu, bool enableItemMenu)
    {
        _enableSkillMenu = enableSkillMenu;
        _enableItemMenu = enableItemMenu;
        
        SetProcessInput(true);
    }
}
