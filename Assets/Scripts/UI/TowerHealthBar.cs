using System;
using Attributes;
using MobileFramework.Abilities;
using Tower;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TowerHealthBar : MonoBehaviour
    {
        public GameObject Player;


        private VitalityAttributes _attributes;
        private Slider _slider;
        
        private void Start()
        {
            var gas = Player.GetComponent<GameplayAbilitySystem>();
            _slider = GetComponent<Slider>();
            _attributes = gas.GetAttributeSet<VitalityAttributes>();
            _slider.minValue = 0.0f;
            _slider.maxValue = _attributes.MaxHealth.CurrentValue;
            _attributes.Health.Changed.AddListener(UpdateHealth);
        }

        private void OnEnable()
        {
            _slider.minValue = 0.0f;
            _slider.maxValue = _attributes.MaxHealth.CurrentValue;
            UpdateHealth();
        }

        private void UpdateHealth()
        {
            _slider.value = _attributes.Health.CurrentValue;
        }
    }
}