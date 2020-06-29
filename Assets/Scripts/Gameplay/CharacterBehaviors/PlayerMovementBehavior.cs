using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBehavior : CharacterMovemevtBehavior
{
    private CharacterHealthComponent _playerHealth = null;
    private bool _activeMovement = true;
    public Action EventRevivePlayer;
    public override void Awake()
    {
        base.Awake();
        _playerHealth = GetComponent<CharacterHealthComponent>();
        _playerHealth.EventCharacterDead += OnCharacterDead;
    }

    public override void Update()
    {
        NewTargetMovement();
        base.Update();
    }

    private void OnDestroy()
    {
        _playerHealth.EventCharacterDead -= OnCharacterDead;
    }

    public override void ChangePlayerMovementDirectionChanged(Vector3 targetMovementVector)
    {
        if(_activeMovement)
            base.ChangePlayerMovementDirectionChanged(targetMovementVector);
    }

    public override void OnPlayerLookPointChanged(Vector3 lookPoint)
    {
        if(_activeMovement)
            base.OnPlayerLookPointChanged(lookPoint);
    }

    private void OnCharacterDead(CharacterHealthComponent _characterHealthComponent, float _score)
    {
        _activeMovement = false;
        StartCoroutine(NewSpawnPlayer());
    }

    public void RevivePlayer()
    {
        _activeMovement = true;
    }

    IEnumerator NewSpawnPlayer()
    {
        SettingsManager.Instance.Message.Send("You are dead bastard!", "", 0, true);
        yield return new WaitForSeconds(5f);
        SettingsManager.Instance.Message.Send("Player spawn! Continue to play ...", "", 0, true);
        EventRevivePlayer?.Invoke();
    }
}
