﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplayPanel : MonoBehaviour
{
    [SerializeField] private Text _scorelabelText = null;
    [SerializeField] private Slider _playerHealthProgressBaSlider = null;
    [SerializeField] private GameObject _pauseDummy = null;

    private void Awake()
    {
        var characterController = FindObjectOfType<PlayerCharacterController>();
        var playerHealth = characterController.GetComponent<CharacterHealthComponent>();
        playerHealth.EventHealthChange += OnHealthChange;
    }

    private void OnDestroy()
    {
        var characterController = FindObjectOfType<PlayerCharacterController>();
        if (characterController != null)
        {
            var playerHealth = characterController.GetComponent<CharacterHealthComponent>();
            playerHealth.EventHealthChange -= OnHealthChange;
        }
    }

    private void OnHealthChange(CharacterHealthComponent healthComponent, float health)
    {
        _playerHealthProgressBaSlider.value = health / healthComponent.MaxHealth;
    }

    public void OnPauseButtonClicked()
    {
        Debug.Log("OnPauseButtonClicked");
        _pauseDummy.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnResumeButtonClicked()
    {
        Debug.Log("OnResumeButtonClicked");
        _pauseDummy.SetActive(false);
        Time.timeScale = 1f;
    }
}
