using System.Collections;
using System.Collections.Generic;
using ShoriGames.HFSM;
using UnityEngine;

public class StateBase : State
{
    public Character Character { get; private set; }
    public Character Player { get; private set; }

    public StateBase(Character character, Character player)
    {
        Character = character;
        Player = player;
    }
}
