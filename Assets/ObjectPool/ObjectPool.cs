using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ObjectPooling
{

    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] Poolable poolablePrefab;

        [SerializeField] int poolSize;
        [SerializeField] int maxSize;

        private Stack<Poolable> objectPool = new Stack<Poolable>();

        private void Awake()
        {
            CreatePool();
        }

        private void CreatePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                Poolable poolable = Instantiate(poolablePrefab);
                poolable.gameObject.SetActive(false);
                poolable.transform.SetParent(transform);
                poolable.Pool = this;
                objectPool.Push(poolable);
            }
        }

        public Poolable Get() // 대여
        {
            if (objectPool.Count > 0)
            {
                Poolable poolable = objectPool.Pop();
                poolable.gameObject.SetActive(true);
                poolable.transform.parent = null;
                return poolable;
            }

            else             // 사이즈 이상의 갯수면 새로만들어서 줌
            {
                Poolable poolable = Instantiate(poolablePrefab);
                poolable.Pool = this;
                return poolable;
            }
        }

        public void Release(Poolable poolable)
        {
            if (objectPool.Count < maxSize)                     // 최대 사이즈를 넘을경우 반납과 초과갯수는 삭제
            {
                poolable.gameObject.SetActive(false);
                poolable.transform.SetParent(transform);
                objectPool.Push(poolable);
            }
            else
            {
                Destroy(poolable.gameObject);
            }
        }
    }
}
