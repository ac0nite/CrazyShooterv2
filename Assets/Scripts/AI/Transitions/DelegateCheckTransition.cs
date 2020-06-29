using System;
using System.Collections;
using System.Collections.Generic;
using ShoriGames.HFSM;
using UnityEngine;
using UnityEngine.Assertions;

public class DelegateCheckTransition<TOwner, TTarget> : Transition<TOwner, TTarget>
    where TOwner:IState
    where TTarget:IState
{
    private readonly Func<TOwner, TTarget, bool> _canTransitionDelegate;
    public DelegateCheckTransition(
        Func<TOwner, TTarget, bool> canTransitionDelegate,
        TOwner ownerState, 
        TTarget targetState, 
        int priority = 0) : base(ownerState, targetState, priority)
    {
        Assert.IsNotNull(canTransitionDelegate);
        _canTransitionDelegate = canTransitionDelegate;
    }

    public override bool CanTransit()
    {
        return _canTransitionDelegate.Invoke(OwnerState, TargetState);
    }
}
