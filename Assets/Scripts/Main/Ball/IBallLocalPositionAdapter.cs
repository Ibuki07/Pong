using UnityEngine;

namespace Ball
{
    // ���[�J�����W���擾�E�ݒ肷�邽�߂̃C���^�[�t�F�C�X
    public interface IBallLocalPositionAdapter
    {
        public Vector3 LocalPosition { get; set; }  // ���[�J�����W���擾�E�ݒ肷��v���p�e�B
    }
}