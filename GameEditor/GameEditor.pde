ArrayList<UIElement> uiElements;
ArrayList<WorldLayer> worldLayers;
boolean showGrid;
boolean mouseIsPressed;
Vector2 roomSize;
Vector2 snap;

void setup()
{
  size(1024, 768);
  mouseIsPressed = false;
  showGrid = true;
  roomSize = new Vector2(512, 512);
  snap = new Vector2(32, 32);
  uiElements = new ArrayList<UIElement>();
  worldLayers = new ArrayList<WorldLayer>();
  
  uiElements.add(new LevelElement());
}
void draw()
{
  background(255);
  for(int i = 0; i < uiElements.size(); i++)
  {
    UIElement currentElement = uiElements.get(i);
    currentElement.update();
  }
  for(int i = 0; i < uiElements.size(); i++)
  {
    UIElement currentElement = uiElements.get(i);
    currentElement.draw();
  }
}
void mousePressed()
{
  if(mouseIsPressed) return;
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
void addWorldLayer(WorldLayer worldLayer)
{
  for(int i = 0; i < worldLayers.size(); i++)
  {
    WorldLayer currentWorldLayer = worldLayers.get(i);
    if(worldLayer.layer >= currentWorldLayer.layer)
    {
      worldLayers.add(i, worldLayer);
      return;
    }
  }
  worldLayers.add(worldLayer);
}
