using System;
using System.Collections;
using System.Collections.Generic;
using ShoriGames.HFSM;
using UnityEngine;

public class AICharacterController : CharacterController
{
    private StateMachine _stateMachine = null;

    private void Start()
    {
        _stateMachine = AIStateMashineFactory.CreateDefaultStateMashine(Character);
        _stateMachine.Start();
    }

    private void Update()
    {
        _stateMachine.Transit();
        _stateMachine.Update();
    }
}
