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
        // 初速度
        public Vector3 InitialVelocity => _initialVelocity;
        public Vector3 LocalPosition
        {
            // ローカル座標を取得する
            get { return this.transform.localPosition; }
            // ローカル座標を設定する
            set { transform.localPosition = value; }
        }
        public BallMoveLogic MoveLogic { get; private set; }
        public CancellationToken CancellationToken { get; private set; }

        [SerializeField] private Vector3 _initialVelocity;
        private BallMoveSimulation _moveSimulation;


        public void OnCollisionWall(WallCore wall)
        {
            MoveLogic.FlipVerticalVelocity();
            SoundManager.Instance.PlaySE(SE_TYPE.HitWall);
        }


        private void Start()
        {
            // 初期化
            _moveSimulation = new BallMoveSimulation(InitialVelocity);
            MoveLogic = new BallMoveLogic(_moveSimulation, this);
            CancellationToken = this.GetCancellationTokenOnDestroy();

            // 破壊されたときにゲームオブジェクトを破棄する
            MoveLogic.Destroyed
                .Where(isDestroyed => isDestroyed)
                .Subscribe(_ =>
                {
                    this.gameObject.SetActive(false);
                });

            MoveLogic.Destroyed
                .Where(isDestroyed => !isDestroyed)
                .Subscribe(async _ =>
                {
                    await MoveLogic.ResetPosition(CancellationToken);
                    this.gameObject.SetActive(true);
                });

            this.OnTriggerEnter2DAsObservable().Subscribe(collider =>
            {
                var hit = collider.gameObject.GetComponent<IBallCollisionHandler>();
                hit?.OnCollisionBall(this);
            });
        }
        private void OnDestroy()
        {
            MoveLogic?.Dispose();   
        }
    }
}