using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;

namespace Option 
{ 
    public class DeleteSaveData : MonoBehaviour
    {
        //-------------------------------------------------------------------

        private CanvasGroup _deletedTextCanvasGroup;

        //-------------------------------------------------------------------

        private DataManager _dataManager;
        private Button _deleteButton;
        private Transform _deleteButtonTransform;
        private bool _isDeleted = true;

        private readonly string _dataManagerTag = "DataManager";
        private readonly string _titleSceneTag = "Title";

        //-------------------------------------------------------------------

        void Start()
        {
            // 初期化
            TryGetComponent(out _deleteButton);
            TryGetComponent(out _deleteButtonTransform);
            _deletedTextCanvasGroup = _deleteButtonTransform.GetChild(0).GetComponent<CanvasGroup>();

            _dataManager = GameObject.FindWithTag(_dataManagerTag).GetComponent<DataManager>();
            _deletedTextCanvasGroup.alpha = 0;
            _deletedTextCanvasGroup.gameObject.SetActive(false);
            if(SceneManager.GetActiveScene().name != _titleSceneTag)
            {
                this.gameObject.SetActive(false);
            }

            //-------------------------------------------------------------------

            // データを削除するボタン
            _deleteButton
                .OnClickAsObservable()
                .Where(_ => _isDeleted)
                .Subscribe(_ =>
                {
                    _isDeleted = false;
                    _dataManager.DeleteSaveData();
                    DisplayAnimationDeletedText();
                });
        }

        //-------------------------------------------------------------------

        // フェード
        private void DisplayAnimationDeletedText()
        {
            _deletedTextCanvasGroup.gameObject.SetActive(true);

            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(_deletedTextCanvasGroup.DOFade(1, 0.5f))
                .Append(_deletedTextCanvasGroup
                   .DOFade(0, 0.5f)
                   .SetDelay(1f)
                   .OnComplete(() =>
                   {
                       _deletedTextCanvasGroup.gameObject.SetActive(false);
                       _isDeleted = true;
                   }));
        }

        //-------------------------------------------------------------------
    }
}
