class LevelElement extends UIElement
{
  public Vector2 viewOffset;
  public LevelElement()
  {
    super(new Vector2(0, 0), new Vector2(width, height), 10);
    viewOffset = new Vector2(0, 0);
  }
  public void draw()
  {
    pushMatrix();
    translate(viewOffset);
    if(showGrid)
    {
      stroke(0, 0, 255);
      noFill();
      rect(0, 0, roomSize.x, roomSize.y);
      stroke(0);
      for(int i = 0; i < roomSize.x; i += snap.x)
      {
        line(i, 0, i, roomSize.y);
      }
      for(int i = 0; i < roomSize.y; i += snap.y)
      {
        line(0, i, roomSize.x, i);
      }
    }
    for(WorldLayer worldLayer : worldLayers)
    {
      for(WorldItem item : worldLayer.worldItems)
      {
        item.draw();
      }
    }
    popMatrix();
  }
  public void update()
  {
    if(mousePressed && mouseButton == CENTER)
    {
      viewOffset.x += mouseX - pmouseX;
      viewOffset.y += mouseY - pmouseY;
    }
  }
  public void mousePressed()
  {
  }
  public void mouseReleased()
  {
  }
}
