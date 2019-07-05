class TextElement extends UIElement
{
  public int halign, valign;
  public int textSize;
  public color textColor;
  public String text;
  public TextElement(String text, color col, int textSize, Vector2 position, int layer)
  {
    super(position, new Vector2(0, 0), layer);
    halign = LEFT;
    valign = TOP;
    this.textSize = textSize;
    textColor = col;
    this.text = text;
  }
  public void draw(PGraphics pg)
  {
    pg.textAlign(halign, valign);
    pg.fill(textColor);
    pg.noStroke();
    pg.text(text, position.x, position.y);
  }
}
