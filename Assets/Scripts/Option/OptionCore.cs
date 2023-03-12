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

            // ゲーム終了タイトル処理を設定する
            _quitTitleInput = new QuitTitleInput(_optionButtons[(int)OPTION_TYPE.QuitTitle]);
            _quitTitleLogic = new QuitTitleLogic(_quitTitleInput);

            // ゲーム終了処理を設定する
            _quitGameInput = new QuitGameInput(_optionButtons[(int)OPTION_TYPE.QuitGame], _decisionQuitGameButton, _cancelQuitGameButton);
            _quitGameLogic = new QuitGameLogic(_quitGameInput);
            _quitGamePresentation = new QuitGamePresentation(_quitGamePanel);

            // グラフィック設定画面
            _graphicSettingInput = new GraphicSettingInput(_fullScreenButton);
            _graphicSettingLogic = new GraphicSettingLogic(_graphicSettingInput);
            _graphicSettingPresentation = new GraphicSettingPresentation(_graphicSettingInput, _graphicSettingPanel, _fullScreenButton);

            // オーディオ設定画面
            _bgmAudioSettingInput = new AudioSettingInput(_bgmVolumeSlider);
            _bgmAudioSettingLogic = new AudioSettingLogic(_bgmAudioSettingInput, SOUND_TYPE.BGM);
            _seAudioSettingInput = new AudioSettingInput(_seVolumeSlider);
            _seAudioSettingLogic = new AudioSettingLogic(_seAudioSettingInput, SOUND_TYPE.SE);
            _audioSettingPresentation = new AudioSettingPresentation(_bgmVolumeSlider, _seVolumeSlider, _audioSettingPanel);

            // オプション画面
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

            // オプション画面の開閉を監視する
            _optionPanelInput.PanelOpen
                // オプション画面が開いたとき
                .Where(isPanelOpen => isPanelOpen)
                .Subscribe(_ =>
                {
                    // オプション画面を表示する
                    _optionPanel.gameObject.SetActive(true);
                    // オプション画面をアニメーション表示する
                    _optionPanelPresentation.StartDisplayPanelAnimation();
                });

            _optionPanelInput.PanelOpen
                // オプション画面が閉じたとき
                .Where(isPanelOpen => !isPanelOpen)
                // 最初の値を無視する（ゲーム起動時にはオプション画面が閉じているため）
                .Skip(1)
                .Subscribe(_ =>
                {
                    // オプション画面をアニメーション非表示にする
                    _optionPanelPresentation.StartHiddenPanelAnimation()
                    .OnComplete(() =>
                    {
                        // オプション画面を非表示にする
                        _graphicSettingPanel.gameObject.SetActive(false);
                        _optionPanel.gameObject.SetActive(false);
                    });
                });

            // グラフィック設定画面の開閉を監視する
            _optionPanelInput.GraphicSetting
                // グラフィック設定画面が開いたとき
                .Where(isGraphicSettingOpen => isGraphicSettingOpen)
                .Subscribe(_ =>
                {
                    // グラフィック設定画面を表示する
                    _graphicSettingPanel.gameObject.SetActive(true);
                    // グラフィック設定画面をアニメーション表示する
                    _graphicSettingPresentation.StartDisplayPanelAnimation();
                });

            // グラフィック設定画面を閉じる
            _optionPanelInput.GraphicSetting
                // グラフィック設定画面が閉じたとき
                .Where(isGraphicSettingOpen => !isGraphicSettingOpen)
                .Subscribe(_ =>
                {
                    // グラフィック設定画面を非表示にする
                    _graphicSettingPanel.gameObject.SetActive(false);
                });

            // オーディオ設定画面の開閉を監視する
            _optionPanelInput.AudioSetting
                // オーディオ設定画面が開いたとき
                .Where(isAudioSettingOpen => isAudioSettingOpen)
                .Subscribe(_ =>
                {
                    // オーディオ設定画面を表示する
                    _audioSettingPanel.gameObject.SetActive(true);
                    // オーディオ設定スライダーの値を更新する
                    _audioSettingPresentation.UpdateSliderValue();
                    // オーディオ設定画面をアニメーション表示する
                    _audioSettingPresentation.StartDisplayPanelAnimation();
                });

            // オーディオ設定画面を閉じる
            _optionPanelInput.AudioSetting
                // オーディオ設定画面が閉じたとき
                .Where(isAudioSettingOpen => !isAudioSettingOpen)
                .Subscribe(_ =>
                {
                    // オーディオ設定画面を非表示にする
                    _audioSettingPanel.gameObject.SetActive(false);
                });

            // ゲーム終了確認画面の開閉を監視する
            _optionPanelInput.QuitGame
                // ゲーム終了確認画面が開いたとき
                .Where(isQuitGameOpen => isQuitGameOpen)
                .Subscribe(_ =>
                {
                    // ゲーム終了確認画面を表示する
                    _quitGamePanel.gameObject.SetActive(true);
                    _quitGamePresentation.StartDisplayPanelAnimation();
                });
            _optionPanelInput.QuitGame
                // ゲーム終了確認画面が閉じたとき
                .Where(isQuitGameOpen => !isQuitGameOpen)
                .Subscribe(_ =>
                {
                    // ゲーム終了確認画面を非表示にする
                    _quitGamePanel.gameObject.SetActive(false);
                });
            // ゲーム終了確認画面からキャンセルされたとき
            _quitGameInput.CancelQuitGame
                .Where(isCancelQuitGame => isCancelQuitGame)
                .Subscribe(_ =>
                {
                    // ゲーム終了確認画面を非表示にし、ゲーム終了の状態をリセットする
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