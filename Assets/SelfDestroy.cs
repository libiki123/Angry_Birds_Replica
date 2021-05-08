using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    void OnFinishAnim()
	{
		Destroy(gameObject);
	}
}
