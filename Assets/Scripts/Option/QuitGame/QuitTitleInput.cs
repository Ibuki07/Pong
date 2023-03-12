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
            // QuitTitleボタンが最初にクリックされたときに、_quitTitleをtrueに設定する。
            quitTitleButton.OnClickAsObservable()
                .First()
                .Subscribe(_ =>
                {
                    _quitTitle.Value = true;
                });
        }
    }
}