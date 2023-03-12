using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Managers
{
    public class SceneStateManager : SingletonMonoBehaviour<SceneStateManager>
    {


        // �t�F�[�h���Ԃ��擾����v���p�e�B
        public float FadeDuration => _fadeDuration;

        [SerializeField] private CanvasGroup _fadeCanvasGroup;
        [SerializeField] private float _fadeDuration = 1f;

        private Image _fadeImage;
        private float _fadeInValue = 0f;
        private float _fadeOutValue = 1f;

        protected override void Awake()
        {
            base.Awake();
            _fadeImage = _fadeCanvasGroup.GetComponent<Image>();
        }

        // �t�F�[�h�C���̃A�j���[�V�������J�n���郁�\�b�h
        public void StartFadeIn()
        {
            // �t�F�[�h�摜���A�N�e�B�u�ɂ���
            _fadeImage.gameObject.SetActive(true);

            // �N���b�N���󂯕t����悤�ɂ���
            _fadeImage.raycastTarget = true;

            // �t�F�[�h�C���̃A�j���[�V�������Đ�����
            _fadeCanvasGroup.DOFade(_fadeInValue, _fadeDuration)
                .SetLink(_fadeImage.gameObject)
                .OnComplete(() =>
                {
                    // �N���b�N���󂯕t���Ȃ��悤�ɂ���
                    _fadeImage.raycastTarget = false;
                });
        }

        // �w�肵���V�[���Ƀt�F�[�h�A�E�g���đJ�ڂ��郁�\�b�h
        public void LoadSceneFadeOut(SCENE_TYPE sceneType)
        {
            // �t�F�[�h�摜���A�N�e�B�u�ɂ���
            _fadeImage.gameObject.SetActive(true);

            // �N���b�N���󂯕t����悤�ɂ���
            _fadeImage.raycastTarget = true; 

            // �t�F�[�h�A�E�g�̃A�j���[�V�������Đ����A�������Ɏw�肵���V�[���ɑJ�ڂ���
            _fadeCanvasGroup
                .DOFade(_fadeOutValue, _fadeDuration)
                .SetLink(_fadeImage.gameObject)
                .OnComplete(() =>
                {
                    SceneManager.LoadSceneAsync(sceneType.ToString());
                });
        }
    }
}