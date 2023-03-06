using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Managers;

namespace Option
{
    public class OptionCore : MonoBehaviour
    {


        [SerializeField] private Button _panelOpenButton;
        [SerializeField] private Button _gameSettingButton;
        [SerializeField] private Button _audioSettingButton;
        [SerializeField] private Button _quitGameButton;
        [SerializeField] private Button _panelCloseButton;
        [SerializeField] private Slider _bgmVolumeSlider;
        [SerializeField] private Slider _seVolumeSlider;
        [SerializeField] private Text _infoMessageText;
        [SerializeField] private CanvasGroup _optionPanel;
        [SerializeField] private CanvasGroup _titleLogoBordor;
        [SerializeField] private CanvasGroup _buttonSpace;
        [SerializeField] private CanvasGroup _infoMessageBordor;
        [SerializeField] private CanvasGroup _audioSettingPanel;

        private OptionQuitGameLogic _quitGameLogic;
        private OptionAudioSettingLogic _bgmAudioSettingLogic;
        private OptionAudioSettingLogic _seAudioSettingLogic;
        private OptionAudioSettingPresentation _audioSettingPresentation;
        private OptionPanelInput _panelInput;
        private OptionPanelPresentation _panelPresentation;


        private void Start()
        {
            _optionPanel.gameObject.SetActive(false);
            _audioSettingPanel.gameObject.SetActive(false);
            _infoMessageText.text = "";
            
            _quitGameLogic = new OptionQuitGameLogic(_quitGameButton);
            _bgmAudioSettingLogic = new OptionAudioSettingLogic(_bgmVolumeSlider, SoundManager.SoundType.BGM);
            _seAudioSettingLogic = new OptionAudioSettingLogic(_seVolumeSlider, SoundManager.SoundType.SE);
            _audioSettingPresentation = new OptionAudioSettingPresentation(_audioSettingPanel);
            _panelInput = new OptionPanelInput(_panelOpenButton, _gameSettingButton, _audioSettingButton, _quitGameButton, _panelCloseButton);
            _panelPresentation = new OptionPanelPresentation(_panelInput, _optionPanel, _titleLogoBordor, _buttonSpace, _infoMessageBordor, _infoMessageText);

            _panelInput.PanelOpen
                .Where(isPanelOpen => isPanelOpen)
                .Subscribe(_ =>
                {
                    _optionPanel.gameObject.SetActive(true);
                    _panelPresentation.OnDisplayPanelAnimation();
                });
            _panelInput.PanelOpen
                .Where(isPanelOpen => !isPanelOpen)
                .Subscribe(_ =>
                {
                    _panelPresentation.OnHiddenPanelAnimation()
                    .OnComplete(() =>
                    {
                        _audioSettingPanel.gameObject.SetActive(false);
                        _optionPanel.gameObject.SetActive(false);
                    });
                });

            _panelInput.AudioSettingOpen
                .Where(isAudioSettingOpen => isAudioSettingOpen)
                .Subscribe(_ =>
                {
                    _audioSettingPanel.gameObject.SetActive(true);
                    _bgmAudioSettingLogic.UpdateSliderValue();
                    _seAudioSettingLogic.UpdateSliderValue();
                    _audioSettingPresentation.OnDisplayPanelAnimation();
                });
        }


    }
}