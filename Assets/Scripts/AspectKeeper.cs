using UnityEngine;

[ExecuteAlways]
public class AspectKeeper : MonoBehaviour
{
    [SerializeField] private Camera _targetCamera;
    // �ړI�𑜓x
    [SerializeField] private Vector2 _aspectVector; 

    private Rect _viewportRect;
    private float _screenAspect;
    private float _targetAspect;
    private float _magRate;

    private void Update()
    {
        if (_targetCamera == null)
        {
            Debug.LogWarning("Target Camera is not set in AspectKeeper script.");
            return;
        }

        // �X�N���[���̃A�X�y�N�g����擾����
        _screenAspect = Screen.width / (float)Screen.height;
        
        // �ړI�̃A�X�y�N�g����擾����
        _targetAspect = _aspectVector.x / _aspectVector.y;

        // ���݂̃A�X�y�N�g��ƖړI�̃A�X�y�N�g�䂪�����ꍇ�͏������X�L�b�v����
        if (_screenAspect == _targetAspect)
        {
            return;
        }

        // �ړI�A�X�y�N�g��ɂ��邽�߂̔{�����v�Z����
        _magRate = _targetAspect / _screenAspect;

        // �J�����̃N���b�s���O���ʂ��ݒ肳��Ă��邱�Ƃ��m�F����
        if (_targetCamera.nearClipPlane == 0f)
        {
            Debug.LogWarning("Near clip plane is not set in Target Camera.");
            return;
        }
        if (_targetCamera.farClipPlane == 0f)
        {
            Debug.LogWarning("Far clip plane is not set in Target Camera.");
            return;
        }

        // Viewport�����l��Rect���쐬
        _viewportRect = new Rect(0, 0, 1, 1);

        // �ړI�̃A�X�y�N�g��ɍ��킹��Viewport�̃T�C�Y��ύX����
        if (_magRate < 1)
        {
            // �g�p���鉡����ύX
            _viewportRect.width = _magRate;

            // ������
            _viewportRect.x = 0.5f - _viewportRect.width * 0.5f;
        }
        else
        {
            // �g�p����c����ύX
            _viewportRect.height = 1 / _magRate;

            // ������
            _viewportRect.y = 0.5f - _viewportRect.height * 0.5f;
        }

        // �J������Viewport�ɓK�p
        _targetCamera.rect = _viewportRect; 
    }
}
