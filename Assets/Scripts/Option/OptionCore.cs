using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Managers;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Option
{
    public class OptionCore : MonoBehaviour
    {
        public enum ButtonType
        {
            PanelOpen = 0,
            GameSetting,
            GraphicSetting,
            AudioSetting,
            QuitTitle,
            QuitGame,
            PanelClose,
        }

        [SerializeField] private List<Button> _buttons = new List<Button>();
        [SerializeField] private Button _fullScreenButton;
        [SerializeField] private Slider _bgmVolumeSlider;
        [SerializeField] private Slider _seVolumeSlider;
        [SerializeField] private Text _infoMessageText;
        [SerializeField] private CanvasGroup _optionPanel;
        [SerializeField] private CanvasGroup _titleLogoBordor;
        [SerializeField] private CanvasGroup _buttonSpace;
        [SerializeField] private CanvasGroup _infoMessageBordor;
        [SerializeField] private CanvasGroup _graphicSettingPanel;
        [SerializeField] private CanvasGroup _audioSettingPanel;

        private QuitTitleLogic _quitTitleLogic;
        private QuitGameLogic _quitGameLogic;
        private GraphicSettingLogic _graphicSettingLogic;
        private GraphicSettingPresentation _graphicSettingPresentation;
        private AudioSettingLogic _bgmAudioSettingLogic;
        private AudioSettingLogic _seAudioSettingLogic;
        private AudioSettingPresentation _audioSettingPresentation;
        private OptionPanelInput _optionPanelInput;
        private OptionPanelPresentation _optionPanelPresentation;


        private void Start()
        {
            _optionPanel.gameObject.SetActive(false);
            _audioSettingPanel.gameObject.SetActive(false);
            _graphicSettingPanel.gameObject.SetActive(false);
            _infoMessageText.text = "";
            
            _quitTitleLogic = new QuitTitleLogic(_buttons[(int)ButtonType.QuitTitle]);
            _quitGameLogic = new QuitGameLogic(_buttons[(int)ButtonType.QuitGame]);

            _graphicSettingLogic = new GraphicSettingLogic(_fullScreenButton);
            _graphicSettingPresentation = new GraphicSettingPresentation(_graphicSettingPanel);

            _bgmAudioSettingLogic = new AudioSettingLogic(_bgmVolumeSlider, SoundManager.SoundType.BGM);
            _seAudioSettingLogic = new AudioSettingLogic(_seVolumeSlider, SoundManager.SoundType.SE);
            _audioSettingPresentation = new AudioSettingPresentation(_audioSettingPanel);


            _optionPanelInput = new OptionPanelInput(_buttons);
            _optionPanelPresentation = new OptionPanelPresentation(_optionPanelInput, _optionPanel, _titleLogoBordor, _buttonSpace, _infoMessageBordor, _infoMessageText);

            if (SceneManager.GetActiveScene().name == "Title")
            {
                _buttons[(int)ButtonType.QuitTitle].gameObject.SetActive(false);
            }
            else
            {
                _buttons[(int)ButtonType.QuitTitle].gameObject.SetActive(true);
            }

            // �I�v�V������ʂ��J��
            _optionPanelInput.PanelOpen
            .Where(isPanelOpen => isPanelOpen)
            .Subscribe(_ =>
            {
                _optionPanel.gameObject.SetActive(true);
                _optionPanelPresentation.OnDisplayPanelAnimation();
            });
            // �I�v�V������ʂ����
            _optionPanelInput.PanelOpen
            .Where(isPanelOpen => !isPanelOpen)
            .Skip(1)
            .Subscribe(_ =>
            {
                _optionPanelPresentation.OnHiddenPanelAnimation()
                .OnComplete(() =>
                {
                    _infoMessageText.text = "";
                    _graphicSettingPanel.gameObject.SetActive(false);
                    _optionPanel.gameObject.SetActive(false);
                });
            });
            
            // �O���t�B�b�N�ݒ��ʂ��J��
            _optionPanelInput.GraphicSetting
            .Where(isGraphicSettingOpen => isGraphicSettingOpen)
            .Subscribe(_ =>
            {
                _graphicSettingPanel.gameObject.SetActive(true);
                _graphicSettingPresentation.OnDisplayPanelAnimation();
            });
            // �O���t�B�b�N�ݒ��ʂ����
            _optionPanelInput.GraphicSetting
            .Where(isGraphicSettingOpen => !isGraphicSettingOpen)
            .Subscribe(_ =>
            {
                _graphicSettingPanel.gameObject.SetActive(false);
            });

            // �I�[�f�B�I�ݒ��ʂ��J��
            _optionPanelInput.AudioSetting
            .Where(isAudioSettingOpen => isAudioSettingOpen)
            .Subscribe(_ =>
            {
                _audioSettingPanel.gameObject.SetActive(true);
                _bgmAudioSettingLogic.UpdateSliderValue();
                _seAudioSettingLogic.UpdateSliderValue();
                _audioSettingPresentation.OnDisplayPanelAnimation();
            });
            // �I�[�f�B�I�ݒ��ʂ����
            _optionPanelInput.AudioSetting
            .Where(isAudioSettingOpen => !isAudioSettingOpen)
            .Subscribe(_ =>
            {
                _audioSettingPanel.gameObject.SetActive(false);
            });
        }


    }
}