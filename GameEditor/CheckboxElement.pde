class CheckboxElement extends UIElement
{
  public boolean ticked;
  public CheckboxElement(Vector2 pos, Vector2 sze, int layer, boolean ticked)
  {
    super(pos, sze, layer);
    this.ticked = ticked;
  }
  public void mousePressed(Vector2 mousePos)
  {
    ticked = !ticked;
    if(ticked)
    {
      onTick();
    }
    else
    {
      onUntick();
    }
  }
  public void draw(PGraphics pg)
  {
    if(ticked)
    {
      backgroundColor = color(0);
    }
    else
    {
      backgroundColor = color(255);
    }
    super.draw(pg);
  }
  public void onTick() {}
  public void onUntick() {}
}
