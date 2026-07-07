using Godot;
using System;

public partial class UIBattleMenuEntryBase : UIMenuEntryBase
{
    public override void _Ready()
    {
        base._Ready();
    }

    public bool IsEnabled() { return _enabled; }

    public virtual string GetMenuDesc() { return ""; }
}
