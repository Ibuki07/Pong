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

        // ゲームオブジェクトが破棄されるときにキャンセルされるトークン
        public CancellationToken CancellationToken { get; private set; }

        // 初速度
        [SerializeField] private Vector3 _initialVelocity;

        private BallMoveSimulation _moveSimulation;

        private void Start()
        {
            // 初期化
            _moveSimulation = new BallMoveSimulation(InitialVelocity);
            MoveLogic = new BallMoveLogic(_moveSimulation, this);
            CancellationToken = this.GetCancellationTokenOnDestroy();

            // ボールが破壊されたときに処理する
            MoveLogic.Destroyed
                .Where(isDestroyed => isDestroyed)
                .Subscribe(_ =>
                {
                    // ゲームオブジェクトを非アクティブにする
                    this.gameObject.SetActive(false);
                });

            MoveLogic.Destroyed
                .Where(isDestroyed => !isDestroyed)
                .Subscribe(async _ =>
                {
                    await MoveLogic.ResetPosition(CancellationToken);
                    this.gameObject.SetActive(true);
                });

            // 他のオブジェクトと衝突したときの処理
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

        // 壁と衝突したときの処理
        public void OnCollisionWall(WallCore wall)
        {
            // Y軸の速度を反転させる
            MoveLogic.FlipVerticalVelocity();

            // SE を再生する
            SoundManager.Instance.PlaySE(SE_TYPE.HitWall);
        }
    }
}