﻿using UnityEngine;
using System.Collections;

public class DestroySelfEvent : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void DestroySelf()
	{
		Destroy( this.gameObject );
	}

	void EnableMoney() {
        Object.FindObjectOfType<GameScript>().EnableMoneyBag();
	}
}
