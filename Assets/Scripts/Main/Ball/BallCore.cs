using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;
using Wall;

namespace Ball
{
    public class BallCore : MonoBehaviour, IBallLocalPositionAdapter, IWallCollisionHandler
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
        public BallLogic BallLogic { get; private set; }
        public CancellationToken CancellationToken { get; private set; }

        // --------------------------------------------------

        private BallSimulation _ballSimulation;
        // FixedUpdate�̍w�ǉ����p
        private System.IDisposable _fixedUpdateDisposable;         

        // --------------------------------------------------

        public void OnCollisionWall(WallCore wall)
        {
            BallLogic.OnFlipVerticalVelocity(Vector3.zero);
        }

        // --------------------------------------------------

        private void Start()
        {
            // ������
            _ballSimulation = new BallSimulation(InitialVelocity);
            BallLogic = new BallLogic(this, _ballSimulation);
            CancellationToken = this.GetCancellationTokenOnDestroy();

            // �j�󂳂ꂽ�Ƃ��ɃQ�[���I�u�W�F�N�g��j������
            BallLogic.Destroyed
                .Where(isDestroyed => isDestroyed)
                .Subscribe(_ =>
                {
                    this.gameObject.SetActive(false);
                });

            BallLogic.Destroyed
                .Where(isDestroyed => !isDestroyed)
                .Skip(1)
                .Subscribe(async _ =>
                {
                    Debug.Log("CreatedBall");
                    await BallLogic.OnResetPosition(CancellationToken);
                    this.gameObject.SetActive(true);
                });

            // FixedUpdate���G�~�����[�g����Observable���쐬����
            var fixedUpdateObservable = Observable.EveryFixedUpdate().Publish().RefCount();

            // Observable���T�u�X�N���C�u����
            _fixedUpdateDisposable = fixedUpdateObservable.Subscribe(_ =>
            {
                BallLogic.UpdateLocalPosition(Time.fixedDeltaTime);
            });
        }

        private void OnDestroy()
        {
            // IDisposable��j������
            _fixedUpdateDisposable?.Dispose();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            var hit = collider.gameObject.GetComponent<IBallCollisionHandler>();
            hit?.OnCollisionBall(this);
        }



    }
}