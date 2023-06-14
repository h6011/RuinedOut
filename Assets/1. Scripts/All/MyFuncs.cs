using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyFunc {
    public class Checking
    {
        /// <summary>
        /// ���� target�� ���� �����ϴ� �������� Ȯ���ϴ� �Լ�
        /// </summary>
        /// <param name="target"> GameObject ������ Ÿ��</param>
        /// <returns></returns>
        public static bool IsItItem(GameObject target)
        {
            if (target.CompareTag("Item"))
            {
                return true;
            }
            return false;
        }

        public static bool IsItItem(Transform target)
        {
            if (target.CompareTag("Item"))
            {
                return true;
            }
            return false;
        }
    }
    public class MyUse1
    {
        public static void p(Object obj)
        {
            Debug.Log(obj);
        }
    }
}
