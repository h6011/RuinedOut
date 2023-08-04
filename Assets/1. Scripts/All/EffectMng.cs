using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    EnemyDeadBody,
    Block1,
}

public class EffectMng : MonoBehaviour
{
    public static EffectMng Instance;

    private void Awake()
    {
        Instance = this;
    }

    IEnumerator LiveTimeAction(GameObject obj, string poolingObjname, float Time_)
    {
        yield return new WaitForSeconds(Time_);
        PoolingMng.Instance.RemoveObj(obj, poolingObjname);
    }

    public void MakeEffect1(EffectType effectType, Vector3 position, int amount, float liveTime, float power = 5f)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newobj = PoolingMng.Instance.CreateObj(effectType.ToString(), transform);
            Rigidbody rb = newobj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity += new Vector3(Random.Range(-power, power), Random.Range(-power, power), Random.Range(-power, power));
            }
            //newobj.layer = LayerMask.
            StartCoroutine(LiveTimeAction(newobj, effectType.ToString(), liveTime));
        }
    }







}
