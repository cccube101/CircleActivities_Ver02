using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace Helper
{
    /// <summary>
    /// 共通タスク用ユーティリティクラス
    /// </summary>
    public static class Tasks
    {
        // ---------------------------- Field


        // ---------------------------- Property
        public static TweenCancelBehaviour TCB => TweenCancelBehaviour.KillAndCancelAwait;



        // ---------------------------- UnityMessage




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

