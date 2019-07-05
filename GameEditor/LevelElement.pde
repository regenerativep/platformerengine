class LevelElement extends UIElement
{
  public Vector2 viewOffset;
  public LevelElement()
  {
    super(new Vector2(0, 0), new Vector2(width, height), 10);
    viewOffset = new Vector2(0, 0);
  }
  public void draw(PGraphics pg)
  {
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
    for(WorldLayer worldLayer : worldLayers)
    {
      for(WorldItem item : worldLayer.worldItems)
      {
        item.draw(pg);
      }
    }
    pg.popMatrix();
  }
  public void update()
  {
    if(mousePressed && mouseButton == CENTER)
    {
      viewOffset.x += mouseX - pmouseX;
      viewOffset.y += mouseY - pmouseY;
    }
  }
}
