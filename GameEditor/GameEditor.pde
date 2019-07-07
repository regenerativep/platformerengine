ArrayList<UIElement> uiElements;
ArrayList<WorldLayer> worldLayers;
ArrayList<WorldItemType> objectTypeList;
ArrayList<WorldItemType> tileTypeList;
boolean showGrid;
boolean mouseIsPressed;
PGraphics mainGraphics;
Vector2 roomSize;
Vector2 snap;
float scrollMultiplier;
ListElement layerListElement, objectListElement, tileListElement;
WorldLayer currentLayer;
TextInputElement addLayerTextInput, saveFileTextInput, snapXTextInput, snapYTextInput;
TextInputElement selectedTextInputElement;
WorldItemType currentWorldItemType;

void setup()
{
  size(1024, 768);
  scrollMultiplier = -4;
  mainGraphics = createGraphics(width, height);
  mouseIsPressed = false;
  showGrid = true;
  selectedTextInputElement = null;
  currentWorldItemType = null;
  roomSize = new Vector2(512, 512);
  snap = new Vector2(32, 32);
  currentLayer = null;
  worldLayers = new ArrayList<WorldLayer>();
  uiElements = new ArrayList<UIElement>();
  objectTypeList = new ArrayList<WorldItemType>();
  tileTypeList = new ArrayList<WorldItemType>();
  //add level viewer
  uiElements.add(new LevelElement());
  //file text input
  saveFileTextInput = new TextInputElement(new Vector2(136, 0), new Vector2(192, 32), 9);
  uiElements.add(saveFileTextInput);
  //snap buttons
  snapXTextInput = new TextInputElement(new Vector2(432, 0), new Vector2(48, 32), 9);
  snapXTextInput.text.text = Integer.toString(snap.x);
  uiElements.add(snapXTextInput);
  snapYTextInput = new TextInputElement(new Vector2(480, 0), new Vector2(48, 32), 9);
  snapYTextInput.text.text = Integer.toString(snap.y);
  uiElements.add(snapYTextInput);
  uiElements.add(new ButtonElement("set snap", new Vector2(528, 0), new Vector2(64, 24), 9)
  {
    public void mousePressed(Vector2 mousePos)
    {
      snap.x = parseInt(snapXTextInput.text.text);
      snap.y = parseInt(snapYTextInput.text.text);
      super.mousePressed(mousePos);
    }
  });
  //add layer list
  resetLayerList();
  //add object list
  objectListElement = new ListElement(new Vector2(0, 256), new Vector2(128, 256), 10);
  objectListElement.addElement(new ButtonElement("-none-", new Vector2(0, 0), new Vector2(80, 28), 9)
  {
    public void mousePressed(Vector2 mousePos)
    {
      currentWorldItemType = null;
      super.mousePressed(mousePos);
    }
  });
  uiElements.add(objectListElement);
  //add tile list
  tileListElement = new ListElement(new Vector2(0, 512), new Vector2(128, 256), 10);
  tileListElement.addElement(new ButtonElement("-none-", new Vector2(0, 0), new Vector2(80, 28), 9)
  {
    public void mousePressed(Vector2 mousePos)
    {
      currentWorldItemType = null;
      super.mousePressed(mousePos);
    }
  });
  uiElements.add(tileListElement);
  //load button
  uiElements.add(new ButtonElement("load", new Vector2(328, 0), new Vector2(48, 24), 9)
  {
    public void mousePressed(Vector2 mousePos)
    {
      String filename = saveFileTextInput.text.text;
      loadLevel(filename);
      super.mousePressed(mousePos);
    }
  });
  //save button
  uiElements.add(new ButtonElement("save", new Vector2(376, 0), new Vector2(48, 24), 9)
  {
    public void mousePressed(Vector2 mousePos)
    {
      String filename = saveFileTextInput.text.text;
      saveLevel(filename);
      super.mousePressed(mousePos);
    }
  });
  //load object and tile types
  JSONObject allTypes = loadJSONObject("types.json");
  JSONArray objectTypes = allTypes.getJSONArray("objectTypes");
  for(int i = 0; i < objectTypes.size(); i++)
  {
    JSONObject obj = objectTypes.getJSONObject(i);
    addGameObject(JSONObjectToType(obj, false));
  }
  JSONArray tileTypes = allTypes.getJSONArray("tileTypes");
  for(int i = 0; i < tileTypes.size(); i++)
  {
    JSONObject obj = tileTypes.getJSONObject(i);
    addTileType(JSONObjectToType(obj, true));
  }
}
void removeElement(UIElement element)
{
  for(int i = uiElements.size() - 1; i >= 0; i--)
  {
    UIElement currentElement = uiElements.get(i);
    if(currentElement == element)
    {
      uiElements.remove(i);
      return;
    }
  }
}
void resetLayerList()
{
  if(layerListElement == null)
  {
    layerListElement = new ListElement(new Vector2(0, 0), new Vector2(128, 256), 9);
    uiElements.add(layerListElement);
  }
  else
  {
    layerListElement.scrollTotal = 0;
    layerListElement.nextYPosition = 0;
    layerListElement.elements = new ArrayList<UIElement>();
  }
  addLayerTextInput = new TextInputElement(new Vector2(64, 0), new Vector2(63, 32), 9);
  layerListElement.addElement(new UIElement[] {
    new ButtonElement("add layer", new Vector2(0, 0), new Vector2(64, 32), 9)
    {
      public void mousePressed(Vector2 mousePos)
      {
        String layerText = addLayerTextInput.text.text;
        int layer = parseInt(layerText);
        WorldLayer worldLayer = new WorldLayer(layer);
        addWorldLayer(worldLayer);
        addLayerTextInput.text.text = "";
        currentLayer = worldLayer;
        super.mousePressed(mousePos);
      }
    },
    addLayerTextInput
  }, new Vector2(0, 32));
}
WorldItemType JSONObjectToType(JSONObject obj, boolean isTile)
{
  String name = obj.getString("name");
  String internalName = obj.getString("internalName");
  Vector2 size = new Vector2(obj.getInt("width"), obj.getInt("height"));
  return new WorldItemType(name, internalName, size, isTile);
}
void draw()
{
  mainGraphics.beginDraw();
  mainGraphics.background(255);
  for(int i = 0; i < uiElements.size(); i++)
  {
    UIElement currentElement = uiElements.get(i);
    currentElement.update();
  }
  for(int i = 0; i < uiElements.size(); i++)
  {
    UIElement currentElement = uiElements.get(i);
    currentElement.draw(mainGraphics);
  }
  mainGraphics.endDraw();
  background(255);
  image(mainGraphics, 0, 0);
}
void mousePressed()
{
  if(mouseIsPressed) return;
  selectedTextInputElement = null;
  mouseIsPressed = true;
  Vector2 mousePos = new Vector2(mouseX, mouseY);
  for(int i = uiElements.size() - 1; i >= 0; i--)
  {
    UIElement currentElement = uiElements.get(i);
    if(pointInRectangle(mousePos, currentElement.position, currentElement.size))
    {
      currentElement.mousePressed(mousePos.subtract(currentElement.position));
      return;
    }
  }
}
void mouseReleased()
{
  mouseIsPressed = false;
  Vector2 mousePos = new Vector2(mouseX, mouseY);
  for(int i = uiElements.size() - 1; i >= 0; i--)
  {
    UIElement currentElement = uiElements.get(i);
    if(pointInRectangle(mousePos, currentElement.position, currentElement.size))
    {
      currentElement.mouseReleased();
      return;
    }
  }
}
void mouseWheel(MouseEvent ev)
{
  float amount = ev.getCount() * scrollMultiplier;
  Vector2 mousePos = new Vector2(mouseX, mouseY);
  for(int i = uiElements.size() - 1; i >= 0; i--)
  {
    UIElement currentElement = uiElements.get(i);
    if(pointInRectangle(mousePos, currentElement.position, currentElement.size))
    {
      currentElement.scroll(amount);
      return;
    }
  }
}
void keyPressed()
{
  if(selectedTextInputElement == null) return;
  if(keyCode == BACKSPACE)
  {
    if(selectedTextInputElement.text.text.length() > 0)
    {
      selectedTextInputElement.text.text = selectedTextInputElement.text.text.substring(0, selectedTextInputElement.text.text.length() - 1);
    }
  }
  else if(key != CODED)
  {
    selectedTextInputElement.text.text += key;
  }
}
void addUIElement(UIElement element)
{
  for(int i = 0; i < uiElements.size(); i++)
  {
    UIElement currentElement = uiElements.get(i);
    if(element.layer >= currentElement.layer)
    {
      uiElements.add(i, element);
      return;
    }
  }
  uiElements.add(element);
}
boolean checkLayerExists(int layer)
{
  for(int i = 0; i < worldLayers.size(); i++)
  {
    WorldLayer currentWorldLayer = worldLayers.get(i);
    if(currentWorldLayer.layer == layer)
    {
      return true;
    }
  }
  return false;
}
void addWorldLayer(WorldLayer worldLayer)
{
  if(checkLayerExists(worldLayer.layer))
  {
    println("tried to add a layer that already exists");
    return;
  }
  boolean foundLocation = false;
  for(int i = 0; i < worldLayers.size(); i++)
  {
    WorldLayer currentWorldLayer = worldLayers.get(i);
    if(worldLayer.layer >= currentWorldLayer.layer)
    {
      worldLayers.add(i, worldLayer);
      foundLocation = true;
      break;
    }
  }
  if(!foundLocation)
  {
    worldLayers.add(worldLayer);
  }
  final int actualLayer = worldLayer.layer;
  layerListElement.addElement(new UIElement[] {
    new ButtonElement("layer " + worldLayer.layer, new Vector2(0, 0), new Vector2(96, 24), 9)
    {
      public void mousePressed(Vector2 mousePos)
      {
        currentLayer = getWorldLayer(actualLayer);
      super.mousePressed(mousePos);
      }
    },
    new CheckboxElement(new Vector2(96, 0), new Vector2(24, 24), 9, true)
    {
      public void onTick()
      {
        WorldLayer layer = getWorldLayer(actualLayer);
        if(layer != null) layer.display = true;
      }
      public void onUntick()
      {
        WorldLayer layer = getWorldLayer(actualLayer);
        if(layer != null) layer.display = false;
      }
    }
  }, new Vector2(0, 24));
}
WorldLayer getWorldLayer(int layer)
{
  for(int i = 0; i < worldLayers.size(); i++)
  {
    WorldLayer worldLayer = worldLayers.get(i);
    if(worldLayer.layer == layer)
    {
      return worldLayer;
    }
  }
  return null;
}
boolean checkGameObjectExists(String internalName)
{
  for(WorldItemType type : objectTypeList)
  {
    if(type.internalName.equals(internalName))
    {
      return true;
    }
  }
  return false;
}
void addGameObject(WorldItemType item)
{
  if(checkGameObjectExists(item.internalName))
  {
    println("tried to add a game object when a game object of the same name already exists \"" + item.internalName + "\"");
    return;
  }
  final int itemId = objectTypeList.size(); //TODO: may want to change this to a string in case we want to be able to change order of types or remove types
  objectTypeList.add(item);
  objectListElement.addElement(new ButtonElement(item.name, new Vector2(0, 0), new Vector2(120, 24), 9)
  {
    public void mousePressed(Vector2 mousePos)
    {
      currentWorldItemType = objectTypeList.get(itemId);
      super.mousePressed(mousePos);
    }
  });
}
boolean checkTileTypeExists(String internalName)
{
  for(WorldItemType type : tileTypeList)
  {
    if(type.internalName.equals(internalName))
    {
      return true;
    }
  }
  return false;
}
void addTileType(WorldItemType item)
{
  if(checkTileTypeExists(item.internalName))
  {
    println("tried to add a tile type when a tile type of the same name already exists \"" + item.internalName + "\"");
    return;
  }
  final int itemId = tileTypeList.size(); //TODO: may want to change this to a string in case we want to be able to change order of types or remove types
  tileTypeList.add(item);
  tileListElement.addElement(new ButtonElement(item.name, new Vector2(0, 0), new Vector2(120, 24), 9)
  {
    public void mousePressed(Vector2 mousePos)
    {
      currentWorldItemType = tileTypeList.get(itemId);
      super.mousePressed(mousePos);
    }
  });
}
WorldItem getItemAtPosition(Vector2 pos)
{
  if(currentLayer == null) return null;
  for(WorldItem item : currentLayer.worldItems)
  {
    if(pointInRectangle(pos, item.position, item.type.size))
    {
      return item;
    }
  }
  return null;
}
void removeItemAtPosition(Vector2 pos)
{
  if(currentLayer == null) return;
  for(int i = 0; i < currentLayer.worldItems.size(); i++)
  {
    WorldItem item = currentLayer.worldItems.get(i);
    if(pointInRectangle(pos, item.position, item.type.size))
    {
      currentLayer.worldItems.remove(i);
      return;
    }
  }
}
boolean checkLayerNameClean(String text)
{
  return true;/*
  if(text.length() == 0) return false;
  for(int i = 0; i < text.length(); i++)
  {
    String c = text.substring(i, i + 1);
    if("0123456789".indexOf(c) < 0)
    {
      return false;
    }
  }
  return true;*/
}
WorldItemType getTypeFromInternalName(String internalName)
{
  for(int i = 0; i < objectTypeList.size(); i++)
  {
    WorldItemType type = objectTypeList.get(i);
    if(internalName.equals(type.internalName))
    {
      return type;
    }
  }
  for(int i = 0; i < tileTypeList.size(); i++)
  {
    WorldItemType type = tileTypeList.get(i);
    if(internalName.equals(type.internalName))
    {
      return type;
    }
  }
  return null;
}
void saveLevel(String filename)
{
  JSONObject level = new JSONObject();
  level.setInt("width", roomSize.x);
  level.setInt("height", roomSize.y);
  JSONArray layerArray = new JSONArray();
  for(int i = 0; i < worldLayers.size(); i++)
  {
    WorldLayer worldLayer = worldLayers.get(i);
    JSONObject layerObject = new JSONObject();
    layerObject.setInt("layer", worldLayer.layer);
    JSONArray objectArray = new JSONArray();
    JSONArray tileArray = new JSONArray();
    int objectArrayInd = 0, tileArrayInd = 0;
    for(int j = 0; j < worldLayer.worldItems.size(); j++)
    {
      WorldItem item = worldLayer.worldItems.get(j);
      JSONObject itemObject = new JSONObject();
      itemObject.setString("name", item.type.internalName);
      itemObject.setInt("x", item.position.x);
      itemObject.setInt("y", item.position.y);
      if(item.type.isTile)
      {
        tileArray.setJSONObject(tileArrayInd, itemObject);
        tileArrayInd++;
      }
      else
      {
        objectArray.setJSONObject(objectArrayInd, itemObject);
        objectArrayInd++;
      }
    }
    layerObject.setJSONArray("objects", objectArray);
    layerObject.setJSONArray("tiles", tileArray);
    layerArray.setJSONObject(i, layerObject);
  }
  level.setJSONArray("layers", layerArray);
  saveJSONObject(level, filename);
}
void loadLevel(String filename)
{
  JSONObject levelObject = loadJSONObject(filename);
  resetLayerList();
  worldLayers = new ArrayList<WorldLayer>();
  currentLayer = null;
  roomSize = new Vector2(levelObject.getInt("width"), levelObject.getInt("height"));
  JSONArray layerArray = levelObject.getJSONArray("layers");
  for(int i = layerArray.size() - 1; i >= 0; i--)
  {
    JSONObject layerObject = layerArray.getJSONObject(i);
    WorldLayer worldLayer = new WorldLayer(layerObject.getInt("layer"));
    JSONArray objectArray = layerObject.getJSONArray("objects");
    JSONArray tileArray = layerObject.getJSONArray("tiles");
    for(int j = 0; j < 2; j++)
    {
      int size = j == 0 ? objectArray.size() : tileArray.size();
      for(int k = 0; k < size; k++)
      {
        JSONObject itemObject = j == 0 ? objectArray.getJSONObject(k) : tileArray.getJSONObject(k);
        Vector2 position = new Vector2(itemObject.getInt("x"), itemObject.getInt("y"));
        String internalName = itemObject.getString("name");
        WorldItem item = new WorldItem(getTypeFromInternalName(internalName), position);
        worldLayer.worldItems.add(item);
      }
    }
    addWorldLayer(worldLayer);
  }
}
