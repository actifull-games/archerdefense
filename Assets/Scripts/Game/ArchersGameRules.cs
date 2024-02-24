using System;
using System.ComponentModel;
using MobileFramework.Game;

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

        private void Start()
        {
            _settings = FindObjectOfType<LevelSettings>();
            var playerContext = PlayerController.GetContext<ArchersPlayerContext>();
            playerContext.MaxPowerCount = _settings.initialPlayerPower;
            playerContext.PropertyChanged += PlayerContextOnPropertyChanged;
            
            UIController.instance.powetSlider.maxValue = playerContext.MaxPowerCount;
            playerContext.PowerCount = _settings.initialPlayerPower;
            
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
    }
}