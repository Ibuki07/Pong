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
        _fadeImage.gameObject.SetActive(true); // �摜���A�N�e�B�u�ɂ���
        _fadeImage.raycastTarget = true;

        _fadeCanvasGroup.DOFade(_fadeInValue, _fadeDuration)
            .SetLink(_fadeImage.gameObject)
            .OnComplete(() =>
            {
                _fadeImage.raycastTarget = false;
            });
    }

    /// �����Ŏw�肵���V�[���֑J��
    public void LoadSceneFadeOut(SceneType sceneType)
    {
        _fadeImage.gameObject.SetActive(true); // �摜���A�N�e�B�u�ɂ���
        _fadeImage.raycastTarget = true;

        // ���X�Ƀ|�b�v�A�b�v�����ɂ���(�t�F�[�h�A�E�g
        _fadeCanvasGroup.DOFade(_fadeOutValue, _fadeDuration)
                .SetLink(_fadeImage.gameObject)
                .OnComplete(() => 
                {
                    SceneManager.LoadSceneAsync(sceneType.ToString());
                });
    }

    //-------------------------------------------------------------------
}