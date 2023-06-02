using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    Dictionary<string, ObjectPool<GameObject>> poolDic; // 매번 풀을 만들지 않고 이름으로 찾아 쓰도록 Dictionary를 사용함

    private void Awake()
    {
        poolDic = new Dictionary<string, ObjectPool<GameObject>>();
    }

    public T Get<T>(T original, Vector3 position, Quaternion rotation) where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            if (!poolDic.ContainsKey(prefab.name))              // 한번도 만들어진적없는 오브젝트는 새롭게 만드는 작업
            {
                CreatePool(prefab.name, prefab);        // key값과 prefab의 이름 그대로 쓰는게 코드와 혼선이 생긱지않는다
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
        if (!poolDic.ContainsKey(prefab.name))              // 한번도 만들어진적없는 오브젝트는 새롭게 만드는 작업
        {
            CreatePool(prefab.name, prefab);        // key값과 prefab의 이름 그대로 쓰는게 코드와 혼선이 생긱지않는다
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
        return Get<T>(original, Vector3.zero, Quaternion.identity);      // 오버로딩의 경우 현재 형태가 제일 최적화된 형태임
    }


    public bool Release(GameObject go)
    {
        if (!poolDic.ContainsKey(go.name))                         // Dic에 key값이 없다면
        {
            return false;
        }
        ObjectPool<GameObject> pool = poolDic[go.name];
        pool.Release(go);
        return true;
    }

    private void CreatePool(string key, GameObject prefab)
    {
        //  유니티는 기본적으로 4가지 경우의 함수를 요구한다
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
        createFunc: () =>               // 만들어질때 작업
        {
            GameObject go = Instantiate(prefab);
            go.name = key;  // 만들당시 복수의 오브젝트가 생성될경우 클론때문에 이름에 (숫자)가 불어서 인식을 못해 반납이 안될경우를 대비해서 모든 생성 오브젝트를 같은 이름을 생성케 한다
            return go;
        },
        actionOnGet: (GameObject go) => // 가저올때 작업
        {
            go.SetActive(true);
            go.transform.SetParent(null);
        },
        actionOnRelease: (GameObject go) =>     // 반납할때 작업
        {
            go.SetActive(false);
            go.transform.SetParent(transform);
        },
        actionOnDestroy: (GameObject go) =>     // 삭제할때 작업
        {
            Destroy(go);
        }
        );
        poolDic.Add(key, pool);
    }
}
