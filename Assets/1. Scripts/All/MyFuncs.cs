using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyFunc {
    public class Checking
    {
        /// <summary>
        /// 현재 target이 현재 존재하는 아이템이 확인하는 함수
        /// </summary>
        /// <param name="target"> GameObject 아이템 타겟</param>
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
