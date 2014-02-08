using UnityEngine;
using System.Collections;
using RTS;

public class HUD : MonoBehaviour {
	public GUISkin resourceSkin, ordersSkin, selectBoxSkin, mouseCursorSkin;
	private const int ORDERS_BAR_HEIGHT = 150, RESOURCE_BAR_HEIGHT = 40;
	private const int SELECTION_NAME_HEIGHT = 15;
	private Player player;
	private CursorState activeCursorState;
	private int currentFrame = 0;
	public Texture2D activeCursor;
	public Texture2D selectCursor, moveCursor, attackCursor, harvestCursor;
	// Use this for initialization
	void Start () {
		player = transform.root.GetComponent<Player>();
		ResourceManager.StoreSelectBoxItems(selectBoxSkin);
		SetCursorState(CursorState.Select);
	}
	
	// Update is called once per frame
	void OnGUI() {
		if(player.human) {
			DrawResourceBar();
			DrawOrdersBar();
			DrawMouseCursor();
		}
	}
	private void DrawOrdersBar() {
		GUI.skin = ordersSkin;
		GUI.BeginGroup(new Rect(0,Screen.height - ORDERS_BAR_HEIGHT,Screen.width,ORDERS_BAR_HEIGHT));
		GUI.Box(new Rect(0,0,Screen.width,Screen.height),player.race);
		string selectionName = "";
		if(player.SelectedObject) {
			selectionName = player.SelectedObject.objectName;
		}
		if(!selectionName.Equals("")) {
			print ("Name: " + selectionName);
			GUI.Label(new Rect(0, ORDERS_BAR_HEIGHT / 2,Screen.width,SELECTION_NAME_HEIGHT), selectionName);
		}
		GUI.EndGroup();
	}
	private void DrawResourceBar() {
		GUI.skin = resourceSkin;
		GUI.BeginGroup(new Rect(0,0,Screen.width,RESOURCE_BAR_HEIGHT));
		GUI.Box(new Rect(0,0,Screen.width,RESOURCE_BAR_HEIGHT),"");
		GUI.EndGroup();
	}
	public bool MouseInBounds() {
		//Screen coordinates start in the lower-left corner of the screen
		//not the top-right of the screen like the drawing coordinates do
		Vector3 mousePos = Input.mousePosition;
		bool insideWidth = mousePos.x >= 0 && mousePos.x <= Screen.width;      bool insideHeight = mousePos.y >= 0 && mousePos.y <= Screen.height - RESOURCE_BAR_HEIGHT;
		return insideWidth && insideHeight;
	}
	public Rect GetPlayingArea() {
		return new Rect(0, RESOURCE_BAR_HEIGHT, Screen.width, Screen.height - RESOURCE_BAR_HEIGHT);
	}

	private void DrawMouseCursor() {
		bool mouseOverHud = !MouseInBounds() && activeCursorState != CursorState.PanRight && activeCursorState != CursorState.PanUp;
		if(mouseOverHud) {
			Screen.showCursor = true;
		} else {
			Screen.showCursor = false;
			GUI.skin = mouseCursorSkin;
			GUI.BeginGroup(new Rect(0,0,Screen.width,Screen.height));
			Rect cursorPosition = GetCursorDrawPosition();
			GUI.Label(cursorPosition, activeCursor);
			GUI.EndGroup();
		}
	}

	private Rect GetCursorDrawPosition() {
		//set base position for custom cursor image
		float leftPos = Input.mousePosition.x;
		float topPos = Screen.height - Input.mousePosition.y; //screen draw coordinates are inverted
		//adjust position base on the type of cursor being shown
		if(activeCursorState == CursorState.PanRight) leftPos = Screen.width - activeCursor.width;
		else if(activeCursorState == CursorState.PanDown) topPos = Screen.height - activeCursor.height;
		else if(activeCursorState == CursorState.Move || activeCursorState == CursorState.Select || activeCursorState == CursorState.Harvest) {
			topPos -= activeCursor.height / 2;
			leftPos -= activeCursor.width / 2;
		}
		return new Rect(leftPos, topPos, activeCursor.width, activeCursor.height);
	}

	public void SetCursorState(CursorState newState) {
		activeCursorState = newState;
		switch(newState) {
		case CursorState.Select:
			activeCursor = selectCursor;
			break;
		case CursorState.Attack:
			activeCursor = attackCursor;
			break;
		case CursorState.Harvest:
			activeCursor = harvestCursor;
			break;
		case CursorState.Move:
			activeCursor = moveCursor;
			break;
		default: break;
		}
	}
}
