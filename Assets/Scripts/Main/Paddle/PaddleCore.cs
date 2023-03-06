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

        // --------------------------------------------------
        [SerializeField] private Button _autoMoveButton;
        [SerializeField] private BallCore _ball;
        [SerializeField] private Vector3 _initialVelocity;

        private PaddleMoveInput _moveInput;
        private PaddleMoveSimulation _moveSimulation;
        private PaddleAutoMoveInput _autoMoveInput;
        private PaddleAutoMovePresentation _autoMovePresentation;


        // --------------------------------------------------

        public void OnCollisionBall(BallCore ball)
        {
            var offsetVerticalVelocity = MoveLogic.OnRandomizeVerticalVelocity(InitialVelocity);
            ball.MoveLogic.OnAdditionVelocity(offsetVerticalVelocity * 0.5f);
            ball.MoveLogic.FlipHorizontalVelocity();
            SoundManager.Instance.PlaySE(SoundManager.SEType.HitPaddle);
        }

        public void OnCollisionWall(WallCore wall)
        {
            MoveLogic.OnStopPaddle();
            AutoMoveLogic.OnStopPaddle();
        }

        // --------------------------------------------------

        private void Start()
        {
            // 初期化
            _moveSimulation = new PaddleMoveSimulation(InitialVelocity);
            _moveInput = new PaddleMoveInput(this);
            MoveLogic = new PaddleMoveLogic(_moveInput, _moveSimulation, this);
            _autoMoveInput = new PaddleAutoMoveInput(this, _autoMoveButton);
            AutoMoveLogic = new PaddleAutoMoveLogic(_autoMoveInput, _moveSimulation, this, _ball);
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