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
        /// UniTaskキャンセル
        /// </summary>
        /// <param name="task">キャンセルしたいタスク</param>
        /// <returns>キャンセル処理</returns>
        public static async UniTask Canceled(UniTask task)
        {
            if (await task.SuppressCancellationThrow()) { return; }
        }




        // ---------------------------- PrivateMethod





    }

}

