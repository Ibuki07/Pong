using UnityEngine;

namespace Managers
{
    public class GameManagerLogic
    {
        public BGM_TYPE RandomizeBGM()
        {
            // BGM�������_���ɍĐ�����
            var bgm = Random.Range((int)BGM_TYPE.Main_1, (int)BGM_TYPE.Main_3);
            return (BGM_TYPE)bgm;
        }
    }
}