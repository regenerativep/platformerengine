class ListElement extends GroupElement
{
  public float scrollTotal;
  public int nextYPosition;
  public ListElement(Vector2 pos, Vector2 sze, int layer)
  {
    super(pos, sze, layer);
    scrollTotal = 0;
    nextYPosition = 0;
  }
  public void scroll(float amount)
  {
    for(UIElement element : elements)
    {
      element.position.y += amount;
    }
    scrollTotal += amount;
  }
  public void addElement(UIElement element)
  {
    addElement(element, element.size);
  }
  public void addElement(UIElement element, Vector2 elementSize)
  {
    addElement(new UIElement[] { element }, elementSize);
  }
  public void addElement(UIElement elementList[], Vector2 elementSize)
  {
    for(int j = 0; j < elementList.length; j++)
    {
      UIElement element = elementList[j];
      element.position.y = nextYPosition + (int)scrollTotal; 
      boolean foundElement = false;
      for(int i = 0; i < elements.size(); i++)
      {
        UIElement currentElement = elements.get(i);
        if(element.layer >= currentElement.layer)
        {
          elements.add(i, element);
          foundElement = true;
          break;
        }
      }
      if(!foundElement)
      {
        elements.add(element);
      }
    }
    nextYPosition += elementSize.y;
  }
}
