using Godot;
using System;

[GlobalClass]
public partial class BaseItemResource : Resource
{
    [Export] string itemName = "";
    [Export(PropertyHint.MultilineText)] string desc = "";
    [Export] Texture2D icon;

    public string GetItemName() { return itemName; }
    public string GetDesc() { return desc; }
    public Texture2D GetIcon() { return icon; }
}
