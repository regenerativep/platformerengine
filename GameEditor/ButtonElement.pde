class ButtonElement extends UIElement
{
  public Vector2 textPadding = new Vector2(2, 2);
  public TextElement text;
  public ButtonElement(String txt, Vector2 position, Vector2 sze, int layer)
  {
    super(position, sze, layer);
    text = new TextElement(txt, color(0, 0, 0), 12, position.add(textPadding), layer);
  }
  public void draw(PGraphics pg)
  {
    text.position = position.add(textPadding);
    pg.fill(255);
    pg.stroke(0);
    pg.rect(position.x, position.y, size.x, size.y);
    text.draw(pg);
  }
}
