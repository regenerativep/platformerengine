class ButtonElement extends UIElement
{
  public Vector2 textPadding = new Vector2(2, 2);
  public TextElement text;
  public PGraphics clickEffect;
  public float clickEffectOpacity, clickEffectOpacityChange;
  public int clickEffectCircleWidth, clickEffectWidthChange;
  public Vector2 clickEffectCirclePosition;
  public ButtonElement(String txt, Vector2 position, Vector2 sze, int layer)
  {
    super(position, sze, layer);
    text = new TextElement(txt, color(0, 0, 0), 12, position.add(textPadding), layer);
    clickEffectCircleWidth = 0;
    clickEffectWidthChange = 8;
    clickEffectOpacityChange = 0.05f;
    clickEffectCirclePosition = new Vector2(0, 0);
  }
  public void draw(PGraphics pg)
  {
    text.position = position.add(textPadding);
    pg.fill(255);
    pg.stroke(0);
    pg.rect(position.x, position.y, size.x, size.y);
    text.draw(pg);
    if(clickEffectOpacity > 0)
    {
      clickEffect = createGraphics(size.x, size.y);
      clickEffect.beginDraw();
      clickEffect.fill(0);
      clickEffect.noStroke();
      clickEffect.ellipse(clickEffectCirclePosition.x, clickEffectCirclePosition.y, clickEffectCircleWidth, clickEffectCircleWidth);
      clickEffect.endDraw();
      pg.tint(255, (int)(clickEffectOpacity * 255));
      pg.image(clickEffect, position.x, position.y);
      pg.tint(255, 255);
    }
  }
  public void mousePressed(Vector2 mousePos)
  {
    clickEffectOpacity = 1f;
    clickEffectCircleWidth = 0;
    clickEffectCirclePosition = mousePos;
  }
  public void update()
  {
    if(clickEffectOpacity > 0)
    {
      clickEffectCircleWidth += clickEffectWidthChange;
      clickEffectOpacity -= clickEffectOpacityChange;
    }
  }
}
