class WorldItem
{
  public Vector2 position;
  public WorldItemType type;
  public WorldItem(WorldItemType type, Vector2 position)
  {
    this.type = type;
    this.position = position;
  }
  public void draw()
  {
    this.draw(255);
  }
  public void draw(int alpha)
  {
    fill(255, alpha);
    stroke(0);
    rect(position, type.size);
    fill(0);
    noStroke();
    textAlign(LEFT, TOP);
    text(type.name, position);
  }
}
class WorldItemType
{
  public String name;
  public Vector2 size;
  public WorldItemType(String name, Vector2 size)
  {
    this.name = name;
    this.size = size;
  }
}
