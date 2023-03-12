using UniRx;
using UnityEngine.UI;

namespace Option
{
    public class QuitTitleInput
    {
        public IReadOnlyReactiveProperty<bool> QuitTitle => _quitTitle;

        private readonly ReactiveProperty<bool> _quitTitle = new ReactiveProperty<bool>(false);

        public QuitTitleInput(Button quitTitleButton)
        {
            // QuitTitle�{�^�����ŏ��ɃN���b�N���ꂽ�Ƃ��ɁA_quitTitle��true�ɐݒ肷��B
            quitTitleButton.OnClickAsObservable()
                .First()
                .Subscribe(_ =>
                {
                    _quitTitle.Value = true;
                });
        }
    }
}