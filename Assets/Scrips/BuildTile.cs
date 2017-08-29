using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTile : MonoBehaviour {

	 
	[SerializeField] private bool isViewedFromFront;
	[SerializeField] private bool isSiegeCompatible;
	[SerializeField] private int sortingOrder;
	
	
	private bool occupied = false;

	public bool IsViewedFromFront{
		get {return isViewedFromFront;}
	}

	public bool Occupied{
		get {return occupied;}
		set {occupied = value;}
	}

	public bool IsSiegeCompatible{
		get{return isSiegeCompatible;}
	}

	public int SortingOrder{
		get {return sortingOrder;}
	}
}
