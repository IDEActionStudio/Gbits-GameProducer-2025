using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    /// <summary>
    /// �����
    /// </summary>
    private Dictionary<string, List<GameObject>> pool;

    /// <summary>
    /// Ԥ����
    /// </summary>
    private Dictionary<string, GameObject> prefabs;
    #region ����
    private static ObjectPool instance;
    private ObjectPool()
    {
        pool = new Dictionary<string, List<GameObject>>();
        prefabs = new Dictionary<string, GameObject>();
    }
    public static ObjectPool GetInstance()
    {
        if (instance == null)
        {
            instance = new ObjectPool();
        }
        return instance;
    }
    #endregion


    /// <summary>
    /// �Ӷ�����л�ȡ����
    /// </summary>
    /// <param name="objName"></param>
    /// <returns></returns>
    public GameObject GetObj(string objName, Vector3 position, Quaternion quaternion)
    {
        //�������
        GameObject result = null;
        //�ж��Ƿ��и����ֵĶ����
        if (pool.ContainsKey(objName))
        {
            //��������ж���
            if (pool[objName].Count > 0)
            {
                //��ȡ���
                result = pool[objName][pool[objName].Count - 1];
                //������󣬿��ǵ��ö����ڲ�����

                result.transform.position = position;
                result.transform.rotation = quaternion;
                result.SetActive(true);
                //�ӳ����Ƴ��ö���
                pool[objName].Remove(result);
                //���ؽ��
                return result;
            }
        }
        //���û�и����ֵĶ���ػ��߸����ֶ����û�ж���

        GameObject prefab = null;
        //����Ѿ����ع���Ԥ����
        if (prefabs.ContainsKey(objName))
        {
            prefab = prefabs[objName];
        }
        else     //���û�м��ع���Ԥ����
        {
            //����Ԥ����
            prefab = Resources.Load<GameObject>("Prefabs/" + objName);
            //�����ֵ�
            prefabs.Add(objName, prefab);
        }

        //����
        result = Object.Instantiate(prefab);
        result.transform.position = position;
        result.transform.rotation = quaternion;
        //������ȥ�� Clone��
        result.name = objName;
        //����
        return result;
    }
    /// <summary>
    /// �Ӷ�����л�ȡ����
    /// </summary>
    public GameObject GetObj(string objName, Transform parent)
    {
        //�������
        GameObject result = null;
        //�ж��Ƿ��и����ֵĶ����
        if (pool.ContainsKey(objName))
        {
            //��������ж���
            if (pool[objName].Count > 0)
            {
                //��ȡ���
                result = pool[objName][pool[objName].Count - 1];
                //������󣬿��ǵ��ö����ڲ�����
                result.transform.SetParent(parent);
                result.SetActive(true);
                //�ӳ����Ƴ��ö���
                pool[objName].Remove(result);
                //pool[objName].RemoveAt(0);
                //���ؽ��
                return result;
            }
        }
        //���û�и����ֵĶ���ػ��߸����ֶ����û�ж���

        GameObject prefab = null;
        //����Ѿ����ع���Ԥ����
        if (prefabs.ContainsKey(objName))
        {
            prefab = prefabs[objName];
        }
        else     //���û�м��ع���Ԥ����
        {
            //����Ԥ����
            prefab = Resources.Load<GameObject>("Prefabs/" + objName);
            //�����ֵ�
            prefabs.Add(objName, prefab);
        }

        //����
        result = Object.Instantiate(prefab,parent);
        //������ȥ�� Clone��
        result.name = objName;
        //����
        return result;
    }
    /// <summary>
    /// ���ն��󵽶����
    /// </summary>
    /// <param name="objName"></param>
    public void RecycleObj(GameObject obj)
    {
        //����Ϊ�Ǽ���
        obj.SetActive(false);
        //�ж��Ƿ��иö���Ķ����
        if (pool.ContainsKey(obj.name))
        {
            //���õ��ö����
            pool[obj.name].Add(obj);
        }
        else
        {
            //���������͵ĳ��ӣ������������
            pool.Add(obj.name, new List<GameObject>() { obj });
        }
    }
    /// <summary>
    /// ���ն��󵽶����
    /// </summary>
    public void RecycleObj(GameObject obj, float t)
    {
        //����һ���������ٵ�monobehaviour�����Э��
        CoroutinePool.Instance.StartCoroutine(IERecycleObjTimeWait(t));

        IEnumerator IERecycleObjTimeWait(float time)
        {
            yield return new WaitForSeconds(time);
            RecycleObj(obj);
        }
    }
    /// <summary>
    /// ���ڳ�������ʱ���������
    /// </summary>
    public void Clear()
    {
        pool.Clear();
    }

}