using Ball;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

namespace Goal
{
    public class GoalCore : MonoBehaviour, IBallCollisionHandler
    {
        public GoalScoreLogic ScoreLogic { get; private set; }
        // UniTask�����̃L�����Z���g�[�N��
        public CancellationToken CancellationToken { get; private set; }
        // �����������������������ǂ�����Ԃ�UniTask
        public UniTask InitializedAsync => _uniTaskCompletionSource.Task;

        [SerializeField] private Text _scoreCountText;

        private readonly UniTaskCompletionSource _uniTaskCompletionSource = new UniTaskCompletionSource();

        private GoalScorePresentation _scorePresentation;

        private void Start()
        {
            ScoreLogic = new GoalScoreLogic();
            // ���_��\������UI�v�f�Ɠ��_�J�E���g���Ǘ����郍�W�b�N��n���āA���_�\�����Ǘ�����v���[���e�[�V�������쐬
            _scorePresentation = new GoalScorePresentation(_scoreCountText, ScoreLogic);
            // �L�����Z���g�[�N����ݒ�
            CancellationToken = this.GetCancellationTokenOnDestroy();

            // �����������������������Ƃ�UniTaskCompletionSource�ɒʒm
            _uniTaskCompletionSource.TrySetResult();
        }

        private void OnDestroy()
        {
            _uniTaskCompletionSource.TrySetCanceled();
        }

        // �{�[�����S�[���ɏՓ˂������̏���
        public void OnCollisionBall(BallCore ball)
        {
            // �{�[���̈ړ������Z�b�g
            ball.MoveLogic.ResetPosition(CancellationToken).Forget();
            // ���_�����Z
            ScoreLogic.CountUpScore();
        }
    }
}