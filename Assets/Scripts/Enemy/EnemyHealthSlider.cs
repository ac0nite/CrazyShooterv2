using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSlider : MonoBehaviour
{

    [SerializeField] private Slider _sliderHealthCharacter = null;
    [SerializeField] private Canvas _canvasHealthCharacter = null;
    private CharacterHealthComponent _characterHealthComponent = null;

    private Camera _worldCamera = null;
    private RectTransform _transform = null;

    private void Awake()
    {

        _worldCamera = Camera.main;
        _sliderHealthCharacter.value = 1f;
        _canvasHealthCharacter.worldCamera = _worldCamera;
        _transform = GetComponent<RectTransform>();
        _characterHealthComponent = GetComponentInParent<CharacterHealthComponent>();
    }

    private void Start()
    {
        _characterHealthComponent.EventHealthChange += OnHealthChange;
    }

    void Update()
    {
        _transform.LookAt(_worldCamera.transform.position, Vector3.up);
    }

    private void OnHealthChange(CharacterHealthComponent _healthCharacter, float _health)
    {
        if (_health == 0f)
        {
            _canvasHealthCharacter.gameObject.SetActive(false);
            _characterHealthComponent.EventHealthChange -= OnHealthChange;
        }
        else
        {
            _sliderHealthCharacter.value = _health / _healthCharacter.MaxHealth;
        }
    }
}
