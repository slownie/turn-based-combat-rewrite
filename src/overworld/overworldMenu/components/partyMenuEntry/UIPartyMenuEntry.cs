using Godot;
using System;

public partial class UIPartyMenuEntry : Control
{
	[Export] Texture2D HPBarImage;
    [Export] Texture2D MPBarImage;

    Label name;
    UIBar hpBar;
    UIBar mpBar;

	public override void _Ready()
	{
		name = GetNode<Label>("VBoxContainer/Name");

        hpBar = GetNode<UIBar>("VBoxContainer/HPBar");
        hpBar.TextureProgress = HPBarImage;

        mpBar = GetNode<UIBar>("VBoxContainer/MPBar");
        mpBar.TextureProgress = MPBarImage;
	}

	public void Setup(ActivePartyMember partyMember)
    {
        name.Text = partyMember.GetPartyMemberName();

        hpBar.Setup(partyMember.GetCharacterStats().GetCurHP(), partyMember.GetCharacterStats().GetMaxHP());
        mpBar.Setup(partyMember.GetCharacterStats().GetCurMP(), partyMember.GetCharacterStats().GetMaxMP());

        // battler.BuffsChanged += OnBuffsChanged;
    }
}
