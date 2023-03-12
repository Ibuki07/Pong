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
            // PanelOpen�{�^��
            var panelOpenButton = optionButtons[(int)OPTION_TYPE.PanelOpen];
            panelOpenButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _panelOpen.Value = true;
                });

            // GraphicSetting�{�^��
            var graphicSettingButton = optionButtons[(int)OPTION_TYPE.GraphicSetting];
            graphicSettingButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    // ���ݒ�E�Q�[���I����ʂ����
                    _audioSetting.Value = false;
                    _quitGame.Value = false;
                    // �O���t�B�b�N�ݒ��ʂ��J��
                    _graphicSetting.Value = true;
                });
            // �O���t�B�b�N�ݒ�{�^���Ƀ}�E�X�I�[�o�[�������ɕ\�����郁�b�Z�[�W��ݒ肷��
            SetInfoMessage(graphicSettingButton, OPTION_TYPE.GraphicSetting);

            // AudioSetting�{�^��
            var audioSettingButton = optionButtons[(int)OPTION_TYPE.AudioSetting];
            audioSettingButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    // �O���t�B�b�N�ݒ�E�Q�[���I����ʂ����
                    _graphicSetting.Value = false;
                    _quitGame.Value = false;
                    // ���ݒ��ʂ��J��
                    _audioSetting.Value = true;
                });
            // ���ݒ�{�^���Ƀ}�E�X�I�[�o�[�������ɕ\�����郁�b�Z�[�W��ݒ肷��
            SetInfoMessage(audioSettingButton, OPTION_TYPE.AudioSetting);

            // QuitTitle�{�^��
            // QuitTitle�{�^���Ƀ}�E�X�I�[�o�[�������ɕ\�����郁�b�Z�[�W��ݒ肷��
            SetInfoMessage(optionButtons[(int)OPTION_TYPE.QuitTitle], OPTION_TYPE.QuitTitle);

            // QuitGame�{�^��
            var quitGameButton = optionButtons[(int)OPTION_TYPE.QuitGame];
            quitGameButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    // �O���t�B�b�N�ݒ�E���ݒ��ʂ����
                    _graphicSetting.Value = false;
                    _audioSetting.Value = false;
                    // �Q�[���I����ʂ��J��
                    _quitGame.Value = true;
                });
            // �Q�[���I���{�^���Ƀ}�E�X�I�[�o�[�������ɕ\�����郁�b�Z�[�W��ݒ肷��
            SetInfoMessage(quitGameButton, OPTION_TYPE.QuitGame);

            // PanelClose�{�^��
            var panelCloseButton = optionButtons[(int)OPTION_TYPE.PanelClose];
            panelCloseButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    // �S�Ă̐ݒ��ʂ����
                    _panelOpen.Value = false;
                    _graphicSetting.Value = false;
                    _audioSetting.Value = false;
                    _quitGame.Value = false;
                });
            // �p�l�������{�^���Ƀ}�E�X�I�[�o�[�������ɕ\�����郁�b�Z�[�W��ݒ肷��
            SetInfoMessage(panelCloseButton, OPTION_TYPE.PanelClose);

            // Esc�L�[�������ꂽ���Ƀp�l�����J����
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
                        // �S�Ă̐ݒ�p�l�������
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

        // QuitGame�����Z�b�g����
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