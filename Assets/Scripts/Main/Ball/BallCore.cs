using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;
using Wall;

namespace Ball
{
    public class BallCore : MonoBehaviour, IBallLocalPositionAdapter, IWallCollisionHandler
    {
        // 初速度
        public Vector3 InitialVelocity;     
        // ILocalPositionAdapterインターフェイスの実装
        public Vector3 LocalPosition
        {
            // ローカル座標を取得する
            get { return this.transform.localPosition; }
            // ローカル座標を設定する
            set { transform.localPosition = value; }
        }
        public BallLogic BallLogic { get; private set; }
        public CancellationToken CancellationToken { get; private set; }

        // --------------------------------------------------

        private BallSimulation _ballSimulation;
        // FixedUpdateの購読解除用
        private System.IDisposable _fixedUpdateDisposable;         

        // --------------------------------------------------

        public void OnCollisionWall(WallCore wall)
        {
            BallLogic.OnFlipVerticalVelocity(Vector3.zero);
        }

        // --------------------------------------------------

        private void Start()
        {
            // 初期化
            _ballSimulation = new BallSimulation(InitialVelocity);
            BallLogic = new BallLogic(this, _ballSimulation);
            CancellationToken = this.GetCancellationTokenOnDestroy();

            // 破壊されたときにゲームオブジェクトを破棄する
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

            // FixedUpdateをエミュレートするObservableを作成する
            var fixedUpdateObservable = Observable.EveryFixedUpdate().Publish().RefCount();

            // Observableをサブスクライブする
            _fixedUpdateDisposable = fixedUpdateObservable.Subscribe(_ =>
            {
                BallLogic.UpdateLocalPosition(Time.fixedDeltaTime);
            });
        }

        private void OnDestroy()
        {
            // IDisposableを破棄する
            _fixedUpdateDisposable?.Dispose();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            var hit = collider.gameObject.GetComponent<IBallCollisionHandler>();
            hit?.OnCollisionBall(this);
        }



    }
}