using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;
using Wall;
using Managers;
using UniRx.Triggers;

namespace Ball
{
    public class BallCore : MonoBehaviour, IBallLocalPositionAdapter, IWallCollisionHandler
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
        public BallMoveLogic MoveLogic { get; private set; }

        // �Q�[���I�u�W�F�N�g���j�������Ƃ��ɃL�����Z�������g�[�N��
        public CancellationToken CancellationToken { get; private set; }

        // �����x
        [SerializeField] private Vector3 _initialVelocity;

        private BallMoveSimulation _moveSimulation;

        private void Start()
        {
            // ������
            _moveSimulation = new BallMoveSimulation(InitialVelocity);
            MoveLogic = new BallMoveLogic(_moveSimulation, this);
            CancellationToken = this.GetCancellationTokenOnDestroy();

            // �{�[�����j�󂳂ꂽ�Ƃ��ɏ�������
            MoveLogic.Destroyed
                .Where(isDestroyed => isDestroyed)
                .Subscribe(_ =>
                {
                    // �Q�[���I�u�W�F�N�g���A�N�e�B�u�ɂ���
                    this.gameObject.SetActive(false);
                });

            MoveLogic.Destroyed
                .Where(isDestroyed => !isDestroyed)
                .Subscribe(async _ =>
                {
                    await MoveLogic.ResetPosition(CancellationToken);
                    this.gameObject.SetActive(true);
                });

            // ���̃I�u�W�F�N�g�ƏՓ˂����Ƃ��̏���
            this.OnTriggerEnter2DAsObservable()
                .Select(collider => collider.gameObject.GetComponent<IBallCollisionHandler>())
                .Where(hit => hit != null)
                .Subscribe(hit =>
                {
                    hit.OnCollisionBall(this);
                })
                .AddTo(this);
        }
        private void OnDestroy()
        {
            MoveLogic?.Dispose();   
        }

        // �ǂƏՓ˂����Ƃ��̏���
        public void OnCollisionWall(WallCore wall)
        {
            // Y���̑��x�𔽓]������
            MoveLogic.FlipVerticalVelocity();

            // SE ���Đ�����
            SoundManager.Instance.PlaySE(SE_TYPE.HitWall);
        }
    }
}