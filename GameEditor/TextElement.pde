class TextElement extends UIElement
{
  public int halign, valign, textSize;
  public color textColor;
  public String text;
  public boolean showRectangle;
  public TextElement(String text, color col, int textSize, Vector2 position, int layer)
  {
    super(position, new Vector2(0, 0), layer);
    halign = LEFT;
    valign = TOP;
    this.textSize = textSize;
    textColor = col;
    this.text = text;
    showRectangle = true;
  }
  public void draw(PGraphics pg)
  {
    if(showRectangle)
    {
      super.draw(pg);
    }
    pg.textAlign(halign, valign);
    pg.fill(textColor);
    pg.noStroke();
    pg.text(text, position.x, position.y);
  }
}
