﻿using UnityEngine;
using System.Collections;

public class Tab2_Page5_script : SubPageHandler {

	public GameObject deckObj = null;
	public UIScrollView scrollPanel = null;
	private int totalCol = 5;
	private Vector2 deckDimension;
	
	// Use this for initialization
	void Start () {
		if(deckObj == null) return;
		
		deckDimension = deckObj.GetComponent<UISprite>().localSize;
		SpawnLocalUserInventory();
		scrollPanel.ResetPosition();
		
		//base.StartSubPage();
	}
	
	private void SpawnLocalUserInventory()
	{
		Transform parent = this.transform.Find("ScrollView/Inventory List Holder");
		int currentRow = 0;
		int currentCol = 0;
		
		for(int i = 0; i<GlobalManager.UICard.localUserCardInventory.Count; i++)
		{
			Vector3 pos = Vector3.zero;
			GameObject holder = Instantiate(deckObj, Vector3.zero, Quaternion.identity) as GameObject;
			holder.name = "Inventory_" + (i+1);
			holder.transform.parent = parent;
			
			if(i % totalCol == 0 && i != 0){ currentRow++; }
			pos.x = ((currentCol * deckDimension.x) + (deckDimension.x / 2)) + (currentCol * 25f);
			pos.y = (((currentRow * deckDimension.y) + (deckDimension.y / 2)) + (currentRow * 40f)) * -1;
			
			holder.transform.localPosition = pos;
			holder.transform.localScale = holder.transform.lossyScale;
			holder.AddComponent<UIDragScrollView>();
			UIEventListener.Get(holder).onClick += ButtonHandler;

			CharacterCard tempCardObj = GlobalManager.UICard.localUserCardInventory[i];
			holder.GetComponent<UICardScript>().Card = tempCardObj;
			holder.GetComponent<UICardScript>().inventoryIndex = i;
			
			currentCol++;
			if(currentCol >= totalCol)
			{
				currentCol = 0;
			}
		}
	}
	
	private void ButtonHandler(GameObject go)
	{
		// chosen card action
		parent.currentSelectedDeckNum = int.Parse(go.name.Split(new char[]{'_'})[1]) - 1;
		parent.enhanceBaseCard = parent.currentSelectedCard = GlobalManager.UICard.localUserCardInventory[go.GetComponent<UICardScript>().inventoryIndex];

		// show popup
		base.OpenPopup(true);
	}
}
