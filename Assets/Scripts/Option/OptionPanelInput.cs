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

        public IReadOnlyReactiveProperty<bool> AudioSetting => _audioSetting;
        private readonly ReactiveProperty<bool> _audioSetting = new ReactiveProperty<bool>(false);

        public IReadOnlyReactiveProperty<bool> GraphicSetting => _graphicSetting;
        private readonly ReactiveProperty<bool> _graphicSetting = new ReactiveProperty<bool>(false);

        public IReadOnlyReactiveProperty<int> InfoMessage => _infoMessage;
        private readonly ReactiveProperty<int> _infoMessage = new ReactiveProperty<int>(0);



        public OptionPanelInput(
            List<Button> buttons
            )
        {
            var buttonType = OptionCore.ButtonType.PanelOpen;
            buttons[(int)buttonType].OnClickAsObservable().Subscribe(_ =>
            {
                _panelOpen.Value = true;
            });

            buttonType = OptionCore.ButtonType.GameSetting;
            SetInfoMessage(buttons[(int)buttonType], buttonType);

            buttonType = OptionCore.ButtonType.GraphicSetting;
            buttons[(int)buttonType].OnClickAsObservable().Subscribe(_ =>
            {
                _audioSetting.Value = false;
                _graphicSetting.Value = true;
            });
            SetInfoMessage(buttons[(int)buttonType], buttonType);

            buttonType = OptionCore.ButtonType.AudioSetting;
            buttons[(int)buttonType].OnClickAsObservable().Subscribe(_ =>
            {
                _graphicSetting.Value = false;
                _audioSetting.Value = true;
            });
            SetInfoMessage(buttons[(int)buttonType], buttonType);


            buttonType = OptionCore.ButtonType.QuitTitle;
            SetInfoMessage(buttons[(int)buttonType], buttonType);

            buttonType = OptionCore.ButtonType.QuitGame;
            SetInfoMessage(buttons[(int)buttonType], buttonType);

            buttonType = OptionCore.ButtonType.PanelClose;
            SetInfoMessage(buttons[(int)buttonType], buttonType);
            buttons[(int)buttonType].OnClickAsObservable().Subscribe(_ =>
            {
                _audioSetting.Value = false;
                _panelOpen.Value = false;
            });




        }

        private void SetInfoMessage(Button button, OptionCore.ButtonType messageType)
        {
            button
            .OnPointerEnterAsObservable()
            .Subscribe(_ =>
            {
                _infoMessage.Value = (int)messageType;
            });
        }
    }
}