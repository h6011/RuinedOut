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

    IEnumerator DisappearEffect1(GameObject obj, float Duration_, string poolingObjname)
    {
        float timer = 0.0f;

        while (timer <= 1.0f)
        {
            timer += Time.deltaTime / Duration_;

            float v1 = Mathf.Clamp((1f - timer) * 0.5f, 0.01f, 1000);//Mathf.Lerp(0.0f, 1.0f, timer);
            obj.transform.localScale = new Vector3(v1, v1, v1);

            yield return null;
        }
    }

    IEnumerator LiveTimeAction(GameObject obj, string poolingObjname, float Time_)
    {
        yield return new WaitForSeconds(Time_);
        PoolingMng.Instance.RemoveObj(obj, poolingObjname);
    }

    public void MakeEffect1(EffectType effectType, Vector3 position, int amount, float liveTime, float power)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newobj = PoolingMng.Instance.CreateObj(effectType.ToString(), transform);
            newobj.layer = LayerMask.NameToLayer("Effect");
            newobj.transform.position = position;

            MeshRenderer ren = newobj.GetComponent<MeshRenderer>();
            Material mat = ren.material;
            ren.material = Instantiate(mat);
            mat = ren.material;

            Rigidbody rb = newobj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity += new Vector3(Random.Range(-power, power), Random.Range(-power, power), Random.Range(-power, power));
            }
            if (effectType == EffectType.Block1)
            {
                StartCoroutine(DisappearEffect1(newobj, liveTime, effectType.ToString()));
                StartCoroutine(LiveTimeAction(newobj, effectType.ToString(), liveTime));
            }
            else
            {
                StartCoroutine(LiveTimeAction(newobj, effectType.ToString(), liveTime));
            }
        }
    }







}
