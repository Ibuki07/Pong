using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Option
{
    public class OptionPanelPresentation
    {
        private CanvasGroup _optionPanel;
        private CanvasGroup _titleLogoBordor;
        private CanvasGroup _buttonSpace;
        private CanvasGroup _infoMessageBordor;
        private Text _infoMessageText;

        // �A�j���[�V�����p�����[�^�[
        private float _verticalMoveValue = 80f;
        private float _horizontalMoveValue = 80f;
        private float _fadeOutValue = 0f;
        private float _fadeInValue = 1.0f;
        private float _duration = 0.5f;
        private float _fastDuration = 0.25f;
        private float _delayTime = 0.25f;

        // �{�^�����Ƃ̐�����
        private readonly string[] info_message_text =
        {
            "",
            "�O���t�B�b�N�ݒ��ʂ�\�����܂��B",
            "���ʐݒ��ʂ�\�����܂��B",
            "�^�C�g���֖߂�܂��B",
            "�Q�[�����I�����܂��B",
            "�I�v�V������ʂ���܂��B",
        };

        public OptionPanelPresentation(
            OptionPanelInput panelInput,
            CanvasGroup optionPanel,
            CanvasGroup titleLogoBordor,
            CanvasGroup buttonSpace,
            CanvasGroup infoMessageBordor,
            Text infoMessageText)
        {
            _optionPanel = optionPanel;
            _titleLogoBordor = titleLogoBordor;
            _buttonSpace = buttonSpace;
            _infoMessageBordor = infoMessageBordor;
            _infoMessageText = infoMessageText;

            // InfoMessage�̕ύX���Ď����A�\�������������؂�ւ���
            panelInput.InfoMessage
                .Subscribe(infoMessage =>
                {
                    SetInfoMessageText((OPTION_TYPE)infoMessage);
                });

            // PanelOpen��true�ɂȂ������A��������\������
            panelInput.PanelOpen
                .Where(isPanelOpen => isPanelOpen)
                .Subscribe(_ =>
                {
                    SetInfoMessageText(OPTION_TYPE.PanelOpen);
                });
        }

        // �{�^�����Ƃ̐�������ݒ肷�郁�\�b�h
        private void SetInfoMessageText(OPTION_TYPE buttonType)
        {
            _infoMessageText.text = info_message_text[(int)buttonType];
        }

        // �p�l���̕\���A�j���[�V����
        public void StartDisplayPanelAnimation()
        {
            // UI�v�f�̓����x��ݒ�
            _optionPanel.alpha = _fadeOutValue;
            _titleLogoBordor.alpha = _fadeOutValue;
            _buttonSpace.alpha = _fadeOutValue;
            _infoMessageBordor.alpha = _fadeOutValue;

            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(_optionPanel.DOFade(_fadeInValue, _fastDuration))
                .Append(_titleLogoBordor.transform.DOMoveX(_verticalMoveValue, _duration).From())
                .Join(_titleLogoBordor.DOFade(_fadeInValue, _duration))
                .Join(_buttonSpace.transform.DOMoveY(_horizontalMoveValue, _fastDuration).From().SetDelay(_delayTime))
                .Join(_buttonSpace.DOFade(_fadeInValue, _duration));

            sequence
                .Append(_infoMessageBordor.DOFade(_fadeInValue, _duration));
        }

        // �p�l�����\���ɂ���A�j���[�V����
        public Sequence StartHiddenPanelAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(_optionPanel.DOFade(_fadeOutValue, _fastDuration));

            return sequence;
        }
    }
}