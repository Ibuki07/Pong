using System.Collections;
using System.Collections.Generic;
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
            _isGameStart.Value = true;
        });
    }

    public void OnGameStartInput()
    {
        _isGameStart.Value = Input.GetKeyDown(KeyCode.Return);
    }



}
