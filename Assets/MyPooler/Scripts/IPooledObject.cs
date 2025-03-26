using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPooler
{
	public interface IPooledObject
	{
		void OnRequestedFromPool();
		void DiscardToPool();
	}
}



