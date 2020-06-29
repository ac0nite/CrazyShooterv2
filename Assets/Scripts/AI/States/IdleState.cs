using System.Collections;
using System.Collections.Generic;
using ShoriGames.HFSM;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class IdleState : StateBase
{

    private readonly AIMotor _motor;
    // private Vector3 _startingPosition = Vector3.zero;
    // private Vector3 _targetPosition = Vector3.zero;
    // private Vector3 _directionMovement = Vector3.zero;
    public IdleState(Character character, Character player) : base(character, player)
    {
        _motor = Character.GetComponent<AIMotor>();
    }

    public override void Init()
    {
        base.Init();
        var direction = Random.onUnitSphere;
        direction.y = 0f;
        direction.Normalize();
        _motor?.SetMovementTarget(direction);
    }

    public override void OnStateEnter(IState fromState)
    {
        
        _motor?.SetMovementTarget(Character.transform.position);
        // Debug.Log("OnStateEnter");
        // base.OnStateEnter(fromState);
        // // _startingPosition = Character.transform.position;
        //
        // var direction = Random.onUnitSphere;
        // direction.y = 0f;
        // direction.Normalize();
        
        //_motor.SetMovementTarget(direction);

        //_targetPosition = _startingPosition + direction * 5f;
        // _targetPosition = _startingPosition + Vector3.forward * 5f;
        //
        // _directionMovement = _targetPosition - Character.transform.position;
    }
    
    // public override void Update()
    // {
    //     base.Update();
    //
    //     //var dist = (_targetPosition - Character.transform.position).magnitude;
    //     if ((_targetPosition - Character.transform.position).magnitude > 5f)
    //     {
    //         _directionMovement = _startingPosition - Character.transform.position;
    //     }
    //     else if((_startingPosition - Character.transform.position).magnitude > 5f)
    //     {
    //         _directionMovement = _targetPosition - Character.transform.position;
    //     }
    //
    //     MoveToPosition(_directionMovement);
    // }
    //
    // private void MoveToPosition(Vector3 movementVector)
    // {
    //     //var movementVector = (position - Character.transform.position).normalized;
    //     _motor.SetMovementTarget(movementVector);
    //     //_motor.ChangePlayerMovementDirectionChanged(movementVector.normalized);
    // }
}
