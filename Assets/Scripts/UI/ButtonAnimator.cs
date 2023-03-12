using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using Managers;
using Cysharp.Threading.Tasks;

namespace UI
{ 
    public class ButtonAnimator : MonoBehaviour
    {
        public UniTask InitializedAsync => _initializedCompletionSource.Task;

        [SerializeField] private Ease _ease = Ease.OutCubic;
        [SerializeField] private float _duration = 0.25f;
        [SerializeField] private float _inactiveAlpha = 0.75f;
        [SerializeField] private float _activeAlpha = 1f;
        [SerializeField] private float _fadeOutAlpha = 0f;
        [SerializeField] private float _fadeInAlpha = 0.75f;

        private Button _button;
        private CanvasGroup _buttonCanvasGroup;
        private Transform _buttonTransform;
        private Vector2 _baseScale;

        private readonly UniTaskCompletionSource _initializedCompletionSource = new UniTaskCompletionSource();

        private void Awake()
        {
            // コンポーネントをキャッシュして、アルファ値とスケールの初期値を設定
            _buttonTransform = transform;
            _button = GetComponent<Button>();
            _buttonCanvasGroup = GetComponent<CanvasGroup>();
            _buttonCanvasGroup.alpha = _inactiveAlpha;
            _baseScale = _buttonTransform.localScale;
        }

        private void Start()
        {
            // UniTaskCompletionSourceの完了を通知し、初期化を完了
            _initializedCompletionSource.TrySetResult();

            // ボタンのマウスオーバー時のアニメーション
            _button.OnPointerEnterAsObservable()
                .Where(_ => _button.interactable)
                .Subscribe(_ =>
                {
                    // 音を再生し、ボタンのCanvasGroupのアルファ値を変更
                    SoundManager.Instance.PlaySE(SE_TYPE.ButtonOnPointerEnter);
                    _buttonCanvasGroup.DOFade(_activeAlpha, _duration).SetEase(_ease);
                })
                .AddTo(this);

            // ボタンからマウスが離れた時のアニメーション
            _button.OnPointerExitAsObservable()
                .Where(_ => _button.interactable)
                .Subscribe(_ =>
                {
                    // ボタンのCanvasGroupのアルファ値を変更
                    _buttonCanvasGroup.DOFade(_inactiveAlpha, _duration).SetEase(_ease);
                })
                .AddTo(this);

            // ボタンをクリックした時のアニメーション
            _button.OnPointerDownAsObservable()
                .Where(_ => _button.interactable)
                .Subscribe(_ =>
                {
                    // 音を再生し、ボタンのTransformのスケールを変更
                    SoundManager.Instance.PlaySE(SE_TYPE.ButtonOnClick);
                    _buttonTransform.DOScale(_baseScale * 0.85f, _duration).SetEase(_ease);
                })
                .AddTo(this);

            // ボタンクリックが終了した時のアニメーション
            _button.OnPointerUpAsObservable()
                .Where(_ => _button.interactable)
                .Subscribe(_ =>
                {
                    // ボタンのTransformのスケールを変更
                    _buttonTransform.DOScale(_baseScale, _duration).SetEase(_ease);
                })
                .AddTo(this);
        }

        private void OnDestroy()
        {
            _initializedCompletionSource.TrySetCanceled();
        }

        // ボタンの表示アニメーション
        public Sequence StartDisplayAnimation()
        {
            _buttonCanvasGroup.alpha = _fadeOutAlpha;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_buttonCanvasGroup.DOFade(_fadeInAlpha, _duration));

            return sequence;
        }
    }
}