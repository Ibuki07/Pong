using UniRx;
using UnityEngine.UI;

namespace Option
{
    public class QuitGameInput
    {
        // �Q�[���I���v���̏�Ԃ�\�� ReactiveProperty
        public IReadOnlyReactiveProperty<bool> QuitGame => _quitGame;
        // �Q�[���I���̃L�����Z���v���̏�Ԃ�\�� ReactiveProperty
        public IReadOnlyReactiveProperty<bool> CancelQuitGame => _cancelQuitGame;

        // �Q�[���I���v���̏�Ԃ��i�[���� ReactiveProperty
        private readonly ReactiveProperty<bool> _quitGame = new ReactiveProperty<bool>(false);
        // �Q�[���I���̃L�����Z���v���̏�Ԃ��i�[���� ReactiveProperty
        private readonly ReactiveProperty<bool> _cancelQuitGame = new ReactiveProperty<bool>(false);

        public QuitGameInput(
            Button quitGameButton,
            Button decisionButton,
            Button cancelButton)
        {
            // �Q�[���I���m��{�^���������ꂽ���̏���
            decisionButton.OnClickAsObservable()
                // �ŏ���1�񂾂���������悤�ɂ���
                .First()
                .Subscribe(_ =>
                {
                    // �Q�[���I���v���𔭍s����
                    _quitGame.Value = true;
                });

            // �Q�[���I���L�����Z���{�^���������ꂽ���̏���
            cancelButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    // �Q�[���I���L�����Z���v���𔭍s����
                    _cancelQuitGame.Value = true;
                });

            // �Q�[���I���{�^���������ꂽ���̏���
            quitGameButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    // �Q�[���I���L�����Z���v�����N���A����
                    _cancelQuitGame.Value = false;
                });
        }
    }
}