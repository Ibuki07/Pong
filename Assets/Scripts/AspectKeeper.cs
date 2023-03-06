using UniRx;
using UnityEngine;

[ExecuteAlways]
public class AspectKeeper : MonoBehaviour
{
    [SerializeField] private Camera _targetCamera; //�ΏۂƂ���J����
    [SerializeField] private Vector2 _aspectVec; //�ړI�𑜓x

    private float _screenAspect;
    private float _targetAspect;
    private float _magRate;
    private Rect _viewportRect;

    private void Update()
    {
        _screenAspect = Screen.width / (float)Screen.height; //��ʂ̃A�X�y�N�g��
        _targetAspect = _aspectVec.x / _aspectVec.y; //�ړI�̃A�X�y�N�g��
        _magRate = _targetAspect / _screenAspect; //�ړI�A�X�y�N�g��ɂ��邽�߂̔{��
        _viewportRect = new Rect(0, 0, 1, 1); //Viewport�����l��Rect���쐬

        if (_magRate < 1)
        {
            _viewportRect.width = _magRate; //�g�p���鉡����ύX
            _viewportRect.x = 0.5f - _viewportRect.width * 0.5f;//������
        }
        else
        {
            _viewportRect.height = 1 / _magRate; //�g�p����c����ύX
            _viewportRect.y = 0.5f - _viewportRect.height * 0.5f;//�����]��
        }

        _targetCamera.rect = _viewportRect; //�J������Viewport�ɓK�p
    }
}
