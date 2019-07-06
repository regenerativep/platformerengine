class WorldLayer
{
  public ArrayList<WorldItem> worldItems;
  public int layer;
  public boolean display;
  public WorldLayer(int layer)
  {
    display = true;
    this.layer = layer;
    worldItems = new ArrayList<WorldItem>();
  }
}
