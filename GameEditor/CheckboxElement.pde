class CheckboxElement extends UIElement
{
  public boolean ticked;
  public CheckboxElement(Vector2 pos, Vector2 sze, int layer, boolean ticked)
  {
    super(pos, sze, layer);
    this.ticked = ticked;
  }
  public void mousePressed()
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
      pg.fill(0);
    }
    else
    {
      pg.fill(255);
    }
    pg.stroke(0);
    pg.rect(position.x, position.y, size.x, size.y);
  }
  public void onTick() {}
  public void onUntick() {}
}
