using System.Collections;
using System.Collections.Generic;
using ShoriGames.HFSM;
using UnityEngine;

public class AIStateMashineFactory
{
    public static StateMachine CreateDefaultStateMashine(Character character)
    {
        var player = GameObject.FindObjectOfType<PlayerCharacterController>().Character;
        var detectFielView = character.GetComponent<DetectInsideFielViewAISensor>();
        var detectInsideRadius = character.GetComponent<DetectInsideRadiusAISensor>();
        
        var idle_stsate = new IdleState(character, player);
        
        var follow_state = new FollowPlayertState(character, player);
        var attack_state = new AttackToPlayerState(character, player);
        var submachineExitState = new StateMachine.ExitState();
        
        
        var followToAttack_transition = new DelegateCheckTransition<FollowPlayertState, AttackToPlayerState>(
            (owner, target) =>
            {
                if (owner.Player.IsDead)
                    return false;
                var distance = (owner.Character.transform.position - owner.Player.transform.position).magnitude;
                return distance < detectInsideRadius.DetectionRadius;
            }, 
            follow_state, 
            attack_state);
        
        var followToExit_transition = new DelegateCheckTransition<FollowPlayertState, StateMachine.ExitState>(
            (owner, target) =>
            {
                if (owner.Player.IsDead)
                    return true;
                var distance = (owner.Character.transform.position - owner.Player.transform.position).magnitude;
                return distance > detectFielView.Distance;
            }, 
            follow_state, 
            submachineExitState);
        
        var attackToExit_transition = new DelegateCheckTransition<AttackToPlayerState, StateMachine.ExitState>(
            (owner, target) =>
            {
                if (owner.Player.IsDead)
                    return true;
                var distance = (owner.Character.transform.position - owner.Player.transform.position).magnitude;
                return distance > detectInsideRadius.DetectionRadius;
            }, 
            attack_state, 
            submachineExitState);

        var attackSubmachine = StateMachine.Create(follow_state, followToAttack_transition, attackToExit_transition, followToExit_transition);
        var attackSubmachineState = new SubmachineState(attackSubmachine);
        
        var idleToAttack_transition = new PlayerDetectTransition(true, character, player, idle_stsate, attackSubmachineState);
        //var followToIdle_transition = new PlayerDetectTransition(false, character, player, follow_state, idle_stsate);
        var attackToIdle_transition = new StateFinishedTransition(attackSubmachineState, idle_stsate);
        
        return StateMachine.Create(idle_stsate, idleToAttack_transition, attackToIdle_transition);
    }
}
