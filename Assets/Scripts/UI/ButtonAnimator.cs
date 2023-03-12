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
            // �R���|�[�l���g���L���b�V�����āA�A���t�@�l�ƃX�P�[���̏����l��ݒ�
            _buttonTransform = transform;
            _button = GetComponent<Button>();
            _buttonCanvasGroup = GetComponent<CanvasGroup>();
            _buttonCanvasGroup.alpha = _inactiveAlpha;
            _baseScale = _buttonTransform.localScale;
        }

        private void Start()
        {
            // UniTaskCompletionSource�̊�����ʒm���A������������
            _initializedCompletionSource.TrySetResult();

            // �{�^���̃}�E�X�I�[�o�[���̃A�j���[�V����
            _button.OnPointerEnterAsObservable()
                .Where(_ => _button.interactable)
                .Subscribe(_ =>
                {
                    // �����Đ����A�{�^����CanvasGroup�̃A���t�@�l��ύX
                    SoundManager.Instance.PlaySE(SE_TYPE.ButtonOnPointerEnter);
                    _buttonCanvasGroup.DOFade(_activeAlpha, _duration).SetEase(_ease);
                })
                .AddTo(this);

            // �{�^������}�E�X�����ꂽ���̃A�j���[�V����
            _button.OnPointerExitAsObservable()
                .Where(_ => _button.interactable)
                .Subscribe(_ =>
                {
                    // �{�^����CanvasGroup�̃A���t�@�l��ύX
                    _buttonCanvasGroup.DOFade(_inactiveAlpha, _duration).SetEase(_ease);
                })
                .AddTo(this);

            // �{�^�����N���b�N�������̃A�j���[�V����
            _button.OnPointerDownAsObservable()
                .Where(_ => _button.interactable)
                .Subscribe(_ =>
                {
                    // �����Đ����A�{�^����Transform�̃X�P�[����ύX
                    SoundManager.Instance.PlaySE(SE_TYPE.ButtonOnClick);
                    _buttonTransform.DOScale(_baseScale * 0.85f, _duration).SetEase(_ease);
                })
                .AddTo(this);

            // �{�^���N���b�N���I���������̃A�j���[�V����
            _button.OnPointerUpAsObservable()
                .Where(_ => _button.interactable)
                .Subscribe(_ =>
                {
                    // �{�^����Transform�̃X�P�[����ύX
                    _buttonTransform.DOScale(_baseScale, _duration).SetEase(_ease);
                })
                .AddTo(this);
        }

        private void OnDestroy()
        {
            _initializedCompletionSource.TrySetCanceled();
        }

        // �{�^���̕\���A�j���[�V����
        public Sequence StartDisplayAnimation()
        {
            _buttonCanvasGroup.alpha = _fadeOutAlpha;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_buttonCanvasGroup.DOFade(_fadeInAlpha, _duration));

            return sequence;
        }
    }
}