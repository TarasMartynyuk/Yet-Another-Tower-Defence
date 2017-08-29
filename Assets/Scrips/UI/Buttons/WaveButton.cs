using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveButton : Button {

	private Wave wave;

	public Wave Wave{
		get{return wave;}
		set{wave = value;}
	}
}
