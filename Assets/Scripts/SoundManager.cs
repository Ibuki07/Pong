using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Managers
{
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        [SerializeField] private float _crossFadeTime = 1.0f;
        [SerializeField] private AudioClip[] _bgmClips;
        [SerializeField] private AudioClip[] _seClips;

        private AudioSource[] _bgmSources = new AudioSource[2];
        private DataManager _dataManager;
        private float[] _volumeValues = new float[2];
        private bool _isCrossFading;

        protected override void Awake()
        {
            base.Awake();

            // BGM�p AudioSource�ǉ�
            _bgmSources[0] = this.gameObject.AddComponent<AudioSource>();
            _bgmSources[1] = this.gameObject.AddComponent<AudioSource>();
        }
        private void Start()
        {
            _dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();

            _volumeValues[(int)SOUND_TYPE.BGM] = _dataManager.SaveData.VolumeValues[(int)SOUND_TYPE.BGM];
            _volumeValues[(int)SOUND_TYPE.SE] = _dataManager.SaveData.VolumeValues[(int)SOUND_TYPE.SE];
            Observable.EveryUpdate()
                .Where(_ => !_isCrossFading)
                .Subscribe(_ =>
                {
                    _bgmSources[0].volume = _volumeValues[(int)SOUND_TYPE.BGM];
                    _bgmSources[1].volume = _volumeValues[(int)SOUND_TYPE.BGM];
                })
                .AddTo(this);
        }

        public void PlayBGM(BGM_TYPE bgmType, bool isloop = true)
        {
            int index = (int)bgmType;
            // ������BGM_TYPE���n���ꂽ�ꍇ�͉������Ȃ�
            if (index < 0 || index >= _bgmClips.Length)
            {
                return;
            }

            // ���łɓ���BGM���Đ����̏ꍇ�͉������Ȃ�
            if (_bgmSources[0].clip == _bgmClips[index] || _bgmSources[1].clip == _bgmClips[index])
            {
                return;
            }

            if (_bgmSources[0].clip == null && _bgmSources[1].clip == null)
            {
                // �����̃\�[�X���󂢂Ă���ꍇ�A[0]��BGM��ݒ肵�čĐ�
                _bgmSources[0].loop = isloop;
                _bgmSources[0].clip = _bgmClips[index];
                _bgmSources[0].Play();
            }
            else
            {
                // �ǂ��炩�̃\�[�X���g�p���̏ꍇ�A�N���X�t�F�[�h��BGM��؂�ւ���
                CrossFadeChangeBGM(index, isloop).Forget();
            }
        }

        private async UniTask CrossFadeChangeBGM(int index, bool isLoop)
        {
            _isCrossFading = true;

            AudioSource sourceToFadeOut = _bgmSources[0].clip != null ? _bgmSources[0] : _bgmSources[1];
            AudioSource sourceToFadeIn = sourceToFadeOut == _bgmSources[0] ? _bgmSources[1] : _bgmSources[0];

            // �V����BGM��ݒ肵�čĐ�
            sourceToFadeIn.clip = _bgmClips[index];
            sourceToFadeIn.loop = isLoop;
            sourceToFadeIn.volume = 0;
            sourceToFadeIn.Play();

            // �Â�BGM���t�F�[�h�A�E�g
            await sourceToFadeOut.DOFade(0, _crossFadeTime).SetEase(Ease.Linear).AsyncWaitForCompletion();
            sourceToFadeOut.Stop();
            sourceToFadeOut.clip = null;

            _isCrossFading = false;
        }

        public void PlaySE(SE_TYPE seType)
        {
            int index = (int)seType;
            if (index < 0 || _seClips.Length <= index)
            {
                return;
            }
            AudioSource.PlayClipAtPoint(_seClips[index], Camera.main.transform.position, _volumeValues[(int)SOUND_TYPE.SE]);
        }

        // BGM�̍Đ����~����
        public void MuteBGM()
        {
            foreach (var bgmSource in _bgmSources)
            {
                bgmSource.Stop();
            }
        }

        // BGM�̍Đ����ĊJ����
        public void ResumeBGM()
        {
            foreach (var bgmSource in _bgmSources)
            {
                bgmSource.Play();
            }
        }

        public float GetVolumeValue(SOUND_TYPE soundType)
        {
            int index = (int)soundType;
            return _dataManager.SaveData.VolumeValues[index];
        }

        public void SetVolumeValue(SOUND_TYPE soundType, float volumeValue)
        {
            int index = (int)soundType;
            _volumeValues[index] = volumeValue;
            _dataManager.SaveData.VolumeValues[index] = (float)System.Math.Round(volumeValue, 2);
        }
    }
}