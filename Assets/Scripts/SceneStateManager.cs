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
        _imgFade.gameObject.SetActive(true); // �摜���A�N�e�B�u�ɂ���
        _imgFade.raycastTarget = true;

        _cgFade.DOFade(0, _duration)
            .SetLink(_imgFade.gameObject)
            .OnComplete(() =>
            {
                _imgFade.raycastTarget = false;
                //_imgFade.gameObject.SetActive(false); // �摜���A�N�e�B�u�ɂ���
            });
    }

    /// �����Ŏw�肵���V�[���֑J��
    public void LoadSceneFadeOut(SCENE_TYPE sceneType)
    {
        _imgFade.gameObject.SetActive(true); // �摜���A�N�e�B�u�ɂ���

        // ���X�Ƀ|�b�v�A�b�v�����ɂ���(�t�F�[�h�A�E�g
        _cgFade.DOFade(1.0f, _duration)
                .SetLink(_imgFade.gameObject)
                .OnComplete(() => 
                {
                    SceneManager.LoadSceneAsync(sceneType.ToString());
                });
    }

    //-------------------------------------------------------------------
}