using Managers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Title
{
    public class TitleCore : MonoBehaviour
    {

        [SerializeField] private Button _gameStartButton;
        [SerializeField] private CanvasGroup titleLogo;
        [SerializeField] private CanvasGroup startText;

        private TitleInput _titleInput;
        private TitlePresentation _titlePresentation;

        private void Start()
        {
            // �V�[�����̃t�F�[�h�C������
            SceneStateManager.Instance.StartFadeIn();

            // �^�C�g��BGM�Đ�
            SoundManager.Instance.PlayBGM(BGM_TYPE.Title);

            _titleInput = new TitleInput(_gameStartButton);
            _titlePresentation = new TitlePresentation(titleLogo, startText);

            // �Q�[���X�^�[�g�̓��͂��������ꍇ�̏���
            _titleInput.IsGameStart
                .Where(isGameStart => isGameStart)
                // ��񂾂����s
                .Take(1) 
                .Subscribe(_ =>
                {
                    // �Q�[�����C���V�[���ɑJ�ڂ���
                    SceneStateManager.Instance.LoadSceneFadeOut(SCENE_TYPE.Main); 
                });

            // ���t���[���A�Q�[���X�^�[�g�̓��͂��擾����
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    _titleInput.OnGameStartInput();
                })
                .AddTo(this);
        }
    }
}