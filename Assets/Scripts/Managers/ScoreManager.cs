using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private LevelGenerationController _levelGeneration = null;
    [SerializeField] private Character _player = null;
    private int _currentScore = 0;
    private int _currentDead = 0;
    public Action<int, int> EventCurrentScoreChanged;

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

    private void OnCharacterDead(CharacterHealthComponent characterHealthComponent, float _score)
    {
        if (_player.GetComponent<CharacterHealthComponent>() != characterHealthComponent)
        {
            _currentDead++;
            _currentScore += (int)_score;
            EventCurrentScoreChanged?.Invoke(_currentDead, _currentScore);   
        }
    }
}
