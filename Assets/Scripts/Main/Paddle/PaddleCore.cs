using UniRx;
using UnityEngine;
using Wall;

namespace Paddle
{
    public class PaddleCore : MonoBehaviour, IPaddleLocalPositionAdapter, IBallCollisionHandler, IWallCollisionHandler
    {
        // �����x
        public Vector3 InitialVelocity;     

        // ILocalPositionAdapter�C���^�[�t�F�C�X�̎���
        public Vector3 LocalPosition
        {
            // ���[�J�����W���擾����
            get { return this.transform.localPosition; }
            // ���[�J�����W��ݒ肷��
            set { transform.localPosition = value; }
        }
        public PaddleLogic PaddleLogic { get; private set; }

        // --------------------------------------------------

        [SerializeField] private PaddleMoveInput _paddleMoveInput;
        private PaddleSimulation _paddleSimulation;
        // FixedUpdate�̍w�ǉ����p
        private System.IDisposable _fixedUpdateDisposable;

        // --------------------------------------------------

        public void OnCollisionBall(Ball.BallCore ball)
        {
            var offsetVelocity = PaddleLogic.OnRandomizeVerticalVelocity(InitialVelocity);
            ball.BallLogic.OnFlipHorizontalVelocity(offsetVelocity);
        }

        public void OnCollisionWall(WallCore wall)
        {
            PaddleLogic.OnStopPaddle();
        }

        // --------------------------------------------------

        private void Start()
        {
            // ������
            _paddleSimulation = new PaddleSimulation(InitialVelocity);
            PaddleLogic = new PaddleLogic(this, _paddleMoveInput, _paddleSimulation);

            // FixedUpdate���G�~�����[�g����Observable���쐬����
            var fixedUpdateObservable = Observable.EveryFixedUpdate().Publish().RefCount();
            // Observable���T�u�X�N���C�u����
            _fixedUpdateDisposable = fixedUpdateObservable
                .Subscribe(_ =>
                {
                    PaddleLogic.OnUpdateLocalPosition(Time.fixedDeltaTime);
                });
        }

        private void OnDestroy()
        {
            // IDisposable��j������
            _fixedUpdateDisposable?.Dispose();
        }


    }
}