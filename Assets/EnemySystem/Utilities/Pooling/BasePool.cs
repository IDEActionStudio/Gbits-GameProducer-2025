using UnityEngine;
using System.Collections.Generic;

namespace EnemySystem.Utilities.Pooling
{
    public abstract class BasePool<T> : MonoBehaviour where T : Component
    {
        [Header("Base Settings")]
        [SerializeField] protected T prefab;
        [SerializeField] protected Transform poolRoot;

        protected Queue<T> _inactive = new();
        protected List<T> _active = new();

        public Transform PoolRoot => poolRoot;

        protected void WarmPool(int size)
        {
            for (int i = 0; i < size; i++)
            {
                Return(CreateInstance());
            }
        }

        public T Get()
        {
            var instance = _inactive.Count > 0 
                ? _inactive.Dequeue() 
                : CreateInstance();

            _active.Add(instance);
            OnGet(instance);
            return instance;
        }

        public void Return(T instance)
        {
            if (_active.Remove(instance))
            {
                _inactive.Enqueue(instance);
                OnReturn(instance);
            }
        }

        protected virtual T CreateInstance()
        {
            var obj = Instantiate(prefab, poolRoot);
            obj.gameObject.SetActive(false);
            return obj;
        }

        protected abstract void OnGet(T instance);
        protected abstract void OnReturn(T instance);
    }
}