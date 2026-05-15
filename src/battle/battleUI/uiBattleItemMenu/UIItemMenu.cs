using Godot;
using System;

public partial class UIItemMenu : UIBattleMenuBase
{
    [Export] PackedScene itemMenuEntry;
    [Export] PackedScene targetCursorScene;

    [Signal] public delegate void ItemSelectedEventHandler(UseableItemResource selectedItem);
    [Signal] public delegate void ItemSelectionCancelledEventHandler();

    Godot.Collections.Array<InventoryItem> _itemList = [];
    Godot.Collections.Array<UIItemMenuEntry> _entries = [];
    int _index = 0;
    int index
    {
        get { return _index; }
        set
        {
            _index = value;

            // Clamp
            if (_entries.Count <= _index) _index = 0;
			if (_index < 0) _index = _entries.Count - 1;

            if (_targetCursor != null)
            {
                _targetCursor.Position = new Vector2(_entries[_index].Position.X, _entries[_index].Position.Y + 16);
            }
        }
    }

    UITargetCursor _targetCursor;

    public override void _Ready()
    {
        SetProcessInput(false);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("AButton"))
        {
            UseableItemResource selectedItem = _itemList[index].GetItemResource() as UseableItemResource;
            EmitSignal(SignalName.ItemSelected, selectedItem);
        }

		// Quit Selection
		if (@event.IsActionPressed("BButton"))
		{
			EmitSignal(SignalName.ItemSelectionCancelled);
		}

        if (@event.IsActionPressed("MoveDown"))
		{
			index += 1;
		}

		if (@event.IsActionPressed("MoveUp"))
		{
			index -= 1;
		}
    }

    public void Setup(Godot.Collections.Array<InventoryItem> itemList)
    {
        _itemList = itemList;

        for (int i=0; i < _itemList.Count; i++)
        {
            UseableItemResource useableItem = _itemList[i].GetItemResource() as UseableItemResource;
            UIItemMenuEntry itemEntry = itemMenuEntry.Instantiate() as UIItemMenuEntry;
            AddChild(itemEntry);

            itemEntry.Position = new Vector2(itemEntry.Position.X, itemEntry.Position.Y + (32 * i));
            itemEntry.Setup(_itemList[i]);
            _entries.Add(itemEntry);
        }

        _targetCursor = targetCursorScene.Instantiate() as UITargetCursor;
        AddChild(_targetCursor);

        index = 0;

        SetProcessInput(true);
    }
}
