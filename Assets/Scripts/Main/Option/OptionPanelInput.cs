using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Option
{
    public class OptionPanelInput
    {
        public IReadOnlyReactiveProperty<bool> PanelOpen => _panelOpen;
        private readonly ReactiveProperty<bool> _panelOpen = new ReactiveProperty<bool>(false);

        public IReadOnlyReactiveProperty<bool> AudioSettingOpen => _audioSettingOpen;
        private readonly ReactiveProperty<bool> _audioSettingOpen = new ReactiveProperty<bool>(false);

        public IReadOnlyReactiveProperty<int> InfoMessage => _infoMessage;
        private readonly ReactiveProperty<int> _infoMessage = new ReactiveProperty<int>(0);

        private Button _panelOpenButton;
        private Button _gameSettingButton;
        private Button _audioSettingButton;
        private Button _quitGameButton;
        private Button _panelCloseButton;
        
        public enum InfoMessageType
        {
            None = 0,
            GameSetting,
            AudioSetting,
            QuitGame,
            PanelClose,
        }

        public OptionPanelInput(
            Button panelOpenButton,
            Button gameSettingButton,
            Button audioSettingButton,
            Button quitGameButton,
            Button panelCloseButton
            )
        {
            _panelOpenButton = panelOpenButton;
            _gameSettingButton = gameSettingButton;
            _audioSettingButton = audioSettingButton;
            _quitGameButton = quitGameButton;
            _panelCloseButton = panelCloseButton;

            _panelOpenButton.OnClickAsObservable().Subscribe(_ =>
            {
                _panelOpen.Value = true;
            });

            _audioSettingButton.OnClickAsObservable().Subscribe(_ =>
            {
                _audioSettingOpen.Value = true;
            });


            _panelCloseButton.OnClickAsObservable().Subscribe(_ =>
            {
                _audioSettingOpen.Value = false;
                _panelOpen.Value = false;
            });


            _gameSettingButton.OnPointerEnterAsObservable().Subscribe(_ =>
            {
                _infoMessage.Value = (int)InfoMessageType.GameSetting;
            });
            _audioSettingButton.OnPointerEnterAsObservable().Subscribe(_ =>
            {
                _infoMessage.Value = (int)InfoMessageType.AudioSetting;
            });
            _quitGameButton.OnPointerEnterAsObservable().Subscribe(_ =>
            {
                _infoMessage.Value = (int)InfoMessageType.QuitGame;
            });
            _panelCloseButton.OnPointerEnterAsObservable().Subscribe(_ =>
            {
                _infoMessage.Value = (int)InfoMessageType.PanelClose;   
            });

        }
    }
}