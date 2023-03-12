using UniRx;
using UnityEngine;

namespace Option
{
    public class QuitGameLogic
    {
        public QuitGameLogic(QuitGameInput input)
        {
            // QuitGameInput����QuitGame��true�ɂȂ������A�Q�[�����I������
            input.QuitGame
                .Where(isQuitGame => isQuitGame)
                .Subscribe(_ =>
                {
#if UNITY_EDITOR
                    // Unity Editor��Ŏ��s���̏ꍇ�A�Đ����~����
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    // ���s���̃A�v���P�[�V�������I������
                    Time.timeScale = 1;
                    Application.Quit();
#endif
                });
        }
    }
}