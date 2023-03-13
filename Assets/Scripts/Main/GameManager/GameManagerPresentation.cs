using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Managers
{
    public class GameManagerPresentation
    {

        private Text _gameStartText;
        private int _gameStartCountTime = 3;
        private float _duration = 1.0f;

        public GameManagerPresentation(Text gameStartText)
        {
            _gameStartText = gameStartText;
            _gameStartText.text = "";
        }

        // ゲーム開始アニメーションを再生する
        public async UniTask GameStartAnimation()
        {
            for (int i = _gameStartCountTime; i > 0; i--)
            {
                // カウントダウンのアニメーションを再生する
                _gameStartText.transform.localScale = Vector3.zero;
                _gameStartText.text = i.ToString();
                await _gameStartText.transform.DOScale(Vector3.one, _duration);
            }
            _gameStartText.transform.localScale = Vector3.zero;
            _gameStartText.text = "Game Start !";
            await _gameStartText.transform.DOScale(Vector3.one, _duration);
        }
    }
}