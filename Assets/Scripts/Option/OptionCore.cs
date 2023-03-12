using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Option
{
    public class OptionCore : MonoBehaviour
    {
        [SerializeField] private List<Button> _optionButtons = new List<Button>();
        [SerializeField] private Button _fullScreenButton;
        [SerializeField] private Button _decisionQuitGameButton;
        [SerializeField] private Button _cancelQuitGameButton;
        [SerializeField] private Slider _bgmVolumeSlider;
        [SerializeField] private Slider _seVolumeSlider;
        [SerializeField] private Text _infoMessageText;
        [SerializeField] private CanvasGroup _optionPanel;
        [SerializeField] private CanvasGroup _titleLogoBordor;
        [SerializeField] private CanvasGroup _buttonSpace;
        [SerializeField] private CanvasGroup _infoMessageBordor;
        [SerializeField] private CanvasGroup _graphicSettingPanel;
        [SerializeField] private CanvasGroup _audioSettingPanel;
        [SerializeField] private CanvasGroup _quitGamePanel;

        private QuitTitleInput _quitTitleInput;
        private QuitTitleLogic _quitTitleLogic;
        private QuitGameInput _quitGameInput;
        private QuitGameLogic _quitGameLogic;
        private QuitGamePresentation _quitGamePresentation;
        private GraphicSettingInput _graphicSettingInput;
        private GraphicSettingLogic _graphicSettingLogic;
        private GraphicSettingPresentation _graphicSettingPresentation;
        private AudioSettingInput _bgmAudioSettingInput;
        private AudioSettingLogic _bgmAudioSettingLogic;
        private AudioSettingInput _seAudioSettingInput;
        private AudioSettingLogic _seAudioSettingLogic;
        private AudioSettingPresentation _audioSettingPresentation;
        private OptionPanelInput _optionPanelInput;
        private OptionPanelPresentation _optionPanelPresentation;

        private void Start()
        {
            _optionPanel.gameObject.SetActive(false);
            _graphicSettingPanel.gameObject.SetActive(false);
            _audioSettingPanel.gameObject.SetActive(false);
            _quitGamePanel.gameObject.SetActive(false);

            // �Q�[���I���^�C�g��������ݒ肷��
            _quitTitleInput = new QuitTitleInput(_optionButtons[(int)OPTION_TYPE.QuitTitle]);
            _quitTitleLogic = new QuitTitleLogic(_quitTitleInput);

            // �Q�[���I��������ݒ肷��
            _quitGameInput = new QuitGameInput(_optionButtons[(int)OPTION_TYPE.QuitGame], _decisionQuitGameButton, _cancelQuitGameButton);
            _quitGameLogic = new QuitGameLogic(_quitGameInput);
            _quitGamePresentation = new QuitGamePresentation(_quitGamePanel);

            // �O���t�B�b�N�ݒ���
            _graphicSettingInput = new GraphicSettingInput(_fullScreenButton);
            _graphicSettingLogic = new GraphicSettingLogic(_graphicSettingInput);
            _graphicSettingPresentation = new GraphicSettingPresentation(_graphicSettingInput, _graphicSettingPanel, _fullScreenButton);

            // �I�[�f�B�I�ݒ���
            _bgmAudioSettingInput = new AudioSettingInput(_bgmVolumeSlider);
            _bgmAudioSettingLogic = new AudioSettingLogic(_bgmAudioSettingInput, SOUND_TYPE.BGM);
            _seAudioSettingInput = new AudioSettingInput(_seVolumeSlider);
            _seAudioSettingLogic = new AudioSettingLogic(_seAudioSettingInput, SOUND_TYPE.SE);
            _audioSettingPresentation = new AudioSettingPresentation(_bgmVolumeSlider, _seVolumeSlider, _audioSettingPanel);

            // �I�v�V�������
            _optionPanelInput = new OptionPanelInput(_optionButtons);
            _optionPanelPresentation = new OptionPanelPresentation(_optionPanelInput, _optionPanel, _titleLogoBordor, _buttonSpace, _infoMessageBordor, _infoMessageText);

            if (SceneManager.GetActiveScene().name == "Title")
            {
                _optionButtons[(int)OPTION_TYPE.QuitTitle].gameObject.SetActive(false);
            }
            else
            {
                _optionButtons[(int)OPTION_TYPE.QuitTitle].gameObject.SetActive(true);
            }

            // �I�v�V������ʂ̊J���Ď�����
            _optionPanelInput.PanelOpen
                // �I�v�V������ʂ��J�����Ƃ�
                .Where(isPanelOpen => isPanelOpen)
                .Subscribe(_ =>
                {
                    // �I�v�V������ʂ�\������
                    _optionPanel.gameObject.SetActive(true);
                    // �I�v�V������ʂ��A�j���[�V�����\������
                    _optionPanelPresentation.StartDisplayPanelAnimation();
                });

            _optionPanelInput.PanelOpen
                // �I�v�V������ʂ������Ƃ�
                .Where(isPanelOpen => !isPanelOpen)
                // �ŏ��̒l�𖳎�����i�Q�[���N�����ɂ̓I�v�V������ʂ����Ă��邽�߁j
                .Skip(1)
                .Subscribe(_ =>
                {
                    // �I�v�V������ʂ��A�j���[�V������\���ɂ���
                    _optionPanelPresentation.StartHiddenPanelAnimation()
                    .OnComplete(() =>
                    {
                        // �I�v�V������ʂ��\���ɂ���
                        _graphicSettingPanel.gameObject.SetActive(false);
                        _optionPanel.gameObject.SetActive(false);
                    });
                });

            // �O���t�B�b�N�ݒ��ʂ̊J���Ď�����
            _optionPanelInput.GraphicSetting
                // �O���t�B�b�N�ݒ��ʂ��J�����Ƃ�
                .Where(isGraphicSettingOpen => isGraphicSettingOpen)
                .Subscribe(_ =>
                {
                    // �O���t�B�b�N�ݒ��ʂ�\������
                    _graphicSettingPanel.gameObject.SetActive(true);
                    // �O���t�B�b�N�ݒ��ʂ��A�j���[�V�����\������
                    _graphicSettingPresentation.StartDisplayPanelAnimation();
                });

            // �O���t�B�b�N�ݒ��ʂ����
            _optionPanelInput.GraphicSetting
                // �O���t�B�b�N�ݒ��ʂ������Ƃ�
                .Where(isGraphicSettingOpen => !isGraphicSettingOpen)
                .Subscribe(_ =>
                {
                    // �O���t�B�b�N�ݒ��ʂ��\���ɂ���
                    _graphicSettingPanel.gameObject.SetActive(false);
                });

            // �I�[�f�B�I�ݒ��ʂ̊J���Ď�����
            _optionPanelInput.AudioSetting
                // �I�[�f�B�I�ݒ��ʂ��J�����Ƃ�
                .Where(isAudioSettingOpen => isAudioSettingOpen)
                .Subscribe(_ =>
                {
                    // �I�[�f�B�I�ݒ��ʂ�\������
                    _audioSettingPanel.gameObject.SetActive(true);
                    // �I�[�f�B�I�ݒ�X���C�_�[�̒l���X�V����
                    _audioSettingPresentation.UpdateSliderValue();
                    // �I�[�f�B�I�ݒ��ʂ��A�j���[�V�����\������
                    _audioSettingPresentation.StartDisplayPanelAnimation();
                });

            // �I�[�f�B�I�ݒ��ʂ����
            _optionPanelInput.AudioSetting
                // �I�[�f�B�I�ݒ��ʂ������Ƃ�
                .Where(isAudioSettingOpen => !isAudioSettingOpen)
                .Subscribe(_ =>
                {
                    // �I�[�f�B�I�ݒ��ʂ��\���ɂ���
                    _audioSettingPanel.gameObject.SetActive(false);
                });

            // �Q�[���I���m�F��ʂ̊J���Ď�����
            _optionPanelInput.QuitGame
                // �Q�[���I���m�F��ʂ��J�����Ƃ�
                .Where(isQuitGameOpen => isQuitGameOpen)
                .Subscribe(_ =>
                {
                    // �Q�[���I���m�F��ʂ�\������
                    _quitGamePanel.gameObject.SetActive(true);
                    _quitGamePresentation.StartDisplayPanelAnimation();
                });
            _optionPanelInput.QuitGame
                // �Q�[���I���m�F��ʂ������Ƃ�
                .Where(isQuitGameOpen => !isQuitGameOpen)
                .Subscribe(_ =>
                {
                    // �Q�[���I���m�F��ʂ��\���ɂ���
                    _quitGamePanel.gameObject.SetActive(false);
                });
            // �Q�[���I���m�F��ʂ���L�����Z�����ꂽ�Ƃ�
            _quitGameInput.CancelQuitGame
                .Where(isCancelQuitGame => isCancelQuitGame)
                .Subscribe(_ =>
                {
                    // �Q�[���I���m�F��ʂ��\���ɂ��A�Q�[���I���̏�Ԃ����Z�b�g����
                    _quitGamePanel.gameObject.SetActive(false);
                    _optionPanelInput.ResetQuitGame();
                });
        }

        private void OnDestroy()
        {
            _optionPanelInput?.Dispose();
        }
    }
}