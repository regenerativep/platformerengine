class WorldLayer
{
  public ArrayList<WorldItem> worldItems;
  public int layer;
  public WorldLayer(int layer)
  {
    this.layer = layer;
    worldItems = new ArrayList<WorldItem>();
  }
}
