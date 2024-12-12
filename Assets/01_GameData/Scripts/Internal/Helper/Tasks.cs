using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace Helper
{
    /// <summary>
    /// ���ʃ^�X�N�p���[�e�B���e�B�N���X
    /// </summary>
    public static class Tasks
    {
        // ---------------------------- Field


        // ---------------------------- Property
        public static TweenCancelBehaviour TCB => TweenCancelBehaviour.KillAndCancelAwait;



        // ---------------------------- UnityMessage




        // ---------------------------- PublicMethod
        /// <summary>
        /// �ҋ@
        /// </summary>
        /// <param name="time">�ҋ@����</param>
        /// <param name="ct">�L�����Z���g�[�N��</param>
        /// <returns>�ҋ@����</returns>
        public static async UniTask DelayTime(float time, CancellationToken ct)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(time), true, cancellationToken: ct);
        }

        /// <summary>
        /// UniTask�L�����Z��
        /// </summary>
        /// <param name="task">�L�����Z���������^�X�N</param>
        /// <returns>�L�����Z������</returns>
        public static async UniTask Canceled(UniTask task)
        {
            if (await task.SuppressCancellationThrow()) { return; }
        }




        // ---------------------------- PrivateMethod





    }

}

