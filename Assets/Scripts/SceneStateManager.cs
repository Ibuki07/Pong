using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneStateManager : SingletonMonoBehaviour<SceneStateManager>
{
    //-------------------------------------------------------------------

    public enum SceneType
    {
        Title,
        Home,
        StageSelect,
        Main,
        Result,

    }

    //-------------------------------------------------------------------

    public float FadeDuration => _fadeDuration;

    //-------------------------------------------------------------------

    [SerializeField] private CanvasGroup _fadeCanvasGroup;
    [SerializeField] private float _fadeDuration = 1f;
    
    private Image _fadeImage;
    private float _fadeInValue = 0f;
    private float _fadeOutValue = 1f;

    //-------------------------------------------------------------------

    protected override void Awake()
    {
        base.Awake();
        _fadeImage = _fadeCanvasGroup.GetComponent<Image>();
    }

    //-------------------------------------------------------------------

    public void OnFadeIn()
    {
        _fadeImage.gameObject.SetActive(true); // 画像をアクティブにする
        _fadeImage.raycastTarget = true;

        _fadeCanvasGroup.DOFade(_fadeInValue, _fadeDuration)
            .SetLink(_fadeImage.gameObject)
            .OnComplete(() =>
            {
                _fadeImage.raycastTarget = false;
            });
    }

    /// 引数で指定したシーンへ遷移
    public void LoadSceneFadeOut(SceneType sceneType)
    {
        _fadeImage.gameObject.SetActive(true); // 画像をアクティブにする
        _fadeImage.raycastTarget = true;

        // 徐々にポップアップを黒にする(フェードアウト
        _fadeCanvasGroup.DOFade(_fadeOutValue, _fadeDuration)
                .SetLink(_fadeImage.gameObject)
                .OnComplete(() => 
                {
                    SceneManager.LoadSceneAsync(sceneType.ToString());
                });
    }

    //-------------------------------------------------------------------
}