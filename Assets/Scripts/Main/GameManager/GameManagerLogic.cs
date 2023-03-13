using UnityEngine;

namespace Managers
{
    public class GameManagerLogic
    {
        public BGM_TYPE RandomizeBGM()
        {
            // BGM‚ğƒ‰ƒ“ƒ_ƒ€‚ÉÄ¶‚·‚é
            var bgm = Random.Range((int)BGM_TYPE.Main_1, (int)BGM_TYPE.Main_3);
            return (BGM_TYPE)bgm;
        }
    }
}