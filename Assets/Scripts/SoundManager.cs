using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Managers
{
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        public enum BGMType
        {
            // BGM�p�̗񋓎q���Q�[���ɍ��킹�ēo�^
            Title,
            Main_1,
            Main_2,
            Main_3,

        }

        public enum SEType
        {
            // SE�p�̗񋓎q���Q�[���ɍ��킹�ēo�^
            Start,
            ButtonOnClick,
            ButtonOnPointerEnter,
            GameStart,
            GameEnd,
            HitPaddle,
            HitWall,
            HitGoal,
        }

        public enum SoundType
        {
            BGM,
            SE,
        }
        public UniTask InitializedAsync => _uniTaskCompletionSource.Task;
        private readonly UniTaskCompletionSource _uniTaskCompletionSource = new UniTaskCompletionSource();

        [SerializeField] private float _crossFadeTime = 1.0f;
        [SerializeField] private AudioClip[] _bgmClips;
        [SerializeField] private AudioClip[] _seClips;


        private AudioSource[] _bgmSources = new AudioSource[2];
        private AudioSource[] _seSources = new AudioSource[32];
        private DataManager _dataManager;

        private float[] _volumeValues = new float[2];
        private bool _isCrossFading;


        protected override void Awake()
        {
            base.Awake();
            _dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();

            // BGM�p AudioSource�ǉ�
            _bgmSources[0] = gameObject.AddComponent<AudioSource>();
            _bgmSources[1] = gameObject.AddComponent<AudioSource>();


            // SE�p AudioSource�ǉ�
            for (int i = 0; i < _seSources.Length; i++)
            {
                _seSources[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        private void Start()
        {
            _volumeValues[(int)SoundType.BGM] = _dataManager.SaveData.VolumeValues[(int)SoundType.BGM];
            _volumeValues[(int)SoundType.SE] = _dataManager.SaveData.VolumeValues[(int)SoundType.SE];


            Observable.EveryUpdate().Subscribe(_ =>
            {
                // �{�����[���ݒ�
                if (!_isCrossFading)
                {
                    _bgmSources[0].volume = _volumeValues[(int)SoundType.BGM];
                    _bgmSources[1].volume = _volumeValues[(int)SoundType.BGM];
                }

                foreach (AudioSource source in _seSources)
                {
                    source.volume = _volumeValues[(int)SoundType.SE];
                }
            }).AddTo(this);

            _uniTaskCompletionSource.TrySetResult();
        }

        // BGM�Đ�
        public void PlayBGM(BGMType bgmType, bool loopFlg = true)
        {
            // BGM�Ȃ��̏�Ԃɂ���ꍇ            
            if ((int)bgmType == 999)
            {
                StopBGM();
                return;
            }

            int index = (int)bgmType;

            if (index < 0 || _bgmClips.Length <= index)
            {
                return;
            }

            // ����BGM�̏ꍇ�͉������Ȃ�
            if (_bgmSources[0].clip != null && _bgmSources[0].clip == _bgmClips[index])
            {
                return;
            }
            else if (_bgmSources[1].clip != null && _bgmSources[1].clip == _bgmClips[index])
            {
                return;
            }

            // �t�F�[�h��BGM�J�n
            if (_bgmSources[0].clip == null && _bgmSources[1].clip == null)
            {
                _bgmSources[0].loop = loopFlg;
                _bgmSources[0].clip = _bgmClips[index];
                _bgmSources[0].Play();
            }
            else
            {
                // �N���X�t�F�[�h����
                CrossFadeChangeBGM(index, loopFlg).Forget();
            }
        }
        /// BGM�̃N���X�t�F�[�h����
        private async UniTaskVoid CrossFadeChangeBGM(int index, bool loopFlg)
        {
            _isCrossFading = true;
            if (_bgmSources[0].clip != null)
            {
                // [0]���Đ�����Ă���ꍇ�A[0]�̉��ʂ����X�ɉ����āA[1]��V�����ȂƂ��čĐ�
                _bgmSources[1].volume = 0;
                _bgmSources[1].clip = _bgmClips[index];
                _bgmSources[1].loop = loopFlg;
                _bgmSources[1].Play();
                await _bgmSources[0].DOFade(0, _crossFadeTime).SetEase(Ease.Linear);

                _bgmSources[0].Stop();
                _bgmSources[0].clip = null;
            }
            else
            {
                // [1]���Đ�����Ă���ꍇ�A[1]�̉��ʂ����X�ɉ����āA[0]��V�����ȂƂ��čĐ�
                _bgmSources[0].volume = 0;
                _bgmSources[0].clip = _bgmClips[index];
                _bgmSources[0].loop = loopFlg;
                _bgmSources[0].Play();
                await _bgmSources[1].DOFade(0, _crossFadeTime).SetEase(Ease.Linear);

                _bgmSources[1].Stop();
                _bgmSources[1].clip = null;
            }
            _isCrossFading = false;
        }



        /// BGM���S��~
        public void StopBGM()
        {
            _bgmSources[0].Stop();
            _bgmSources[1].Stop();
            _bgmSources[0].clip = null;
            _bgmSources[1].clip = null;
        }

        /// SE�Đ�
        public void PlaySE(SEType seType)
        {
            int index = (int)seType;
            if (index < 0 || _seClips.Length <= index)
            {
                return;
            }

            // �Đ����ł͂Ȃ�AudioSouce��������SE��炷
            foreach (AudioSource source in _seSources)
            {
                if (false == source.isPlaying)
                {
                    source.clip = _seClips[index];
                    source.Play();
                    return;
                }
            }
        }

        /// SE��~
        public void StopSE()
        {
            // �S�Ă�SE�p��AudioSouce���~����
            foreach (AudioSource source in _seSources)
            {
                source.Stop();
                source.clip = null;
            }
        }

        /// BGM�ꎞ��~
        public void MuteBGM()
        {
            _bgmSources[0].Stop();
            _bgmSources[1].Stop();
        }

        /// �ꎞ��~��������BGM���Đ�(�ĊJ)
        public void ResumeBGM()
        {
            _bgmSources[0].Play();
            _bgmSources[1].Play();
        }

        public float GetVolumeValue(SoundType soundType)
        {
            int index = (int)soundType;
            return _dataManager.SaveData.VolumeValues[index];
        }

        public void SetVolumeValue(SoundType soundType, float volumeValue)
        {
            int index = (int)soundType;
            _volumeValues[index] = volumeValue;
            _dataManager.SaveData.VolumeValues[index] = (float)System.Math.Round(volumeValue, 2);
        }

        private void OnDestroy()
        {
            _uniTaskCompletionSource.TrySetCanceled();
        }
    }
}