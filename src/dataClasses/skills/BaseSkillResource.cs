using Godot;
using System;

[GlobalClass]
public abstract partial class BaseSkillResource : Resource
{
    [Export] string skillName = "";
    [Export(PropertyHint.MultilineText)] string desc = "";
    [Export] Texture2D icon;

    public string GetSkillName() { return skillName; }
    public string GetDesc() { return desc; }
    public Texture2D GetIcon() { return icon; }
}
