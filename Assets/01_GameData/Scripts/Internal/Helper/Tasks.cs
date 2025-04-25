using Cysharp.Threading.Tasks;
using System;
using System.Threading;
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
            public RectItem(GameObject obj) => Obj = obj;

            public GameObject Obj;
            public readonly RectTransform Rect => Obj.GetComponent<RectTransform>();
        }

        /// <summary>
        /// �L�����o�X�O���[�v�A�C�e��
        /// </summary>
        public struct GroupItem
        {
            public GroupItem(GameObject obj) => Obj = obj;

            public GameObject Obj;
            public readonly CanvasGroup Group => Obj.GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// �C���[�W�A�C�e��
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
        /// �L�����Z������
        /// </summary>
        /// <param name="cts">�L�����Z���g�[�N���\�[�X</param>
        public static void Cancel(ref CancellationTokenSource cts)
        {
            if (cts.IsCancellationRequested)
            {
                Debug.Log("�L�����Z���ς�");
                return;
            }
            cts.Cancel();
            cts.Dispose();
            Debug.Log("�L�����Z���I��");
        }

        // ---------------------------- PrivateMethod





    }

}

