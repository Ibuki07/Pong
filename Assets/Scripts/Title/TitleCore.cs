using Managers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TitleCore : MonoBehaviour
{

    [SerializeField] private Button _gameStartButton;
    [SerializeField] private CanvasGroup titleLogo;
    [SerializeField] private CanvasGroup startText;
    
    private TitleInput _titleInput;
    private TitlePresentation _titlePresentation;

    private async void Start()
    {
        SceneStateManager.Instance.OnFadeIn();

        await SoundManager.Instance.InitializedAsync;
        SoundManager.Instance.PlayBGM(SoundManager.BGMType.Title);

        _titleInput = new TitleInput(_gameStartButton);
        _titlePresentation = new TitlePresentation(titleLogo, startText);

        _titleInput.IsGameStart
            .Where(isGameStart => isGameStart)
            .Take(1)
            .Subscribe(_ =>
            {
                SceneStateManager.Instance.LoadSceneFadeOut(SceneStateManager.SceneType.Main);
            });

        Observable.EveryUpdate().Subscribe(_ =>
        {
            _titleInput.OnGameStartInput();
        }).AddTo(this);
    }
}
