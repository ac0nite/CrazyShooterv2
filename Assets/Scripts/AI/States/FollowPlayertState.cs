using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FollowPlayertState : StateBase
{
    private readonly AIMotor _motor;
    public FollowPlayertState(Character character, Character player) : base(character, player)
    {
        _motor = Character.GetComponent<AIMotor>();
    }

    public override void Update()
    {
        base.Update();
        _motor.SetMovementTarget(Player.transform.position);
    }
}
