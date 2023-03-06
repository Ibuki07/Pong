using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneStateManager : SingletonMonoBehaviour<SceneStateManager>
{
    //-------------------------------------------------------------------

    public enum SCENE_TYPE
    {
        Title,
        Home,
        StageSelect,
        Explore,
        Result,

    }

    //-------------------------------------------------------------------

    public float Duration => _duration;

    //-------------------------------------------------------------------

    [SerializeField] private Image _imgFade;
    [SerializeField] private Image _imgLoad;
    [SerializeField] private CanvasGroup _cgFade;
    [SerializeField] private float _duration = 1f;

    //-------------------------------------------------------------------

    protected override void Awake()
    {
        base.Awake();

    }

    //-------------------------------------------------------------------

    public void OnFadeIn()
    {
        _imgFade.gameObject.SetActive(true); // 画像をアクティブにする
        _imgFade.raycastTarget = true;

        _cgFade.DOFade(0, _duration)
            .SetLink(_imgFade.gameObject)
            .OnComplete(() =>
            {
                _imgFade.raycastTarget = false;
                //_imgFade.gameObject.SetActive(false); // 画像を非アクティブにする
            });
    }

    /// 引数で指定したシーンへ遷移
    public void LoadSceneFadeOut(SCENE_TYPE sceneType)
    {
        _imgFade.gameObject.SetActive(true); // 画像をアクティブにする

        // 徐々にポップアップを黒にする(フェードアウト
        _cgFade.DOFade(1.0f, _duration)
                .SetLink(_imgFade.gameObject)
                .OnComplete(() => 
                {
                    SceneManager.LoadSceneAsync(sceneType.ToString());
                });
    }

    //-------------------------------------------------------------------
}