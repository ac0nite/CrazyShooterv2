using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;


public class MovementToTargetDirectionAIMotor : AIMotor
{
    private BotMovementBehavior _botMovement = null;
    private Enemy _bot = null;

    private void Awake()
    {
        _botMovement = GetComponent<BotMovementBehavior>();
        _bot = GetComponent<Enemy>();
        SetMovementTarget(transform.position);
        //SetMovementTarget(Vector3.zero);
    }

    private void Update()
    {
        if (!_bot.IsDead)
        {
            if ((MovementTargetPoint - transform.position).magnitude > 0.7f)
            {
                var movementDirection = (MovementTargetPoint - transform.position).normalized;
                _botMovement.ChangePlayerMovementDirectionChanged(movementDirection);
                if (!IsFreeLookEnabled)
                {
                    _botMovement.OnPlayerLookPointChanged(MovementTargetPoint);   
                }
            }
            else
            {
                _botMovement.ChangePlayerMovementDirectionChanged(Vector3.zero); 
            }

            if (IsFreeLookEnabled)
            {
                _botMovement.OnPlayerLookPointChanged(FreeLookTargetPoint);
            }
        }
    }
}
