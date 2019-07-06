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
TextInputElement addLayerTextInput, selectedTextInputElement;
WorldItemType currentWorldItemType;

void setup()
{
  size(1024, 768);
  scrollMultiplier = -4;
  mainGraphics = createGraphics(width, height);
  mouseIsPressed = false;
  showGrid = true;
  roomSize = new Vector2(512, 512);
  snap = new Vector2(32, 32);
  currentLayer = null;
  selectedTextInputElement = null;
  currentWorldItemType = null;
  uiElements = new ArrayList<UIElement>();
  worldLayers = new ArrayList<WorldLayer>();
  objectTypeList = new ArrayList<WorldItemType>();
  tileTypeList = new ArrayList<WorldItemType>();
  
  uiElements.add(new LevelElement());
  
  objectListElement = new ListElement(new Vector2(0, 256), new Vector2(128, 256), 10);
  objectListElement.addElement(new ButtonElement("-none-", new Vector2(0, 0), new Vector2(80, 28), 9)
  {
    public void mousePressed()
    {
      currentWorldItemType = null;
    }
  });
  uiElements.add(objectListElement);
  tileListElement = new ListElement(new Vector2(0, 512), new Vector2(128, 256), 10);
  tileListElement.addElement(new ButtonElement("-none-", new Vector2(0, 0), new Vector2(80, 28), 9)
  {
    public void mousePressed()
    {
      currentWorldItemType = null;
    }
  });
  uiElements.add(tileListElement);
  layerListElement = new ListElement(new Vector2(0, 0), new Vector2(128, 256), 10);
  addLayerTextInput = new TextInputElement(new Vector2(64, 0), new Vector2(64, 32), 9);
  layerListElement.addElement(new UIElement[] {
    new ButtonElement("add layer", new Vector2(0, 0), new Vector2(64, 32), 9)
    {
      public void mousePressed()
      {
        int layer = parseInt(addLayerTextInput.text.text);
        WorldLayer worldLayer = new WorldLayer(layer);
        addWorldLayer(worldLayer);
        addLayerTextInput.text.text = "";
      }
    },
    addLayerTextInput
  }, new Vector2(0, 32));
  uiElements.add(layerListElement);
  
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
      currentElement.mousePressed();
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
      public void mousePressed()
      {
        currentLayer = getWorldLayer(actualLayer);
      }
    },
    new CheckboxElement(new Vector2(96, 0), new Vector2(24, 24), 9, true)
    {
      public void onTick()
      {
        getWorldLayer(actualLayer).display = true;
      }
      public void onUntick()
      {
        getWorldLayer(actualLayer).display = false;
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
    public void mousePressed()
    {
      currentWorldItemType = objectTypeList.get(itemId);
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
    public void mousePressed()
    {
      currentWorldItemType = tileTypeList.get(itemId);
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
