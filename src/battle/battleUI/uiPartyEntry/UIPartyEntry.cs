using Godot;
using System;

public partial class UIPartyEntry : TextureRect
{
    [Export] Texture2D HPBarImage;
    [Export] Texture2D MPBarImage;

    Label name;
    UIBar hpBar;
    UIBar mpBar;
    HBoxContainer buffIcons;

    BattleActor _partyActor;

    public override void _Ready()
    {
        name = GetNode<Label>("VBoxContainer/Name");

        hpBar = GetNode<UIBar>("VBoxContainer/HPBar");
        hpBar.TextureProgress = HPBarImage;

        mpBar = GetNode<UIBar>("VBoxContainer/MPBar");
        mpBar.TextureProgress = MPBarImage;

    }
    
    public void Setup(BattleActor partyActor)
    {
        _partyActor = partyActor;
        name.Text = _partyActor.GetActorName();

        hpBar.Setup(_partyActor.GetCurHP(), _partyActor.GetMaxHP());
        mpBar.Setup(_partyActor.GetCurMP(), _partyActor.GetMaxMP());

        _partyActor.GetCharacterStats().HPChanged += OnBattlerHPChanged;
        _partyActor.GetCharacterStats().MPChanged += OnBattlerMPChanged;

        // battler.BuffsChanged += OnBuffsChanged;
    }

    // Update HPBar to new value
    private void OnBattlerHPChanged(int newHP)
    {
        hpBar.targetValue = _partyActor.GetCurHP();
    }

    // Update MPBar to new value
    private void OnBattlerMPChanged(int newMP)
    {
        mpBar.targetValue = _partyActor.GetCurMP();
    }

    private void OnBuffsChanged()
    {
    }
}
