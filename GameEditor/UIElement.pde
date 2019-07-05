abstract class UIElement
{
  public Vector2 position, size;
  public int layer;
  public UIElement(Vector2 pos, Vector2 sze, int layer)
  {
    position = pos;
    size = sze;
    this.layer = layer;
  }
  public void mousePressed() {}
  public void mouseReleased() {}
  public void update() {}
  public void draw(PGraphics pg) {}
  public void scroll(float amount) {}
}
