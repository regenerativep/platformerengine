class WorldItem
{
  public Vector2 position;
  public WorldItemType type;
  public WorldItem(WorldItemType type, Vector2 position)
  {
    this.type = type;
    this.position = position;
  }
  public void draw(PGraphics pg)
  {
    this.draw(pg, 255);
  }
  public void draw(PGraphics pg, int alpha)
  {
    pg.fill(255, alpha);
    pg.stroke(0);
    pg.rect(position.x, position.y, type.size.x, type.size.y);
    pg.fill(0);
    pg.noStroke();
    pg.textAlign(LEFT, TOP);
    pg.text(type.name, position.x, position.y);
  }
}
class WorldItemType
{
  public String name, internalName;
  public Vector2 size;
  public boolean isTile;
  public WorldItemType(String name, String internalName, Vector2 size, boolean isTile)
  {
    this.name = name;
    this.internalName = internalName;
    this.size = size;
    this.isTile = isTile;
  }
}
