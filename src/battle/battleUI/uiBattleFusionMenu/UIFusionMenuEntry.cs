using Godot;
using System;

public partial class UIFusionMenuEntry : UIBattleMenuEntryBase
{
    [Export] Color hpColor = new Color("#ff0000");
	[Export] Color mpColor = new Color("#00ff0d");

    FusionSkillResource _fusionSkillResource;

    Label _costTypeText;
    Label _costAmountText;
    TextureRect _partnerIcon;

    public override void _Ready()
    {
        base._Ready();
        _costTypeText = GetNode<Label>("HBoxContainer/CostType");
        _costAmountText = GetNode<Label>("HBoxContainer/CostAmount");
        _partnerIcon = GetNode<TextureRect>("HBoxContainer/PartnerIcon");
    }

    public void Setup(FusionSkillResource fusionSkillResource, Texture2D partnerIcon, bool enabled)
    {
        _fusionSkillResource = fusionSkillResource;
        GD.Print(_fusionSkillResource.GetSkillName());

        _enabled = enabled;
        if (!_enabled) _nameText.Modulate = disabledColor;

        _icon.Texture = _fusionSkillResource.GetIcon();
        _nameText.Text = _fusionSkillResource.GetSkillName();
        _partnerIcon.Texture = partnerIcon;

        if (_fusionSkillResource.GetSkillCostType() == UseableSkillResource.SkillCostType.HP)
        {
            _costTypeText.Text = "HP";
            _costTypeText.Modulate = hpColor;
        } else {
            _costTypeText.Text = "MP";
            _costTypeText.Modulate = mpColor;
        }

        _costAmountText.Text = _fusionSkillResource.GetSkillCostAmount().ToString();
    }

    public FusionSkillResource GetFusionSkill() { return _fusionSkillResource; }
    public override string GetMenuDesc() { return _fusionSkillResource.GetDesc(); }
}
