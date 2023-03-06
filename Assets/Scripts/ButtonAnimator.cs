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
        // ----------------------------------------------
        public UniTask InitializedAsync => _uniTaskCompletionSource.Task;
        private readonly UniTaskCompletionSource _uniTaskCompletionSource = new UniTaskCompletionSource();

        [SerializeField] float _duration = 0.25f;
        [SerializeField] Ease _ease = Ease.OutCubic;

        private Button _button;
        private CanvasGroup _cgButton;
        private Transform _tranButton;
        private Vector2 _baseScaleButton;
        
        // ----------------------------------------------

        private void OnDestroy()
        {
            _uniTaskCompletionSource.TrySetCanceled();
        }

        void Start()
        {
            _tranButton = GetComponent<Transform>();
            _button = GetComponent<Button>();
            _cgButton = GetComponent<CanvasGroup>();

            _cgButton.alpha = 0.75f;
            _baseScaleButton = _tranButton.localScale;

            _uniTaskCompletionSource.TrySetResult();



            _button.OnPointerEnterAsObservable()
                .Subscribe(_ =>
                {
                    SoundManager.Instance.PlaySE(SoundManager.SEType.ButtonOnPointerEnter);
                    _cgButton.DOFade(1f, _duration).SetEase(_ease);


                }).AddTo(this);

            _button.OnPointerExitAsObservable()
                .Subscribe(_ =>
                {
                    _cgButton.DOFade(0.75f, _duration).SetEase(_ease);

                }).AddTo(this);

            _button.OnPointerDownAsObservable()
                .Subscribe(_ =>
                {
                    SoundManager.Instance.PlaySE(SoundManager.SEType.ButtonOnClick);
                    _tranButton.DOScale(_baseScaleButton * 0.85f, _duration).SetEase(_ease);
                }).AddTo(this);

            _button.OnPointerUpAsObservable()
                .Subscribe(_ =>
                {
                    _tranButton.DOScale(_baseScaleButton, _duration).SetEase(_ease);
                }).AddTo(this);

        }

        public Sequence DisplayAnimation()
        {
            _cgButton.alpha = 0f;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_cgButton.DOFade(0.75f, _duration));

            return sequence;
        }

    }
}