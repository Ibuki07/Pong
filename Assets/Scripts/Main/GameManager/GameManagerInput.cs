using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class GameManagerInput
    {
        public IReadOnlyReactiveProperty<bool> Restart => _restart;
        private readonly ReactiveProperty<bool> _restart = new ReactiveProperty<bool>(false);
        private Button _restartButton;

        public GameManagerInput(
            Button restartButton
            )
        {
            _restartButton = restartButton;

            _restartButton.OnClickAsObservable()
                .Where(_ => !_restart.Value)
                .Subscribe(_ =>
                {
                    _restart.Value = true;
                });
        }

        public bool IsRestart()
        {
            _restart.Value = false;
            if (_restartButton.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return))
            {
                _restart.Value = true;
            }

            return _restart.Value;
        }

        public void ResetRestart()
        {
            _restart.Value = false;
        }
    }
}