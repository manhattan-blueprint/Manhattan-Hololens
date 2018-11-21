using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HoloUI : MonoBehaviour {

	void Start () {
    int TextWidth = 45;
    GUI.Label(new Rect(Screen.width-TextWidth, 10, TextWidth, 22), "Text");
  }

	// Update is called once per frame
	void Update () {

	}
}
