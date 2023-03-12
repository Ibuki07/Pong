using Managers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Title
{
    public class TitleInput
    {
        // �Q�[���J�n�t���O�̓ǂݎ���pReactiveProperty
        public IReadOnlyReactiveProperty<bool> IsGameStart => _isGameStart;

        private readonly ReactiveProperty<bool> _isGameStart = new ReactiveProperty<bool>(false);
        private Button _gameStartButton;

        public TitleInput(Button gameStartButton)
        {
            // �Q�[���X�^�[�g�{�^�����N���b�N���ꂽ�Ƃ��ɌĂ΂��C�x���g
            gameStartButton.OnClickAsObservable().Subscribe(_ =>
            {
                SoundManager.Instance.PlaySE(SE_TYPE.Start);
                // �Q�[���J�n�t���O�𗧂Ă�
                _isGameStart.Value = true;
            });
        }

        public void OnGameStartInput()
        {
            // �G���^�[�L�[�������ꂽ�Ƃ�
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SoundManager.Instance.PlaySE(SE_TYPE.Start);
                // �Q�[���J�n�t���O�𗧂Ă�
                _isGameStart.Value = true;
            }
        }



    }
}