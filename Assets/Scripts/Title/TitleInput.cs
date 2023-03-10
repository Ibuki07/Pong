using Managers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TitleInput
{
    public IReadOnlyReactiveProperty<bool> IsGameStart => _isGameStart;
    private readonly ReactiveProperty<bool> _isGameStart = new ReactiveProperty<bool>(false);

    private Button _gameStartButton;

    public TitleInput(Button gameStartButton)
    {
        gameStartButton.OnClickAsObservable().Subscribe(_ =>
        {
            SoundManager.Instance.PlaySE(SoundManager.SEType.Start);
            _isGameStart.Value = true;
        });
    }

    public void OnGameStartInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        { 
            SoundManager.Instance.PlaySE(SoundManager.SEType.Start);
            _isGameStart.Value = true;
        }
    }



}
