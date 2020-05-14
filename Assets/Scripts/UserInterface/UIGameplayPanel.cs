using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplayPanel : MonoBehaviour
{
    [SerializeField] private Text _scorelabelText = null;
    [SerializeField] private Slider _playerHealthProgressBaSlider = null;
    [SerializeField] private GameObject _pauseDummy = null;
    [SerializeField] private ScoreManager _scoreManager = null;
    private float _score = 0;

    private void Awake()
    {
        var characterController = FindObjectOfType<PlayerCharacterController>();
        var playerHealth = characterController.GetComponent<CharacterHealthComponent>();
        playerHealth.EventHealthChange += OnHealthChange;
        _scoreManager.EventCurrentScoreChanged += OnCurrentScoreChanged;
    }

    private void Start()
    {
        _scorelabelText.text = "0";
        _playerHealthProgressBaSlider.value = 1;
    }

    private void OnDestroy()
    {
        var characterController = FindObjectOfType<PlayerCharacterController>();
        if (characterController != null)
        {
            var playerHealth = characterController.GetComponent<CharacterHealthComponent>();
            playerHealth.EventHealthChange -= OnHealthChange;
        }
        
        _scoreManager.EventCurrentScoreChanged -= OnCurrentScoreChanged;
        
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

    private void OnCurrentScoreChanged(int score)
    {
        _scorelabelText.text = score.ToString();
    }
}
