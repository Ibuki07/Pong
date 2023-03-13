using Ball;
using Cysharp.Threading.Tasks;
using Goal;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class GameManagerCore : MonoBehaviour
    {
        [SerializeField] private GoalCore[] _goal = new GoalCore[2];
        [SerializeField] private BallCore _ball;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Text _gameStartText;
        [SerializeField] private CanvasGroup _gameStartCanvasGroup;
        [SerializeField, Min(1)] private int _maxGoalCount;

        private GameManagerInput _gameManagerInput;
        private GameManagerLogic _gameManagerLogic;
        private GameManagerPresentation _gameManagerPresentation;

        private async void Start()
        {
            _gameManagerInput = new GameManagerInput(_restartButton);
            _gameManagerLogic = new GameManagerLogic();
            _gameManagerPresentation = new GameManagerPresentation(_gameStartText);


            // SceneStateManager���g�p���ăt�F�[�h�C�����J�n����
            SceneStateManager.Instance.StartFadeIn();

            // GoalCore�̏�������҂�
            foreach (var goal in _goal)
            {
                await goal.InitializedAsync;
                EndGame(goal);
            }

            // �{�[�����j�󂳂ꂽ��ĊJ�{�^����\������
            _ball.MoveLogic.Destroyed
                .Where(isDestroyed => isDestroyed)
                .Subscribe(_ =>
                {
                    _restartButton.gameObject.SetActive(true);
                });

            // �ĊJ�{�^�����N���b�N���ꂽ��A�ĊJ�{�^�����\���ɂ��ăQ�[�����ĊJ����
            _gameManagerInput.Restart
                .Where(isRestart => isRestart)
                .Subscribe(_ =>
                {
                    _restartButton.gameObject.SetActive(false);
                    StartGame().Forget();
                });

            // �{�[�����j�󂳂ꂽ��Ԃ�Enter�L�[�������ꂽ��A�ĊJ�{�^�����\���ɂ��ăQ�[�����ĊJ����
            Observable.EveryUpdate()
                .Where(_ => _ball.MoveLogic.Destroyed.Value)
                .Subscribe(_ =>
                {
                    _gameManagerInput.IsRestart();
                })
                .AddTo(this);

            // �{�[���ƃQ�[���J�n�e�L�X�g���\���ɂ���
            _ball.gameObject.SetActive(false);
            _restartButton.gameObject.SetActive(false);
            _gameStartText.gameObject.SetActive(false);

            // �t�F�[�h�C������������܂őҋ@����
            await UniTask.Delay(System.TimeSpan.FromSeconds(SceneStateManager.Instance.FadeDuration));

            // �Q�[�����J�n����
            StartGame().Forget();

        }

        // �Q�[�����J�n����
        private async UniTask StartGame()
        {
            // BGM�������_���ɍĐ�����
            SoundManager.Instance.PlayBGM(_gameManagerLogic.RandomizeBGM());

            // �Q�[���J�n�e�L�X�g��\������
            _gameStartText.gameObject.SetActive(true);
            
            // �Q�[���J�n�A�j���[�V�������Đ�����
            await _gameManagerPresentation.GameStartAnimation();
            
            // �Q�[���J�n�e�L�X�g���\���ɂ���
            _gameStartText.gameObject.SetActive(false);

            // �S�[���Ɋւ���X�R�A�����Z�b�g���A�{�[���𐶐�����
            foreach (var goal in _goal)
            {
                goal.ScoreLogic.ResetScoreCount();
            }
            _ball.MoveLogic.OnCreatedBall();
        }

        private void EndGame(GoalCore goal)
        {
            // �S�[����_maxGoalCount��ȏ�B�����ꂽ�ꍇ�A�{�[����j�󂷂�
            goal.ScoreLogic.ScoreCount
                .Where(goalCount => goalCount >= _maxGoalCount)
                .Subscribe(_ =>
                {
                    _ball.MoveLogic.OnDestroyedBall();
                });
        }
    }
}