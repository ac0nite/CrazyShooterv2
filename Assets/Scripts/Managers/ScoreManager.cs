using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private LevelGenerationController _levelGeneration = null;
    [SerializeField] private Character _player = null;
    private int _currentScore = 0;
    public Action<int> EventCurrentScoreChanged;

    private void Start()
    {
        _levelGeneration.EventCharacterSpawned += OnCharacterSpawned;
    }

    private void OnDestroy()
    {
        if (_levelGeneration != null)
        {
            _levelGeneration.EventCharacterSpawned -= OnCharacterSpawned;
        }
    }

    private void OnCharacterSpawned(Character character)
    {
        character.HealthComponent.EventCharacterDead += OnCharacterDead;
        //как отписаться!!!
    }

    private void OnCharacterDead(CharacterHealthComponent characterHealthComponent)
    {
        if (_player.GetComponent<CharacterHealthComponent>() != characterHealthComponent)
        {
            _currentScore++;
            EventCurrentScoreChanged?.Invoke(_currentScore);   
        }
    }
}
