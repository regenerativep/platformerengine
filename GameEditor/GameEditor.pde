ArrayList<UIElement> uiElements;
ArrayList<WorldLayer> worldLayers;
boolean showGrid;
boolean mouseIsPressed;
PGraphics mainGraphics;
Vector2 roomSize;
Vector2 snap;
float scrollMultiplier;
ListElement layerListElement;
WorldLayer currentLayer;
TextInputElement addLayerTextInput;
TextInputElement selectedTextInputElement;

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
  uiElements = new ArrayList<UIElement>();
  worldLayers = new ArrayList<WorldLayer>();
  
  uiElements.add(new LevelElement());
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
  layerListElement.addElement(new ButtonElement("layer " + worldLayer.layer, new Vector2(0, 0), new Vector2(96, 24), 9)
  {
    public void mousePressed()
    {
      currentLayer = getWorldLayer(actualLayer);
    }
  });
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
