class Vector2
{
  public int x, y;
  public Vector2(int x, int y)
  {
    this.x = x;
    this.y = y;
  }
  public Vector2 multiply(int scalar)
  {
    return new Vector2(x * scalar, y * scalar);
  }
  public Vector2 add(Vector2 other)
  {
    return new Vector2(x + other.x, y + other.y);
  }
  public Vector2 subtract(Vector2 other)
  {
    return new Vector2(x - other.x, y - other.y);
  }
}
public void translate(Vector2 amount)
{
  translate(amount.x, amount.y);
}
public void rect(Vector2 a, Vector2 b)
{
  rect(a.x, a.y, b.x, b.y);
}
public void text(String text, Vector2 pos)
{
  text(text, pos.x, pos.y);
}
public boolean pointInRectangle(Vector2 point, Vector2 rectPos, Vector2 rectSize)
{
  return point.x >= rectPos.x && point.x < rectPos.x + rectSize.x && point.y >= rectPos.y && point.y < rectPos.y + rectSize.y;
}
