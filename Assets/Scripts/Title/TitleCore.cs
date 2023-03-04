using Paddle;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleCore : MonoBehaviour
{

    private TitleInput _titleInput;
    [SerializeField] private Button _gameStartButton;

    void Start()
    {
        _titleInput = new TitleInput(_gameStartButton);

        _titleInput.IsGameStart
            .Where(isGameStart => isGameStart)
            .Take(1)
            .Subscribe(_ =>
            {
                SceneManager.LoadScene("ClassDesignTest");
            });

        this.UpdateAsObservable().Subscribe(_ =>
        {
            _titleInput.OnGameStartInput();
        });
    }
}
