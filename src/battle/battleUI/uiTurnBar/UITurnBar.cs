using Godot;
using System;

public partial class UITurnBar : Control
{
    [Export] PackedScene _uiTurnIconScene;
    TextureRect _background;
    Control _icons;

    public override void _Ready()
    {
        _background = GetNode<TextureRect>("Background");
        _icons = GetNode<Control>("Background/Icons");
    }

    // Initialize the turn bar, passing in all the battlers that we want to display
    public void Setup(Godot.Collections.Array<BattleActor> actors, Godot.Collections.Array<FollowerActor> followerActors)
    {
        foreach (BattleActor actor in actors)
        {
            UITurnIcon turnIcon = _uiTurnIconScene.Instantiate() as UITurnIcon;
            _icons.AddChild(turnIcon);

            turnIcon.Setup(actor);
            turnIcon.PositionRange = new Vector2(turnIcon.Size.X / 2.0f, _background.Size.X - turnIcon.Size.X / 2.0f);

            if (actor.GetIsPlayer())
            {
                turnIcon.Position = new Vector2(0, 8);
            } else {
                turnIcon.Position = new Vector2(0, -16);
            }

            actor.ReadinessChanged += (readiness) => turnIcon.Progress = readiness / 100.0;
        }

        foreach (FollowerActor followerActor in followerActors)
        {
            BattleActor leaderActor = followerActor.GetLeaderActor();

            UITurnIcon turnIcon = _uiTurnIconScene.Instantiate() as UITurnIcon;
            _icons.AddChild(turnIcon);

            turnIcon.Setup(leaderActor);
            turnIcon.PositionRange = new Vector2(turnIcon.Size.X / 2.0f, _background.Size.X - turnIcon.Size.X / 2.0f);

            if (leaderActor.GetIsPlayer())
            {
                turnIcon.Position = new Vector2(0, 8);
            } else {
                turnIcon.Position = new Vector2(0, -16);
            }

            followerActor.ReadinessChanged += (readiness) => turnIcon.Progress = readiness / 100.0;
        }
    }

    private void OnHPDepleted()
    {
        
    }
}
