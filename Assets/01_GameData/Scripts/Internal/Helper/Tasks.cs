using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Helper
{
    /// <summary>
    /// ���ʃ^�X�N�p���[�e�B���e�B�N���X
    /// </summary>
    public static class Tasks
    {
        /// <summary>
        /// ���N�g�g�����X�t�H�[���A�C�e��
        /// </summary>
        public struct RectItem
        {
            public RectItem(GameObject obj, RectTransform rect)
            {
                Obj = obj;
                Rect = rect;
            }

            public GameObject Obj;
            public RectTransform Rect;
        }

        /// <summary>
        /// �L�����o�X�O���[�v�A�C�e��
        /// </summary>
        public struct GroupItem
        {
            public GroupItem(GameObject obj, CanvasGroup group)
            {
                Obj = obj;
                Group = group;
            }

            public GameObject Obj;
            public CanvasGroup Group;
        }

        /// <summary>
        /// �C���[�W�A�C�e��
        /// </summary>
        public struct ImageItem
        {
            public ImageItem(GameObject obj, Image img)
            {
                Obj = obj;
                Img = img;
            }

            public GameObject Obj;
            public Image Img;
        }


        // ---------------------------- Property
        public static TweenCancelBehaviour TCB => TweenCancelBehaviour.KillAndCancelAwait;

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

