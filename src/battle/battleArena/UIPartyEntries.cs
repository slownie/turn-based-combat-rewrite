using Godot;
using System;

public partial class UIPartyEntries : VBoxContainer
{
    [Export] PackedScene partyEntryScene;

    public void Setup(Godot.Collections.Array<BattleActor> partyActors)
    {
        foreach (BattleActor partyActor in partyActors)
        {
            UIPartyEntry partyEntry = partyEntryScene.Instantiate() as UIPartyEntry;
            AddChild(partyEntry);

            partyEntry.Setup(partyActor);

        }
    }
}
