using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paddle
{
    // ���[�J�����W���擾�E�ݒ肷�邽�߂̃C���^�[�t�F�C�X
    public interface IPaddleLocalPositionAdapter
    {
        public Vector3 LocalPosition { get; set; }  // ���[�J�����W���擾�E�ݒ肷��v���p�e�B
    }
}