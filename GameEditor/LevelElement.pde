class LevelElement extends UIElement
{
  public Vector2 viewOffset;
  public WorldItem currentWorldItem;
  public LevelElement()
  {
    super(new Vector2(0, 0), new Vector2(width, height), 10);
    viewOffset = new Vector2(160, 64);
    currentWorldItem = null;
  }
  public void draw(PGraphics pg)
  {
    if(currentWorldItemType == null && currentWorldItem != null)
    {
      currentWorldItem = null;
    }
    else if((currentWorldItemType != null && currentWorldItem == null) || (currentWorldItemType != null && currentWorldItem.type != currentWorldItemType))
    {
      currentWorldItem = new WorldItem(currentWorldItemType, snapPosition(getMousePos()));
    }
    else if(currentWorldItem != null)
    {
      currentWorldItem.position = snapPosition(getMousePos());
    }
    pg.pushMatrix();
    pg.translate(viewOffset.x, viewOffset.y);
    if(showGrid)
    {
      pg.stroke(0, 0, 255);
      pg.noFill();
      pg.rect(0, 0, roomSize.x, roomSize.y);
      pg.stroke(0);
      for(int i = 0; i < roomSize.x; i += snap.x)
      {
        pg.line(i, 0, i, roomSize.y);
      }
      for(int i = 0; i < roomSize.y; i += snap.y)
      {
        pg.line(0, i, roomSize.x, i);
      }
    }
    for(int i = worldLayers.size() - 1; i >= 0; i--)
    {
      WorldLayer worldLayer = worldLayers.get(i);
      if(worldLayer.display)
      {
        for(WorldItem item : worldLayer.worldItems)
        {
          item.draw(pg);
        }
      }
    }
    if(currentWorldItem != null)
    {
      currentWorldItem.draw(pg, 204);
    }
    pg.popMatrix();
  }
  public Vector2 getMousePos()
  {
    return (new Vector2(mouseX, mouseY)).subtract(viewOffset);
  }
  public Vector2 snapPosition(Vector2 pos)
  {
    Vector2 newVec = new Vector2((pos.x / snap.x) * snap.x, (pos.y / snap.y) * snap.y);
    if(pos.x < 0)
    {
      newVec.x -= snap.x;
    }
    if(pos.y < 0)
    {
      newVec.y -= snap.y;
    }
    return newVec;
  }
  public void update()
  {
    if(mousePressed && mouseButton == CENTER)
    {
      viewOffset.x += mouseX - pmouseX;
      viewOffset.y += mouseY - pmouseY;
    }
  }
  public void mousePressed(Vector2 mousePos)
  {
    if(mouseButton == LEFT)
    {
      if(currentWorldItem != null && currentLayer != null)
      {
        Vector2 placePos = snapPosition(getMousePos());
        currentLayer.worldItems.add(new WorldItem(currentWorldItemType, placePos));
      }
    }
    else if(mouseButton == RIGHT)
    {
      removeItemAtPosition(getMousePos());
    }
  }
}
