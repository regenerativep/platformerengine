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
  public abstract void mousePressed();
  public abstract void mouseReleased();
  public abstract void update();
  public abstract void draw();
}
