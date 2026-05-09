using Godot;
using System;

public partial class UIBattlerEntry : TextureRect
{
    [Export] Texture2D HPBarImage;
    [Export] Texture2D MPBarImage;

    Label name;
    UIBar hpBar;
    UIBar mpBar;
    HBoxContainer buffIcons;

    BattleActor battler;

    public override void _Ready()
    {
        name = GetNode<Label>("HBoxContainer/BattlerEntry/Name");
        hpBar = GetNode<UIBar>("HBoxContainer/BattlerEntry/Bars/UIHPBar");
        hpBar.TextureProgress = HPBarImage;

        mpBar = GetNode<UIBar>("HBoxContainer/BattlerEntry/Bars/UIMPBar");
        mpBar.TextureProgress = MPBarImage;

        buffIcons = GetNode<HBoxContainer>("HBoxContainer/BuffIcons");
    }
    
    public void SetupBattler(BattleActor _battler)
    {
        battler = _battler;
        name.Text = battler.GetName();

        // hpBar.Setup(battler..curHP, battler.GetStats().maxHP);
        // mpBar.Setup(battler.GetStats().curMP, battler.GetStats().maxMP);

        // battler.GetStats().HPChanged += OnBattlerHPChanged;
        // battler.GetStats().MPChanged += OnBattlerMPChanged;

        // battler.BuffsChanged += OnBuffsChanged;
    }

    // Update HPBar to new value
    private void OnBattlerHPChanged()
    {
        //hpBar.targetValue = battler.GetStats().curHP;
    }

    // Update MPBar to new value
    private void OnBattlerMPChanged()
    {
        //mpBar.targetValue = battler.GetStats().curMP;
    }

    private void OnBuffsChanged()
    {
    }
}
