using Godot;
using System;

/*
	A template menu that provides common functionality for other menus.
	Contains a number of button entries that can be navigated with the D-Pad.
	A cursor follows the selected menu entry and player input is forwarded via signals.
*/
public partial class UIListMenu : VBoxContainer
{
	// The scene representing the different menu entries.
	// The scene must be some derivation of [BaseButton].
	[Export] protected PackedScene entryScene;

	// Track all battler list entries in the following array
	protected Godot.Collections.Array<TextureRect> entries = [];

	protected int index = 0;
	public virtual int Index {
		get {return index;}  
		set {
			index = value;
		
			// Clamp
			if (entries.Count <= index) index = 0;
			if (index < 0) index = entries.Count - 1;
		}
	}

    public override void _Ready()
	{
	}

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("MoveDown"))
		{
			Index += 1;
		}

		if (@event.IsActionPressed("MoveUp"))
		{
			Index -= 1;
		}
    }


	protected TextureRect CreateEntry()
	{
		TextureRect newEntry = entryScene.Instantiate() as TextureRect;
		AddChild(newEntry);

		entries.Add(newEntry);

		return newEntry;
	}

	public virtual void OnEntryPressed(TextureRect entry) {}
	public virtual void OnEntryFocused(TextureRect entry) {}
}
