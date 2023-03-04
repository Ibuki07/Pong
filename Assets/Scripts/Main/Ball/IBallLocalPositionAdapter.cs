using UnityEngine;

namespace Ball
{
    // ローカル座標を取得・設定するためのインターフェイス
    public interface IBallLocalPositionAdapter
    {
        public Vector3 LocalPosition { get; set; }  // ローカル座標を取得・設定するプロパティ
    }
}