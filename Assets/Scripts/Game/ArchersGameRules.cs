using System;
using System.Collections.Generic;
using System.ComponentModel;
using MobileFramework.Game;
using Tower;
using UnityEngine;

namespace Game
{
    public class ArchersGameRules : GameRules
    {
        public ArchersGameRules()
        {
            gameContextClass = typeof(ArchersGameContext);
            playerControllerClass = typeof(ArchersPlayerController);
            playerContextClass = typeof(ArchersPlayerContext);
        }

        private LevelSettings _settings;

        public bool IsWin { get; private set; }
        
        public void LevelFailed()
        {
            IsWin = false;
            FinishGame();
        }

        private void Start()
        {
            _settings = FindObjectOfType<LevelSettings>();
            var playerContext = PlayerController.GetContext<ArchersPlayerContext>();
            playerContext.MaxPowerCount = _settings.initialPlayerPower;
            playerContext.PropertyChanged += PlayerContextOnPropertyChanged;
            
            UIController.instance.powetSlider.maxValue = playerContext.MaxPowerCount;
            playerContext.PowerCount = _settings.initialPlayerPower;
            
        }

        protected override void OnBeginPlay()
        {
            if (PlayerController is ArchersPlayerController controller)
            {
                var tower = FindObjectOfType<TowerPawn>();
                controller.Tower = tower;
            }
        }

        private void PlayerContextOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var playerContext = (ArchersPlayerContext)sender;
            if (e.PropertyName == nameof(ArchersPlayerContext.PowerCount))
            {
                UIController.instance.powetSlider.value = playerContext.PowerCount;
                UIController.instance.SetTextPower(playerContext.PowerCount, playerContext.MaxPowerCount);
            }
        }

        public ArchersGameContext TypedGameContext => GetContext<ArchersGameContext>();

        public IReadOnlyList<GameObject> Enemies => _enemies;


        private List<GameObject> _enemies = new();
        
        public void AddEnemy(GameObject obj)
        {
            _enemies.Add(obj);
        }

        public void RemoveEnemy(GameObject obj)
        {
            _enemies.Remove(obj);
        }
    }
}