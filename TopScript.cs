using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class TopScript : MonoBehaviour
{
    [SerializeField]
    private TextAsset inkJSONAsset = null;
    
    [SerializeField]
    private Canvas canvas = null;

    [SerializeField]
    private GameObject buttonGroupPrefab;
    
    [SerializeField]
    private Text textPrefab = null;

    [SerializeField]
    private Button buttonPrefab = null;
    
    // story stuff
    public Story story;
    private string currentKnot = null;
    private bool storyWaiting = false;
    private GameObject preCanvas;
    private Character speakerCharacter;
    private bool lastLineHadChoices = false;
    private string currentStoryText = "";
    
    // game stuff
    private const float ScreenTopY = 5.0f;
    private const float ScreenBottomY = -5.0f;
    private const float ScreenRightX = 8.0f;
    private const float ScreenLeftX = -8.0f;
    private const float ScreenEdgeBuffer = 0.25f;
    private Screen currentScreen;

    // player stuff
    private const int PlayerAnimationDelay = 10;

    private Character playerCharacter;
    private GameObject playerGo;
    private List<Character> playerInventory = new List<Character>();
    private int playerFacing = 0; // 0 = left, 1 = right

    private int playerAnimationCounter = PlayerAnimationDelay;
    
    public class Character {
	private string _name;
	private Vector3 _translateVector;
	private string _sortingLayerName;
	private string _knot;
	private bool _collides;
	private int _totalFrames;
	private List<Sprite> _frames;
	private bool _isItem;
	
	public int currentFrame = 0;
	public GameObject go;
	
	public Character(
	    string name,
	    Vector3 translateVector,
	    int totalFrames = 1,
	    string sortingLayerName = "CharacterLayer",
	    string knot = null,
	    bool collides = false,
	    bool isItem = false
	) {
	    _name = name;
	    _translateVector = translateVector;
	    _sortingLayerName = sortingLayerName;
	    _knot = knot;
	    _collides = collides;
	    _totalFrames = totalFrames;
	    _frames = new List<Sprite>();
	    _isItem = isItem;
	}

	public string GetKnot() {
	    return _knot;
	}

	public string GetName() {
	    return _name;
	}

	public bool IsItem() {
	    return _isItem;
	}

	public void SetIsItem(bool isItem) {
	    _isItem = isItem;
	}

	public void UpdateTranslateVector() {
	    _translateVector = go.transform.position;
	}

	public void SetFrame(int i) {
	    SpriteRenderer goSR = go.GetComponent<SpriteRenderer>();
	    goSR.sprite = _frames[i];
	    currentFrame = i;
	}
	
	public void AddToScene() {
	    // create game object
	    go = new GameObject(_name);

	    // add an empty link to a screen
	    go.AddComponent<ScreenChild>();
	    go.GetComponent<ScreenChild>().character = this;
	    
	    // add texture
	    go.AddComponent<SpriteRenderer>();
	    SpriteRenderer goSR = go.GetComponent<SpriteRenderer>();
	    goSR.sortingLayerID = SortingLayer.NameToID(_sortingLayerName);

	    if (_frames.Count == 0) {
		for (int i = 0; i < _totalFrames; i++) {
		    _frames.Add(Resources.Load<Sprite>(_name + i.ToString()));
		}
	    }    
	    goSR.sprite = _frames[0];

	    // scale and place
	    go.transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);
	    go.transform.Translate(_translateVector);

	    // add overlap
	    go.AddComponent<BoxCollider2D>();
	    go.GetComponent<BoxCollider2D>().size = goSR.size;

	    // add collision
	    if (_collides) {
		go.AddComponent<Rigidbody2D>();
		go.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
		go.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
	    } else {
		go.GetComponent<BoxCollider2D>().isTrigger = true;
	    }
	}  	
    }

    public class Screen {
	private List<Character> _characters;
	public Screen north;
	public Screen east;
	public Screen south;
	public Screen west;
	
	public Screen(List<Character> characters) {
	    _characters = characters;
	}

	public void AddCharacter(Character addedCharacter) {
	    _characters.Add(addedCharacter);
	}

	public void RemoveCharacter(Character removedCharacter) {
	    _characters.Remove(removedCharacter);
	}

	public void DrawScreen() {
	    for (int i = 0; i < _characters.Count; i++) {
		_characters[i].AddToScene();
		_characters[i].go.GetComponent<ScreenChild>().screen = this;
	    }
	}

	public void DeleteScreen() {
	    for (int i = 0; i < _characters.Count; i++) {
		_characters[i].UpdateTranslateVector();
		GameObject.Destroy(_characters[i].go);
	    }
	}
    }

        // Start is called before the first frame update
    void Start()
    {
	// spawn player
	playerCharacter = new Character(
	    "player",
	    Vector3.up * 2.0f,
	    totalFrames: 4,
	    sortingLayerName: "PlayerLayer",
	    collides: true
	);
	playerCharacter.AddToScene();
	playerGo = playerCharacter.go;
	playerGo.AddComponent<CollideChild>();
	playerGo.transform.SetParent(this.transform);
	playerGo.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
	playerGo.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
	playerGo.GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
	
	// Screens
	Screen throneRoom = new Screen(new List<Character>(){
		new Character(
		    "throneRoom",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
		new Character(
		    "cakeKing",
		    new Vector3(4.0f, 0.5f, 0.0f),
		    knot: "king_cake"
		),
		new Character(
		    "page",
		    new Vector3(-4.3f, 3.0f, 0.0f),
		    isItem: true,
		    knot: "berries_and_cream_recipe"
		),
		new Character(
		    "cakeStatue",
		    new Vector3(4.5f, -3.0f, 0.0f),
 		    collides: true
		),
		new Character(
		    "cakeStatue",
		    new Vector3(4.5f, 3.5f, 0.0f),
		    collides: true
		),
		new Character(
		    "fullWallHorizontal",
		    new Vector3(0.0f, -5.0f, 0.0f),
		    collides: true
		),
		new Character(
		    "fullWallVertical",
		    new Vector3(8.0f, 0.0f, 0.0f),
		    collides: true
		),
		new Character(
		    "partWallVertical",
		    new Vector3(-8.0f, 5.0f, 0.0f),
		    collides: true
		),
		new Character(
		    "partWallVertical",
		    new Vector3(-8.0f, -5.0f, 0.0f),
		    collides: true
		),
		new Character(
		    "partWallHorizontal",
		    new Vector3(8.0f, 5.0f, 0.0f),
		    collides: true
		),
		new Character(
		    "partWallHorizontal",
		    new Vector3(-8.0f, 5.0f, 0.0f),
		    collides: true
		),
		    }
	);
	
	Screen kitchen = new Screen(new List<Character>(){
		new Character(
		    "kitchen",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
		new Character(
		    "cooker",
		    new Vector3(-5.0f, 0.0f, 0.0f),
		    knot: "cooker"
		),
		new Character(
		    "fullWallHorizontal",
		    new Vector3(0.0f, -5.0f, 0.0f),
		    collides: true
		),
   		new Character(
		    "partWallVertical",
		    new Vector3(8.0f, 5.0f, 0.0f),
		    collides: true
		),
		new Character(
		    "partWallVertical",
		    new Vector3(8.0f, -5.0f, 0.0f),
		    collides: true
		),
		new Character(
		    "fullWallVertical",
		    new Vector3(-8.0f, 0.0f, 0.0f),
		    collides: true
		),
		new Character(
		    "fullWallHorizontal",
		    new Vector3(0.0f, 5.0f, 0.0f),
		    collides: true
		),
		    }
	);

	currentScreen = kitchen;
	currentScreen.DrawScreen();

	Screen crossroads = new Screen(new List<Character>(){
		new Character(
		    "crossroads",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
		new Character(
		    "partWallHorizontal",
		    new Vector3(8.0f, -5.0f, 0.0f),
		    collides: true
		),
		new Character(
		    "partWallHorizontal",
		    new Vector3(-8.0f, -5.0f, 0.0f),
		    collides: true
		),
		new Character(
		    "berryBush",
		    new Vector3(-4.0f, -2.0f, 0.0f),
    		    sortingLayerName: "PropLayer"
	        ),
		new Character(
		    "berries",
		    new Vector3(-4.0f, -2.0f, 0.0f),
		    isItem: true
	        ),
		new Character(
		    "signpost",
		    new Vector3(4.0f, 2.5f, 0.0f),
		    knot: "crossroads_sign"
	        ),
		    }
		
	);

	Screen creamery = new Screen(new List<Character>(){
		new Character(
		    "creamery",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
		new Character(
		    "page",
		    new Vector3(-4.0f, 4.7f, 0.0f),
		    isItem: true,
		    knot: "potato_pie_recipe"
		),		    
		new Character(
		    "cow",
		    new Vector3(2.4f, 2.0f, 0.0f),
		    sortingLayerName: "PropLayer",
		    collides: true
		),
		new Character(
		    "cream",
		    new Vector3(4.5f, -1.0f, 0.0f),
		    isItem: true
		),
		new Character(
		    "cream",
		    new Vector3(2.5f, -2.0f, 0.0f),
		    isItem: true
		),
       		new Character(
		    "cream",
		    new Vector3(4.5f, -3.0f, 0.0f),
		    isItem: true
		),
       		new Character(
		    "cream",
		    new Vector3(2.5f, -4.0f, 0.0f),
		    isItem: true
		),
		    }
	);

	Screen field = new Screen(new List<Character>(){
		new Character(
		    "field",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
		new Character(
		    "page",
		    new Vector3(-4.0f, -2.7f, 0.0f),
		    isItem: true,
		    knot: "sweet_potato_fries_recipe"
		),
      		new Character(
		    "wheat",
		    new Vector3(1.2f, 2.8f, 0.0f),
		    isItem: true
		),
      		new Character(
		    "wheat",
		    new Vector3(3.8f, 3.1f, 0.0f),
		    isItem: true
		),
      		new Character(
		    "wheat",
		    new Vector3(6.0f, 2.8f, 0.0f),
		    isItem: true
		),
      		new Character(
		    "potato",
		    new Vector3(1.2f, -2.8f, 0.0f),
		    isItem: true
		),
      		new Character(
		    "potato",
		    new Vector3(3.8f, -3.1f, 0.0f),
		    isItem: true
		),
      		new Character(
		    "potato",
		    new Vector3(6.0f, -2.8f, 0.0f),
		    isItem: true
		),		    
		    }
	);

	Screen barn = new Screen(new List<Character>(){
		new Character(
		    "page",
		    new Vector3(4.0f, -2.7f, 0.0f),
		    isItem: true,
		    knot: "dough_recipe"
		),
		new Character(
		    "barn",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
		new Character(
		    "barnDoor",
		    new Vector3(-1.8f, 3.0f, 0.0f),
		    collides: true,
		    sortingLayerName: "PropLayer"
		),
     		new Character(
		    "wheat",
		    new Vector3(6.2f, 4.4f, 0.0f),
		    isItem: true
		),
      		new Character(
		    "wheat",
		    new Vector3(6.3f, 2.0f, 0.0f),
		    isItem: true
		),
		new Character(
		    "gentleBird",
		    new Vector3(-5.0f, -1.5f, 0.0f),
		    knot: "gentle_bird"
		),
		    }
	);

	Screen enterWoods = new Screen(new List<Character>(){
		new Character(
		    "enterWoods",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
                new Character(
		    "sugarFlies",
		    new Vector3(-4.8f, 4.1f, 0.0f),
		    isItem: true
		),
		new Character(
		    "berryBush",
		    new Vector3(-5.3f, -3.0f, 0.0f),
    		    sortingLayerName: "PropLayer"
	        ),
		new Character(
		    "berries",
		    new Vector3(-5.3f, -3.0f, 0.0f),
		    isItem: true
	        ),
		new Character(
		    "berryBush",
		    new Vector3(-2.3f, -1.0f, 0.0f),
    		    sortingLayerName: "PropLayer"
	        ),
		new Character(
		    "berries",
		    new Vector3(-2.3f, -1.0f, 0.0f),
		    isItem: true
	        ),
		new Character(
		    "page",
		    new Vector3(3.8f, -3.7f, 0.0f),
		    isItem: true,
		    knot: "sugar_recipe"
		),
		    }
	);

	Screen woods = new Screen(new List<Character>(){
		new Character(
		    "woods",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
		new Character(
		    "page",
		    new Vector3(-5.0f, 4.2f, 0.0f),
		    isItem: true,
		    knot: "cheesey_broccoli_recipe"
		),
                new Character(
		    "sugarFlies",
		    new Vector3(-3.7f, -2.1f, 0.0f),
		    isItem: true
		),
                new Character(
		    "sugarFlies",
		    new Vector3(-0.8f, 4.1f, 0.0f),
		    isItem: true
		),
                new Character(
		    "broccoli",
		    new Vector3(-2.8f, 1.1f, 0.0f),
		    isItem: true
		),
                new Character(
		    "broccoli",
		    new Vector3(0.8f, -1.8f, 0.0f),
		    isItem: true
		),
                new Character(
		    "broccoli",
		    new Vector3(3.0f, 2.0f, 0.0f),
		    isItem: true
		),
		new Character(
		    "fishingPole",
		    new Vector3(4.5f, -3.7f, 0.0f),
		    isItem: true
		),
		    }
	);
	
	Screen enterDesert = new Screen(new List<Character>(){
		new Character(
		    "enterDesert",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
		new Character(
		    "page",
		    new Vector3(-4.0f, 2.7f, 0.0f),
		    isItem: true,
		    knot: "pepper_popper_recipe"
		),
                new Character(
		    "cheese",
		    new Vector3(4.0f, -1.3f, 0.0f),
		    isItem: true
		),
		    }
	);

	Screen desertNW = new Screen(new List<Character>(){
		new Character(
		    "desertA",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
                new Character(
		    "pepper",
		    new Vector3(2.0f, 1.3f, 0.0f),
		    isItem: true
		),
		new Character(
		    "page",
		    new Vector3(-3.5f, 2.7f, 0.0f),
		    isItem: true,
		    knot: "extra_spicy_quesadilla_recipe"
		),
		    }
	);

	Screen desertN = new Screen(new List<Character>(){
		new Character(
		    "desertB",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
                new Character(
		    "guacamole",
		    new Vector3(1.5f, 4.7f, 0.0f),
		    isItem: true
		),
		    }
	);

	Screen desertNE = new Screen(new List<Character>(){
		new Character(
		    "desert",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
                new Character(
		    "pepper",
		    new Vector3(-1.0f, -3.3f, 0.0f),
		    isItem: true
		),
		    }
	);

	
	Screen desertW = new Screen(new List<Character>(){
		new Character(
		    "desertC",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
                new Character(
		    "guacamole",
		    new Vector3(3.0f, -3.7f, 0.0f),
		    isItem: true
		),

		    }
	);

	Screen desertM = new Screen(new List<Character>(){
		new Character(
		    "desert",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
                new Character(
		    "cheese",
		    new Vector3(-0.5f, 0.7f, 0.0f),
		    isItem: true
		),

		    }
	);

	Screen desertE = new Screen(new List<Character>(){
		new Character(
		    "desert",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
		new Character(
		    "tent",
		    new Vector3(4.0f, 1.0f, 0.0f),
		    sortingLayerName: "PropLayer",
		    collides: true
		),
		new Character(
		    "oil",
		    new Vector3(5.0f, -1.2f, 0.0f),
		    sortingLayerName: "PropLayer",
		    isItem: true
		),
		new Character(
		    "oil",
		    new Vector3(6.0f, -3.2f, 0.0f),
		    sortingLayerName: "PropLayer",
		    isItem: true
		),
		new Character(
		    "oil",
		    new Vector3(3.0f, -2.5f, 0.0f),
		    sortingLayerName: "PropLayer",
		    isItem: true
		),	
		    }
	);

	Screen desertSW = new Screen(new List<Character>(){
		new Character(
		    "desert",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
                new Character(
		    "cheese",
		    new Vector3(-5.0f, -3.3f, 0.0f),
		    isItem: true
		),
                new Character(
		    "cheese",
		    new Vector3(-4.1f, -2.7f, 0.0f),
		    isItem: true
		),
		    }
	);

	Screen desertS = new Screen(new List<Character>(){
		new Character(
		    "desertA",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
                new Character(
		    "pepper",
		    new Vector3(-4.0f, 4.3f, 0.0f),
		    isItem: true
		),
		    }
	);

	Screen desertSE = new Screen(new List<Character>(){
		new Character(
		    "oasis",
		    new Vector3(0.0f, 0.0f, 0.0f),
		    sortingLayerName: "BackgroundLayer"
		),
		new Character(
		    "page",
		    new Vector3(-4.0f, 2.7f, 0.0f),
		    isItem: true,
		    knot: "fish_and_chips_recipe"
		),
		new Character(
		    "wateringHole",
		    new Vector3(2.0f, -1.0f, 0.0f),
		    knot: "watering_hole",
		    sortingLayerName: "PropLayer"
		),
		new Character(
		    "palmTree",
		    new Vector3(3.2f, 2.0f, 0.0f),
		    collides: true
		),
		    }
	);


	// link rooms

	// castle rooms
	kitchen.east = throneRoom;

	throneRoom.north = crossroads;
	throneRoom.west = kitchen;

	crossroads.north = creamery;
	crossroads.east = enterDesert;
	crossroads.south = throneRoom;
	crossroads.west = enterWoods;

	// farm rooms
	creamery.south = crossroads;
	creamery.east = field;
	creamery.north = barn;

	field.west = creamery;

	barn.south = creamery;
	
	// woods rooms
	enterWoods.east = crossroads;
	enterWoods.north = woods;

	woods.south = enterWoods;
	
	// desert rooms
	enterDesert.west = crossroads;
	enterDesert.east = desertW;

	desertNW.north = desertSW;
	desertNW.east = desertN;
	desertNW.south = desertW;
	desertNW.west = desertNE;

	desertN.north = desertS;
	desertN.east = desertNE;
	desertN.south = desertM;
	desertN.west = desertNW;

	desertNE.north = desertSE;
	desertNE.east = desertNW;
	desertNE.south = desertE;
	desertNE.west = desertN;

	desertW.north = desertNW;
	desertW.east = desertM;
	desertW.south = desertSW;
	desertW.west = enterDesert;

	desertM.north = desertN;
	desertM.east = desertE;
	desertM.south = desertS;
	desertM.west = desertW;

	desertE.north = desertNE;
	desertE.east = desertW;
	desertE.south = desertSE;
	desertE.west = desertM;

       	desertSW.north = desertW;
	desertSW.east = desertS;
	desertSW.south = desertNW;
	desertSW.west = desertSE;

	desertS.north = desertM;
	desertS.east = desertSE;
	desertS.south = desertN;
	desertS.west = desertSW;

	desertSE.north = desertE;
	desertSE.east = desertSW;
	desertSE.south = desertNE;
	desertSE.west = desertS;
	
	// story
	story = new Story(inkJSONAsset.text);
	currentKnot = "introduction";
	story.ChoosePathString(currentKnot);

	// add preCanvas
	preCanvas = new GameObject();
	preCanvas.AddComponent<SpriteRenderer>();
	SpriteRenderer preCanvasSR = preCanvas.GetComponent<SpriteRenderer>();
	preCanvasSR.sortingLayerID = SortingLayer.NameToID("PreCanvasLayer");
	preCanvasSR.sprite = Resources.Load<Sprite>("precanvas");
	preCanvas.transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);

    }

       
    // Update is called once per frame
    void Update()
    {
	if (currentKnot == null) {
	    ClearCanvas();
	    MovePlayer();
	    MoveInventory();
	    MoveScreen();
	} else {
	    StoryTime();
	    MoveInventory();
	}
    }

    void StoryTime() {
	if (story.canContinue && !storyWaiting) {
	    ClearCanvas();

	    
	    if (lastLineHadChoices) {
		currentStoryText = story.Continue().Trim();
	    } else {
		currentStoryText += "\n" + story.Continue().Trim();
	    }

	    // process story tags
	    for (int i = 0; i < story.currentTags.Count; i++) {
		string tag = story.currentTags[i].Trim();
		
		// end dialogue on holding_knot
		if (tag == "holding_knot") {
		    currentKnot = null;
		    GameObject.Destroy(preCanvas);
		    return;
		}

		// take items that are taken
		if (tag.StartsWith("take_")) {
		    string itemName = tag.Substring(5);
		    for (int j = 0; j < playerInventory.Count; j++) {
			if (playerInventory[j].GetName() == itemName) {
			    // destroy item
			    GameObject.Destroy(playerInventory[j].go);
			    playerInventory.RemoveAt(j);
			    story.variablesState["inventory_" + j.ToString()] = "";
			    
			    // clean up inventory variables if needed
			    if (j < 2) {
				for (int k = j+1; k < 3; k++) {
				    if ((string) story.variablesState["inventory_" + k.ToString()] != "" && (string) story.variablesState["inventory_" + (k-1).ToString()] == "") {
					string shiftedItemName = (string) story.variablesState["inventory_" + k.ToString()];
					story.variablesState["inventory_" + k.ToString()] = "";
					story.variablesState["inventory_" + (k-1).ToString()] = shiftedItemName;
				    }
				}				    
			    }
			}
		    }
		}

		// give items that given
		if (tag.StartsWith("give_")) {
		    string itemName = tag.Substring(5);
		    Character itemCharacter = new Character(
			itemName,
			new Vector3(0.0f, 0.0f, 0.0f),
			isItem: true
		    );
		    itemCharacter.AddToScene();
		    playerInventory.Add(itemCharacter);
		    story.variablesState["inventory_" + (playerInventory.Count-1).ToString()] = itemCharacter.GetName();
		}

		// become an item
		if (tag.StartsWith("itemize")) {
		    speakerCharacter.SetIsItem(true);
		}
	    }

	    Text storyTextField = Instantiate (textPrefab) as Text;
	    storyTextField.text = currentStoryText;
	    storyTextField.transform.SetParent(canvas.transform, false);

	    GameObject buttonGroup = Instantiate (this.buttonGroupPrefab) as GameObject;
	    buttonGroup.transform.SetParent(canvas.transform, false);

	    // choices
    	    lastLineHadChoices = story.currentChoices.Count != 0;

	    if (story.currentChoices.Count == 1) {
		storyWaiting = true;
	    }

	    for (int i = 0; i < story.currentChoices.Count; i++) {
		Choice choice = story.currentChoices [i];
		
		Button choiceButton = Instantiate (buttonPrefab) as Button;
		choiceButton.transform.SetParent (buttonGroup.transform, false);

		Text choiceText = choiceButton.GetComponentInChildren<Text> ();
		choiceText.text = choice.text.Trim();

		choiceButton.onClick.AddListener (delegate {
			OnClickChoiceButton (choice);
		});
	    }
	    
	}
    }

    void OnClickChoiceButton (Choice choice) {
	this.story.ChooseChoiceIndex (choice.index);
	this.story.Continue();
	storyWaiting = false;
    }

    void ClearCanvas() {
	int childCount = canvas.transform.childCount;
	for (int i = childCount - 1; i >= 0; --i) {
	    GameObject.Destroy (canvas.transform.GetChild (i).gameObject);
	}
    }
    
    public void CollisionDetected(Collider2D collider)
    {
	// pick up item
	ScreenChild screenChild = collider.gameObject.GetComponent<ScreenChild>();

	// enter dialogue
	if (Input.GetKey("space") && currentKnot == null && screenChild.character.GetKnot() != null && screenChild.screen != null) {
	    currentKnot = screenChild.character.GetKnot();
	    currentStoryText = "";
	    speakerCharacter = screenChild.character;
	    story.ChoosePathString(currentKnot);

	    // add preCanvas
	    preCanvas = new GameObject();
	    preCanvas.AddComponent<SpriteRenderer>();
	    SpriteRenderer preCanvasSR = preCanvas.GetComponent<SpriteRenderer>();
	    preCanvasSR.sortingLayerID = SortingLayer.NameToID("PreCanvasLayer");
	    preCanvasSR.sprite = Resources.Load<Sprite>("precanvas");
    	    preCanvas.transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);

	}

	if (Input.GetKey("e") && playerInventory.Count < 3 && screenChild.character.IsItem() && screenChild.screen != null) {
	    screenChild.screen.RemoveCharacter(screenChild.character);
	    screenChild.screen = null;
	    playerInventory.Add(screenChild.character);
	    story.variablesState["inventory_" + (playerInventory.Count-1).ToString()] = screenChild.character.GetName();
	}
    }

    void MoveInventory() {
	// drop item
	if (Input.GetKeyDown("q") && playerInventory.Count != 0) {
	    int index = playerInventory.Count-1;
	    Character itemCharacter = playerInventory[index];
	    currentScreen.AddCharacter(itemCharacter);
	    itemCharacter.go.GetComponent<ScreenChild>().screen = currentScreen;
	    itemCharacter.go.transform.Translate(new Vector3(0.0f, (index+1) * -1.5f, 0.0f));
	    playerInventory.RemoveAt(playerInventory.Count-1);
	    story.variablesState["inventory_" + (playerInventory.Count).ToString()] = "";
	}

	// move carried items
	for (int i = 0; i < playerInventory.Count; i++) {
	    playerInventory[i].go.transform.SetPositionAndRotation(
		playerGo.transform.position + new Vector3(0.0f, (i+1) * 1.5f, 0.0f),
		Quaternion.identity
	    );
	}
    }
    
    void MovePlayer()
    {
	bool up = Input.GetKey("w");
	bool down = Input.GetKey("s");
	bool left = Input.GetKey("a");
	bool right = Input.GetKey("d");

	bool vertical = up ^ down;
	bool horizontal = left ^ right;
	bool diagonal = vertical && horizontal;
	
	float speedScale = diagonal ? 6.2f : 8.0f;

	if (up && playerGo.transform.position.y < ScreenTopY + ScreenEdgeBuffer) {
	    playerGo.transform.Translate(Vector3.up * Time.deltaTime * speedScale);
	}
	if (down && playerGo.transform.position.y > ScreenBottomY - ScreenEdgeBuffer) {
	    playerGo.transform.Translate(Vector3.down * Time.deltaTime * speedScale);
	}
	if (left && playerGo.transform.position.x > ScreenLeftX - ScreenEdgeBuffer) {
	    playerGo.transform.Translate(Vector3.left * Time.deltaTime * speedScale);
	    playerFacing = 0;
	}
	if (right && playerGo.transform.position.x < ScreenRightX + ScreenEdgeBuffer) {
	    playerGo.transform.Translate(Vector3.right * Time.deltaTime * speedScale);
	    playerFacing = 1;
	}

	// TODO: clean this up a lot
	if (left || right || up || down) {
	    playerAnimationCounter -= 1;
	    if (playerAnimationCounter == 0) {
		playerAnimationCounter = PlayerAnimationDelay;
		playerCharacter.currentFrame += 1;
	    }
	} else {
	    playerCharacter.currentFrame = 0;
	}
	
	if (playerFacing == 0) {
	    playerCharacter.SetFrame((playerCharacter.currentFrame) % 2);   
	} else {
	    playerCharacter.SetFrame((playerCharacter.currentFrame) % 2 + 2);
	}
    }

    void MoveScreen() {
	// go north
	if (playerGo.transform.position.y > ScreenTopY && currentScreen.north != null) {
	    playerGo.transform.Translate(new Vector3(0.0f, -9.5f, 0.0f));
	    currentScreen.DeleteScreen();
	    currentScreen = currentScreen.north;
	    currentScreen.DrawScreen();
	}
	// go south
	if (playerGo.transform.position.y < ScreenBottomY && currentScreen.south != null) {
	    playerGo.transform.Translate(new Vector3(0.0f, 9.5f, 0.0f));
	    currentScreen.DeleteScreen();
	    currentScreen = currentScreen.south;
	    currentScreen.DrawScreen();
	}
	// go east
	if (playerGo.transform.position.x > ScreenRightX && currentScreen.east != null) {
	    playerGo.transform.Translate(new Vector3(-15.5f, 0.0f, 0.0f));
	    currentScreen.DeleteScreen();
	    currentScreen = currentScreen.east;
	    currentScreen.DrawScreen();
	}
	// go east
	if (playerGo.transform.position.x < ScreenLeftX && currentScreen.west != null) {
	    playerGo.transform.Translate(new Vector3(15.5f, 0.0f, 0.0f));
	    currentScreen.DeleteScreen();
	    currentScreen = currentScreen.west;
	    currentScreen.DrawScreen();
	}
    }
}
