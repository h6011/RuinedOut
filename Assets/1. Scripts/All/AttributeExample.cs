using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class AttributeExample : MonoBehaviour
{
	[MenuItem("TestMenu/Application Version")]
	static void MenuTest()
	{
		Debug.Log(Application.version);
	}

}