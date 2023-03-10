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

            // オプション画面を開く
            _optionPanelInput.PanelOpen
            .Where(isPanelOpen => isPanelOpen)
            .Subscribe(_ =>
            {
                _optionPanel.gameObject.SetActive(true);
                _optionPanelPresentation.OnDisplayPanelAnimation();
            });
            // オプション画面を閉じる
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
            
            // グラフィック設定画面を開く
            _optionPanelInput.GraphicSetting
            .Where(isGraphicSettingOpen => isGraphicSettingOpen)
            .Subscribe(_ =>
            {
                _graphicSettingPanel.gameObject.SetActive(true);
                _graphicSettingPresentation.OnDisplayPanelAnimation();
            });
            // グラフィック設定画面を閉じる
            _optionPanelInput.GraphicSetting
            .Where(isGraphicSettingOpen => !isGraphicSettingOpen)
            .Subscribe(_ =>
            {
                _graphicSettingPanel.gameObject.SetActive(false);
            });

            // オーディオ設定画面を開く
            _optionPanelInput.AudioSetting
            .Where(isAudioSettingOpen => isAudioSettingOpen)
            .Subscribe(_ =>
            {
                _audioSettingPanel.gameObject.SetActive(true);
                _bgmAudioSettingLogic.UpdateSliderValue();
                _seAudioSettingLogic.UpdateSliderValue();
                _audioSettingPresentation.OnDisplayPanelAnimation();
            });
            // オーディオ設定画面を閉じる
            _optionPanelInput.AudioSetting
            .Where(isAudioSettingOpen => !isAudioSettingOpen)
            .Subscribe(_ =>
            {
                _audioSettingPanel.gameObject.SetActive(false);
            });
        }


    }
}