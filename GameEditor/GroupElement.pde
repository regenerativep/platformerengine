class GroupElement extends UIElement
{
  public ArrayList<UIElement> elements;
  public PGraphics graphics;
  public GroupElement(Vector2 pos, Vector2 sze, int layer)
  {
    super(pos, sze, layer);
    elements = new ArrayList<UIElement>();
    graphics = createGraphics(sze.x, sze.y);
  }
  public void mousePressed(Vector2 mousePos)
  {
    //Vector2 mousePos = new Vector2(mouseX - position.x, mouseY - position.y);
    for(int i = elements.size() - 1; i >= 0; i--)
    {
      UIElement currentElement = elements.get(i);
      if(pointInRectangle(mousePos, currentElement.position, currentElement.size))
      {
        currentElement.mousePressed(mousePos.subtract(currentElement.position));
        return;
      }
    }
  }
  public void mouseReleased()
  {
    Vector2 mousePos = new Vector2(mouseX - position.x, mouseY - position.y);
    for(int i = elements.size() - 1; i >= 0; i--)
    {
      UIElement currentElement = elements.get(i);
      if(pointInRectangle(mousePos, currentElement.position, currentElement.size))
      {
        currentElement.mouseReleased();
        return;
      }
    }
  }
  public void update()
  {
    for(int i = 0; i < elements.size(); i++)
    {
      UIElement currentElement = elements.get(i);
      currentElement.update();
    }
  }
  public void draw(PGraphics pg)
  {
    graphics.beginDraw();
    graphics.fill(255);
    graphics.stroke(0);
    graphics.rect(0, 0, size.x - 1, size.y - 1);
    for(int i = 0; i < elements.size(); i++)
    {
      UIElement currentElement = elements.get(i);
      currentElement.draw(graphics);
    }
    graphics.endDraw();
    pg.image(graphics, position.x, position.y);
  }
}
