using Ball;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Goal;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GoalCore[] _goal = new GoalCore[2];
        [SerializeField] private BallCore _ball;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Text _gameStartText;
        [SerializeField] private CanvasGroup _gameStartCanvasGroup;
        [SerializeField, Min(1)] private int _maxGoalCount;

        private float _duration = 1.0f;
        private int _gameStartCountTime = 3;

        private async void Start()
        {
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
            _restartButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _restartButton.gameObject.SetActive(false);
                    StartGame().Forget();
                });

            // �{�[�����j�󂳂ꂽ��Ԃ�Enter�L�[�������ꂽ��A�ĊJ�{�^�����\���ɂ��ăQ�[�����ĊJ����
            Observable.EveryUpdate()
                .Where(_ => _ball.MoveLogic.Destroyed.Value && Input.GetKeyDown(KeyCode.Return))
                .Subscribe(_ =>
                {
                    _restartButton.gameObject.SetActive(false);
                    StartGame().Forget();
                })
                .AddTo(this);

            // �{�[���ƃQ�[���J�n�e�L�X�g���\���ɂ���
            _ball.gameObject.SetActive(false);
            _restartButton.gameObject.SetActive(false);
            _gameStartText.text = "";
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
            var bgm = Random.Range((int)BGM_TYPE.Main_1, (int)BGM_TYPE.Main_3);
            SoundManager.Instance.PlayBGM((BGM_TYPE)bgm);

            // �Q�[���J�n�A�j���[�V�������Đ�����
            await GameStartAnimation();

            // �S�[���Ɋւ���X�R�A�����Z�b�g���A�{�[���𐶐�����
            foreach (var goal in _goal)
            {
                goal.ScoreLogic.ResetScoreCount();
            }
            _ball.MoveLogic.OnCreatedBall();
        }

        // �Q�[���J�n�A�j���[�V�������Đ�����
        private async UniTask GameStartAnimation()
        {
            // �Q�[���J�n�e�L�X�g��\������
            _gameStartText.gameObject.SetActive(true);
            for(int i = _gameStartCountTime; i > 0; i--)
            {
                // �J�E���g�_�E���̃A�j���[�V�������Đ�����
                _gameStartText.transform.localScale = Vector3.zero;
                _gameStartText.text = i.ToString();
                await _gameStartText.transform.DOScale(Vector3.one, _duration);
            }
            _gameStartText.transform.localScale = Vector3.zero;
            _gameStartText.text = "Game Start !";
            await _gameStartText.transform.DOScale(Vector3.one, _duration);
            
            // �Q�[���J�n�e�L�X�g���\���ɂ���
            _gameStartText.gameObject.SetActive(false);
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