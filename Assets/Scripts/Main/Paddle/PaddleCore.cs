using Managers;
using UnityEngine;
using Wall;
using UnityEngine.UI;
using Ball;

namespace Paddle
{
    public class PaddleCore : MonoBehaviour, IPaddleLocalPositionAdapter, IBallCollisionHandler, IWallCollisionHandler
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
        public PaddleMoveLogic MoveLogic { get; private set; }
        public PaddleAutoMoveLogic AutoMoveLogic { get; private set; }

        [SerializeField] private Button _autoMoveButton;
        [SerializeField] private BallCore _ball;
        [SerializeField] private Vector3 _initialVelocity;

        private PaddleMoveInput _moveInput;
        private PaddleMoveSimulation _moveSimulation;
        private PaddleAutoMoveInput _autoMoveInput;
        private PaddleAutoMovePresentation _autoMovePresentation;

        // ボールと接触した際の処理
        public void OnCollisionBall(BallCore ball)
        {
            // ボールに加える速度のオフセットを計算する
            var offsetVerticalVelocity = MoveLogic.RandomizeVerticalVelocity(InitialVelocity);
            ball.MoveLogic.AddVelocity(offsetVerticalVelocity * 0.5f);
            // ボールの水平速度を反転させる
            ball.MoveLogic.FlipHorizontalVelocity();

            // SEを再生する
            SoundManager.Instance.PlaySE(SE_TYPE.HitPaddle);
        }

        // 壁と接触した際の処理
        public void OnCollisionWall(WallCore wall)
        {
            // パドルを停止し、自動移動も停止する
            MoveLogic.StopPaddle();
            AutoMoveLogic.OnStopPaddle();
        }

        private void Start()
        {
            // 移動シミュレーションオブジェクトを初期化する
            _moveSimulation = new PaddleMoveSimulation(InitialVelocity);

            // 入力・シミュレーション・アダプタを使って移動ロジックを初期化する
            _moveInput = new PaddleMoveInput(this);
            MoveLogic = new PaddleMoveLogic(_moveInput, _moveSimulation, this);

            // 自動移動ロジックを初期化する
            _autoMoveInput = new PaddleAutoMoveInput(this, _autoMoveButton);
            AutoMoveLogic = new PaddleAutoMoveLogic(_autoMoveInput, _moveSimulation, this, _ball);

            // 自動移動ボタンのCanvasGroupコンポーネントを取得して、プレゼンテーション用のオブジェクトを初期化する
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