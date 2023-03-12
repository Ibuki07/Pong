using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Option
{
    public class OptionPanelInput : System.IDisposable
    {
        public IReadOnlyReactiveProperty<bool> PanelOpen => _panelOpen;
        public IReadOnlyReactiveProperty<bool> GraphicSetting => _graphicSetting;
        public IReadOnlyReactiveProperty<bool> AudioSetting => _audioSetting;
        public IReadOnlyReactiveProperty<bool> QuitGame => _quitGame;
        public IReadOnlyReactiveProperty<int> InfoMessage => _infoMessage;

        private readonly ReactiveProperty<bool> _panelOpen = new ReactiveProperty<bool>(false);
        private readonly ReactiveProperty<bool> _graphicSetting = new ReactiveProperty<bool>(false);
        private readonly ReactiveProperty<bool> _audioSetting = new ReactiveProperty<bool>(false);
        private readonly ReactiveProperty<bool> _quitGame = new ReactiveProperty<bool>(false);
        private readonly ReactiveProperty<int> _infoMessage = new ReactiveProperty<int>(0);

        private System.IDisposable _paenlOpenDisposable;

        public OptionPanelInput(List<Button> optionButtons)
        {
            // PanelOpenボタン
            var panelOpenButton = optionButtons[(int)OPTION_TYPE.PanelOpen];
            panelOpenButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _panelOpen.Value = true;
                });

            // GraphicSettingボタン
            var graphicSettingButton = optionButtons[(int)OPTION_TYPE.GraphicSetting];
            graphicSettingButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    // 音設定・ゲーム終了画面を閉じる
                    _audioSetting.Value = false;
                    _quitGame.Value = false;
                    // グラフィック設定画面を開く
                    _graphicSetting.Value = true;
                });
            // グラフィック設定ボタンにマウスオーバーした時に表示するメッセージを設定する
            SetInfoMessage(graphicSettingButton, OPTION_TYPE.GraphicSetting);

            // AudioSettingボタン
            var audioSettingButton = optionButtons[(int)OPTION_TYPE.AudioSetting];
            audioSettingButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    // グラフィック設定・ゲーム終了画面を閉じる
                    _graphicSetting.Value = false;
                    _quitGame.Value = false;
                    // 音設定画面を開く
                    _audioSetting.Value = true;
                });
            // 音設定ボタンにマウスオーバーした時に表示するメッセージを設定する
            SetInfoMessage(audioSettingButton, OPTION_TYPE.AudioSetting);

            // QuitTitleボタン
            // QuitTitleボタンにマウスオーバーした時に表示するメッセージを設定する
            SetInfoMessage(optionButtons[(int)OPTION_TYPE.QuitTitle], OPTION_TYPE.QuitTitle);

            // QuitGameボタン
            var quitGameButton = optionButtons[(int)OPTION_TYPE.QuitGame];
            quitGameButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    // グラフィック設定・音設定画面を閉じる
                    _graphicSetting.Value = false;
                    _audioSetting.Value = false;
                    // ゲーム終了画面を開く
                    _quitGame.Value = true;
                });
            // ゲーム終了ボタンにマウスオーバーした時に表示するメッセージを設定する
            SetInfoMessage(quitGameButton, OPTION_TYPE.QuitGame);

            // PanelCloseボタン
            var panelCloseButton = optionButtons[(int)OPTION_TYPE.PanelClose];
            panelCloseButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    // 全ての設定画面を閉じる
                    _panelOpen.Value = false;
                    _graphicSetting.Value = false;
                    _audioSetting.Value = false;
                    _quitGame.Value = false;
                });
            // パネルを閉じるボタンにマウスオーバーした時に表示するメッセージを設定する
            SetInfoMessage(panelCloseButton, OPTION_TYPE.PanelClose);

            // Escキーが押された時にパネルを開閉する
            _paenlOpenDisposable = Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Escape))
                .Subscribe(_ =>
                {
                    if(!_panelOpen.Value)
                    {
                        _panelOpen.Value = true;
                    }
                    else
                    {
                        // 全ての設定パネルを閉じる
                        _panelOpen.Value = false;
                        _graphicSetting.Value = false;
                        _audioSetting.Value = false;
                        _quitGame.Value = false;
                    }
                });

        }

        public void Dispose()
        {
            _paenlOpenDisposable?.Dispose();
        }

        // QuitGameをリセットする
        public void ResetQuitGame()
        {
            _quitGame.Value = false;
        }

        private void SetInfoMessage(Button button, OPTION_TYPE optionType)
        {
            button.OnPointerEnterAsObservable()
                .Subscribe(_ =>
                {
                    _infoMessage.Value = (int)optionType;
                });
        }
    }
}