using Godot;
using System;

[GlobalClass]
public partial class MoveToTargetEffect : BattleSequenceEffectResource
{
    Vector2 targetPosition;
    float moveSpeed = 1.0f;

    public async override void ExecuteSequence()
    {
        
    }
}
