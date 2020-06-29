using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ShoriGames.HFSM;
using UnityEngine;

public class PlayerDetectTransition : Transition<IState, IState>
{
    
    private readonly List<AISensor> _sernsors;
    private Character _player = null;
    private readonly bool _transitWhenDetect;
    
    public PlayerDetectTransition(
        bool transitWhenDetect,
        Character bot, 
        Character player, 
        IState ownerState, 
        IState targetState, 
        int priority = 0) 
        : base(ownerState, targetState, priority)
    {
        _sernsors = bot.GetComponentsInChildren<AISensor>().ToList();
        _transitWhenDetect = transitWhenDetect;
        _player = player;
    }

    public override bool CanTransit()
    {
        if (_player.IsDead)
            return false;
        
        bool isPlayerDetect = _sernsors.Any(s => s.IsTargetDetected(_player.transform));
        return _transitWhenDetect ? isPlayerDetect : !isPlayerDetect;
    }
}
