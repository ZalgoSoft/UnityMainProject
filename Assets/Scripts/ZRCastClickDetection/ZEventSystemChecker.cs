using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZEventSystemChecker : MonoBehaviour
{
    //public GameObject eventSystem;
	void Awake ()
	{
	    if(!FindObjectOfType<EventSystem>())
        {
           //Instantiate(eventSystem);
            GameObject obj = new GameObject("EventSystem");
            obj.AddComponent<EventSystem>();
            obj.AddComponent<StandaloneInputModule>().forceModuleActive = true;
        }
	}
}