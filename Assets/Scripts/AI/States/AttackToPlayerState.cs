using System.Collections;
using System.Collections.Generic;
using ShoriGames.HFSM;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AttackToPlayerState : StateBase
{
    private readonly AIMotor _motor;
    private readonly float _timer_default = 0.5f;
    
    private float _timer;
    public AttackToPlayerState(Character character, Character player) : base(character, player)
    {
        _motor = character.GetComponent<AIMotor>();
        _timer = _timer_default;
    }

    public override void OnStateEnter(IState fromState)
    {
        base.OnStateEnter(fromState);
        _motor.SetMovementTarget(Character.transform.position);
    }

    public override void Update()
    {
        base.Update();
        _motor.SetFreeLookTarget(true, Player.transform.position);
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            if (!Character.IsDead)
            {
                Character.Shoot(false);
                //Debug.Log($"Shoot!");   
            }
            _timer = _timer_default;
        }
    }

    public override void OnStateExit(IState toState)
    {
        base.OnStateExit(toState);
        //_motor.SetFreeLookTarget(false, Character.transform.position);
    }
}
