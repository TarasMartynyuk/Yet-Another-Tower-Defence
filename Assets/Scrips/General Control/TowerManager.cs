using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;



/// <summary>
/// class that manages building towers(showing build panel and 
/// using  input gathered by it to place a tower)
/// handles gold  storage and usage
/// </summary>
public class TowerManager : Singleton<TowerManager> {

	[SerializeField] private  GameObject buildPanel;
	[SerializeField] private  Canvas canvas;
	
	//UI support
	private BuildTile selectedBuildTile;
	private BuildPanel buildPanelScript;
	private Text goldLabel;
	private bool buildPanelshown;

	private List<Tower> activeTowers;
	
	private int goldAmount;
	public int GoldAmount
	{ 
		get {return goldAmount;}
		set
		{
			goldAmount = value;
			reevaluateAllObjectsReferencingGoldAmount();
		}
	}

#region Unity initialization methods
	
	protected override void Awake () 
	{
		base.Awake();
		Assert.IsNotNull(buildPanel);
		Assert.IsNotNull(canvas);
		
	}
	
	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		GameObject goldPanel = canvas.transform.Find("Gold&HealthBanner").gameObject;
		GameObject wavesPanel = canvas.transform.Find("WavesBanner").gameObject;

		//set info banners positions to the left upper corner of the screen
		// RectTransform gameMenuPanel = canvas.transform.Find("GameMenu").GetComponent<RectTransform>();
		
		
		goldLabel = goldPanel.transform.Find("GoldPanel/GoldAmount").GetComponent<Text>();
		buildPanelScript = buildPanel.GetComponent<BuildPanel>();
		activeTowers = new List<Tower>();

		SetGlobalValuesToInitialState();

	}
#endregion
	
	// Update is called once per frame
	void Update () {
		switch(GameManager.Instance.CurrentState){
			case GameState.Game :
				if(Input.GetMouseButtonDown(0)){

					if(buildPanelshown) {
						//ignore clicking inside the build panel
						if(RectTransformUtility.RectangleContainsScreenPoint(buildPanel.GetComponent<RectTransform>(), Input.mousePosition)){
						return;
						}
			 
						hideBuildPanel();
					} 
		

					// get the object beneath the cursor
					RaycastHit2D hit = Physics2D.Raycast( Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			 	
					 if(hit.collider == null) 
						return;
				
					GameObject rayObstacle =  hit.collider.gameObject;
					if(rayObstacle.tag == "buildTile" && !rayObstacle.GetComponent<BuildTile>().Occupied){
					
						//save selectedBuildPanel
						selectedBuildTile = rayObstacle.GetComponent<BuildTile>();

						//show build panel
						buildPanel.SetActive(true);
						Vector2 buildPanelPosition = Input.mousePosition;
						buildPanelPosition.x -= (buildPanel.GetComponent<RectTransform>().rect.width * canvas.scaleFactor) / 2;
						buildPanelPosition.y -= (buildPanel.GetComponent<RectTransform>().rect.height * canvas.scaleFactor) / 2;
						buildPanel.transform.position = buildPanelPosition;

						buildPanel.transform.Find("SiegeTowerAvatar").gameObject.SetActive(selectedBuildTile.IsSiegeCompatible);

						buildPanelshown = true;

					} else if(rayObstacle.tag == "tower"){
						}
			
				}

				break;
		}

	}
	
	public void hideBuildPanel(){
		buildPanel.SetActive(false);
		buildPanelshown = false;
	}

	public void buildTowerOnselectedTile(TowerButton selectedButton){

		
		GameObject builtTower = Instantiate(selectedButton.Tower.gameObject);
		Tower builtTowerScript = builtTower.GetComponent<Tower>();

		builtTowerScript.TowersBuildSite = selectedBuildTile;

		//calculate position
		Vector2 buildPosition = selectedBuildTile.transform.position;
		if(!builtTower.name.Equals("Siege Tower(Clone)")){
			buildPosition.y += GameConstants.ArcherTowerVertOffset;
		}
		builtTower.transform.position = buildPosition;

		// find out if to place tower's front or back sprite
		if(!selectedBuildTile.IsViewedFromFront){
			builtTower.GetComponent<SpriteRenderer>().sprite = builtTower.GetComponent<Tower>().ViewFromBackSprite;
		}

		
		builtTower.GetComponent<Renderer>().sortingOrder = selectedBuildTile.SortingOrder;
		//save references to tile and tower to destroy them when the game ends
		selectedBuildTile.Occupied = true;
		activeTowers.Add(builtTowerScript);

		GoldAmount -= builtTowerScript.Cost;
		SoundManager.Instance.PlayTowerBuiltSound();
	}

#region game restart methods
	
	/// <summary>
	/// destroys all built towers,
	/// sets all buildSites Occupied properties to false
	/// shrinks tower list
	/// </summary>
	public void ClearAllObjectsCreatedDuringGame(){

		foreach (Tower tower in activeTowers)
		{
			tower.DestroyTower();
		}
		activeTowers.Clear();
	}

	public void SetGlobalValuesToInitialState()
	{
		GoldAmount = GameConstants.InitialGold;
	}
#endregion

	private void reevaluateAllObjectsReferencingGoldAmount(){
		goldLabel.text = GoldAmount.ToString();
		buildPanelScript.updateTowerButtonsStatus();
	}


}
