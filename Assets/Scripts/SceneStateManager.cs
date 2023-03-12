using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Managers
{
    public class SceneStateManager : SingletonMonoBehaviour<SceneStateManager>
    {


        // フェード時間を取得するプロパティ
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

        // フェードインのアニメーションを開始するメソッド
        public void StartFadeIn()
        {
            // フェード画像をアクティブにする
            _fadeImage.gameObject.SetActive(true);

            // クリックを受け付けるようにする
            _fadeImage.raycastTarget = true;

            // フェードインのアニメーションを再生する
            _fadeCanvasGroup.DOFade(_fadeInValue, _fadeDuration)
                .SetLink(_fadeImage.gameObject)
                .OnComplete(() =>
                {
                    // クリックを受け付けないようにする
                    _fadeImage.raycastTarget = false;
                });
        }

        // 指定したシーンにフェードアウトして遷移するメソッド
        public void LoadSceneFadeOut(SCENE_TYPE sceneType)
        {
            // フェード画像をアクティブにする
            _fadeImage.gameObject.SetActive(true);

            // クリックを受け付けるようにする
            _fadeImage.raycastTarget = true; 

            // フェードアウトのアニメーションを再生し、完了時に指定したシーンに遷移する
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