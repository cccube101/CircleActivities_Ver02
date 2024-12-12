using UnityEngine;

namespace Helper
{
    public static class Base
    {
        // ---------------------------- Enum
        public enum Switch
        {
            ON, OFF
        }
        public enum GameState
        {
            DEFAULT, PAUSE, GAMECLEAR
        }

        // ---------------------------- Field
        private static (Rect[], GUIStyle) _logParam = GetLogParam();

        // ---------------------------- Property
        public const float CONST_HEIGHT = 1080;
        public const float CONST_WIDTH = 1920;
        public static (Rect[] pos, GUIStyle style) LogParam => _logParam;



        // ---------------------------- PublicMethod
        /// <summary>
        /// ���O�p�����[�^�擾
        /// </summary>
        /// <returns>���O�p�p�����[�^</returns>
        private static (Rect[], GUIStyle) GetLogParam()
        {
            //  �p�����[�^����
            var pos = new Rect[30];

            //  �ʒu�ۑ�
            for (int i = 0; i < pos.Length; i++)
            {
                pos[i] = new Rect(10, 1075 - i * 30, 300, 30);
            }

            //  �o�̓X�^�C���ۑ�
            var style = new GUIStyle();
            style.normal.textColor = Color.black;
            style.fontSize = 25;


            return (pos, style);

        }
    }
}


