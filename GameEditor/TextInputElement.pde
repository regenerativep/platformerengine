class TextInputElement extends ButtonElement
{
  public TextInputElement(Vector2 position, Vector2 sze, int layer)
  {
    super("", position, sze, layer);
  }
  public void mousePressed()
  {
    selectedTextInputElement = this;
  }
}
