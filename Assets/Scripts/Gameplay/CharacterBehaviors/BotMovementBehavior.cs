using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMovementBehavior : CharacterMovemevtBehavior
{
    public override void ChangePlayerMovementDirectionChanged(Vector3 targetMovementVector)
    {
        //base.ChangePlayerMovementDirectionChanged(targetMovementVector);
        _targetMovementVelocity = targetMovementVector * _speed;
    }
}
