using UniRx;
using UnityEngine;
using Wall;

namespace Paddle
{
    public class PaddleCore : MonoBehaviour, IPaddleLocalPositionAdapter, IBallCollisionHandler, IWallCollisionHandler
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
        public PaddleLogic PaddleLogic { get; private set; }

        // --------------------------------------------------

        [SerializeField] private PaddleMoveInput _paddleMoveInput;
        private PaddleSimulation _paddleSimulation;
        // FixedUpdateの購読解除用
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
            // 初期化
            _paddleSimulation = new PaddleSimulation(InitialVelocity);
            PaddleLogic = new PaddleLogic(this, _paddleMoveInput, _paddleSimulation);

            // FixedUpdateをエミュレートするObservableを作成する
            var fixedUpdateObservable = Observable.EveryFixedUpdate().Publish().RefCount();
            // Observableをサブスクライブする
            _fixedUpdateDisposable = fixedUpdateObservable
                .Subscribe(_ =>
                {
                    PaddleLogic.OnUpdateLocalPosition(Time.fixedDeltaTime);
                });
        }

        private void OnDestroy()
        {
            // IDisposableを破棄する
            _fixedUpdateDisposable?.Dispose();
        }


    }
}