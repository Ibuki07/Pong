using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GraphicSettingLogic
{
    private readonly ReactiveProperty<bool> _fullScreen = new ReactiveProperty<bool>(false);

    public GraphicSettingLogic(Button fullScreenButton) 
    {
        Image fullScreenButtonImage = fullScreenButton.GetComponent<Image>();

        // �t���X�N���[��
        fullScreenButton
        .OnClickAsObservable()
        .Where(_ => !_fullScreen.Value)
        .Subscribe(_ =>
        {
            _fullScreen.Value = true;        
        });

        // �E�B���h�E
        fullScreenButton
        .OnClickAsObservable()
        .Where(_ => _fullScreen.Value)
        .Subscribe(_ =>
        {
            _fullScreen.Value = false;
        });

        _fullScreen
        .Where(isFullScreen => isFullScreen)
        .Subscribe(_ =>
        {
            fullScreenButtonImage.color = Color.red;
            Screen.fullScreen = true;
        });

        _fullScreen
        .Where(isFullScreen => !isFullScreen)
        .Subscribe(_ =>
        {
            fullScreenButtonImage.color = Color.white;
            Screen.fullScreen = false;
        });
    }

}
