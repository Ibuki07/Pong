using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paddle
{
    // ローカル座標を取得・設定するためのインターフェイス
    public interface IPaddleLocalPositionAdapter
    {
        public Vector3 LocalPosition { get; set; }  // ローカル座標を取得・設定するプロパティ
    }
}