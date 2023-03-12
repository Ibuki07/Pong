using Managers;
using UnityEngine;
using Wall;
using UnityEngine.UI;
using Ball;

namespace Paddle
{
    public class PaddleCore : MonoBehaviour, IPaddleLocalPositionAdapter, IBallCollisionHandler, IWallCollisionHandler
    {
        // �����x
        public Vector3 InitialVelocity => _initialVelocity;
        public Vector3 LocalPosition
        {
            // ���[�J�����W���擾����
            get { return this.transform.localPosition; }
            // ���[�J�����W��ݒ肷��
            set { transform.localPosition = value; }
        }
        public PaddleMoveLogic MoveLogic { get; private set; }
        public PaddleAutoMoveLogic AutoMoveLogic { get; private set; }

        [SerializeField] private Button _autoMoveButton;
        [SerializeField] private BallCore _ball;
        [SerializeField] private Vector3 _initialVelocity;

        private PaddleMoveInput _moveInput;
        private PaddleMoveSimulation _moveSimulation;
        private PaddleAutoMoveInput _autoMoveInput;
        private PaddleAutoMovePresentation _autoMovePresentation;

        // �{�[���ƐڐG�����ۂ̏���
        public void OnCollisionBall(BallCore ball)
        {
            // �{�[���ɉ����鑬�x�̃I�t�Z�b�g���v�Z����
            var offsetVerticalVelocity = MoveLogic.RandomizeVerticalVelocity(InitialVelocity);
            ball.MoveLogic.AddVelocity(offsetVerticalVelocity * 0.5f);
            // �{�[���̐������x�𔽓]������
            ball.MoveLogic.FlipHorizontalVelocity();

            // SE���Đ�����
            SoundManager.Instance.PlaySE(SE_TYPE.HitPaddle);
        }

        // �ǂƐڐG�����ۂ̏���
        public void OnCollisionWall(WallCore wall)
        {
            // �p�h�����~���A�����ړ�����~����
            MoveLogic.StopPaddle();
            AutoMoveLogic.OnStopPaddle();
        }

        private void Start()
        {
            // �ړ��V�~�����[�V�����I�u�W�F�N�g������������
            _moveSimulation = new PaddleMoveSimulation(InitialVelocity);

            // ���́E�V�~�����[�V�����E�A�_�v�^���g���Ĉړ����W�b�N������������
            _moveInput = new PaddleMoveInput(this);
            MoveLogic = new PaddleMoveLogic(_moveInput, _moveSimulation, this);

            // �����ړ����W�b�N������������
            _autoMoveInput = new PaddleAutoMoveInput(this, _autoMoveButton);
            AutoMoveLogic = new PaddleAutoMoveLogic(_autoMoveInput, _moveSimulation, this, _ball);

            // �����ړ��{�^����CanvasGroup�R���|�[�l���g���擾���āA�v���[���e�[�V�����p�̃I�u�W�F�N�g������������
            var autoModeButtonCanvasGroup = _autoMoveButton.GetComponent<CanvasGroup>();
            _autoMovePresentation = new PaddleAutoMovePresentation(_autoMoveInput, autoModeButtonCanvasGroup);
        }

        private void OnDestroy()
        {
            MoveLogic?.Dispose();
            AutoMoveLogic?.Dispose();
        }
    }


}