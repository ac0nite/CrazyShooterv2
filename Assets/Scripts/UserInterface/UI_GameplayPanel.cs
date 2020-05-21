using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameplayPanel : UI_Panel
{
    [SerializeField] private Text _scorelabelText = null;
    [SerializeField] private Slider _playerHealthProgressBaSlider = null;
    //[SerializeField] private GameObject _pauseDummy = null;
    //[SerializeField] private GameObject _inventoryPanelDummy = null;
    [SerializeField] private ScoreManager _scoreManager = null;

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
        //Debug.Log("OnPauseButtonClicked");
        //_pauseDummy.SetActive(true);
        //Time.timeScale = 0f;
        UIManager.Instance.ShowPanel(UI_PanelType.Pause);
    }
    
    public void OnInventoryPanelButtonClicked()
    {
        //Debug.Log("OnInventoryPanelButtonClicked");
        //_inventoryPanelDummy.SetActive(true);
        ////Time.timeScale = 0f;
        UIManager.Instance.ShowPanel(UI_PanelType.Inventory);
    }

    private void OnCurrentScoreChanged(int score)
    {
        _scorelabelText.text = score.ToString();
    }
}
