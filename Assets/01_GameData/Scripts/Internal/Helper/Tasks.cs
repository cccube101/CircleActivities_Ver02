using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Helper
{
    /// <summary>
    /// 共通タスク用ユーティリティクラス
    /// </summary>
    public static class Tasks
    {
        /// <summary>
        /// レクトトランスフォームアイテム
        /// </summary>
        public struct RectItem
        {
            public RectItem(GameObject obj) => Obj = obj;

            public GameObject Obj;
            public readonly RectTransform Rect => Obj.GetComponent<RectTransform>();
        }

        /// <summary>
        /// キャンバスグループアイテム
        /// </summary>
        public struct GroupItem
        {
            public GroupItem(GameObject obj) => Obj = obj;

            public GameObject Obj;
            public readonly CanvasGroup Group => Obj.GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// イメージアイテム
        /// </summary>
        public struct ImageItem
        {
            public ImageItem(GameObject obj) => Obj = obj;

            public GameObject Obj;
            public readonly Image Img => Obj.GetComponent<Image>();
        }


        // ---------------------------- Property
        public static TweenCancelBehaviour TCB => TweenCancelBehaviour.KillAndCancelAwait;

        // ---------------------------- PublicMethod
        /// <summary>
        /// 待機
        /// </summary>
        /// <param name="time">待機時間</param>
        /// <param name="ct">キャンセルトークン</param>
        /// <returns>待機処理</returns>
        public static async UniTask DelayTime(float time, CancellationToken ct)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(time), true, cancellationToken: ct);
        }

        /// <summary>
        /// キャンセル処理
        /// </summary>
        /// <param name="cts">キャンセルトークンソース</param>
        public static void Cancel(ref CancellationTokenSource cts)
        {
            if (cts.IsCancellationRequested)
            {
                Debug.Log("キャンセル済み");
                return;
            }
            cts.Cancel();
            cts.Dispose();
            Debug.Log("キャンセル終了");
        }

        // ---------------------------- PrivateMethod





    }

}

