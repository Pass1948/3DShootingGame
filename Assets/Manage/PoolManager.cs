using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    Dictionary<string, ObjectPool<GameObject>> poolDic; // �Ź� Ǯ�� ������ �ʰ� �̸����� ã�� ������ Dictionary�� �����

    private void Awake()
    {
        poolDic = new Dictionary<string, ObjectPool<GameObject>>();
    }

    public T Get<T>(T original, Vector3 position, Quaternion rotation) where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            if (!poolDic.ContainsKey(prefab.name))              // �ѹ��� ������������� ������Ʈ�� ���Ӱ� ����� �۾�
            {
                CreatePool(prefab.name, prefab);        // key���� prefab�� �̸� �״�� ���°� �ڵ�� ȥ���� �������ʴ´�
            }
            ObjectPool<GameObject> pool = poolDic[prefab.name];
            GameObject go = pool.Get();
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go as T;
        } 
        else if(original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
            {
                CreatePool(key, component.gameObject);
            }

            GameObject go = poolDic[key].Get();
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }
    /*
    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDic.ContainsKey(prefab.name))              // �ѹ��� ������������� ������Ʈ�� ���Ӱ� ����� �۾�
        {
            CreatePool(prefab.name, prefab);        // key���� prefab�� �̸� �״�� ���°� �ڵ�� ȥ���� �������ʴ´�
        }
        ObjectPool<GameObject> pool = poolDic[prefab.name];
        GameObject go =  pool.Get();
        go.transform.position = position;
        go.transform.rotation = rotation;
        return go;
    }
    */

    public T Get<T>(T original) where T : Object
    {
        return Get<T>(original, Vector3.zero, Quaternion.identity);      // �����ε��� ��� ���� ���°� ���� ����ȭ�� ������
    }


    public bool Release(GameObject go)
    {
        if (!poolDic.ContainsKey(go.name))                         // Dic�� key���� ���ٸ�
        {
            return false;
        }
        ObjectPool<GameObject> pool = poolDic[go.name];
        pool.Release(go);
        return true;
    }

    private void CreatePool(string key, GameObject prefab)
    {
        //  ����Ƽ�� �⺻������ 4���� ����� �Լ��� �䱸�Ѵ�
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
        createFunc: () =>               // ��������� �۾�
        {
            GameObject go = Instantiate(prefab);
            go.name = key;  // ������ ������ ������Ʈ�� �����ɰ�� Ŭ�ж����� �̸��� (����)�� �Ҿ �ν��� ���� �ݳ��� �ȵɰ�츦 ����ؼ� ��� ���� ������Ʈ�� ���� �̸��� ������ �Ѵ�
            return go;
        },
        actionOnGet: (GameObject go) => // �����ö� �۾�
        {
            go.SetActive(true);
            go.transform.SetParent(null);
        },
        actionOnRelease: (GameObject go) =>     // �ݳ��Ҷ� �۾�
        {
            go.SetActive(false);
            go.transform.SetParent(transform);
        },
        actionOnDestroy: (GameObject go) =>     // �����Ҷ� �۾�
        {
            Destroy(go);
        }
        );
        poolDic.Add(key, pool);
    }
}
