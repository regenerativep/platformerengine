abstract class UIElement
{
  public Vector2 position, size;
  public int layer;
  public color outlineColor, backgroundColor;
  public UIElement(Vector2 pos, Vector2 sze, int layer)
  {
    position = pos;
    size = sze;
    this.layer = layer;
    outlineColor = color(0);
    backgroundColor = color(255);
  }
  public void mousePressed(Vector2 mousePos) {}
  public void mouseReleased() {}
  public void update() {}
  public void draw(PGraphics pg)
  {
    pg.fill(backgroundColor);
    pg.stroke(outlineColor);
    pg.rect(position.x, position.y, size.x - 1, size.y - 1);
  }
  public void scroll(float amount) {}
}
