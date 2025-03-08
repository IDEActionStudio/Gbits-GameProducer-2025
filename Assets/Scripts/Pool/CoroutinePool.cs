using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutinePool : MonoBehaviour
{
    public static CoroutinePool Instance;

    private Dictionary<string, List<Coroutine>> pool = new Dictionary<string, List<Coroutine>>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void StopAndStartCourtine(IEnumerator enumerator,string courtineName)
    {
        Coroutine coroutine = StartCoroutine(enumerator);
        if (pool.ContainsKey(courtineName))
        {
            for (int i = 0; i < pool[courtineName].Count; i++)
            {
                if (pool[courtineName][i] != null)
                {
                    StopCoroutine(pool[courtineName][0]);
                }
            }
            pool[courtineName].Clear();
            pool[courtineName].Add(coroutine);
        }
        else 
        {
            pool.Add(courtineName, new List<Coroutine> { coroutine } );
        }
    }
}