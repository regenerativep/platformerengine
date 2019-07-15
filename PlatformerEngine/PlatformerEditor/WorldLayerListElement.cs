using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEditor
{
    public class WorldLayerListElement : ListElement
    {
        private List<GroupElement> layerButtonGroups;
        public WorldLayerListElement(PlatformerEditor game, Vector2 position, Vector2 size, float layer, string name) : base(game, position, size, layer, name)
        {
            GroupElement group = new GroupElement(Game, new Vector2(0, 0), new Vector2(128, 32), Layer, Name + "_add");
            ButtonElement addLayerButton = new ButtonElement(Game, new Vector2(0, 0), new Vector2(64, 32), Layer + 0.01f, group.Name + "_button", "add layer");
            TextInputElement addLayerInput = new TextInputElement(Game, new Vector2(64, 0), new Vector2(64, 32), Layer + 0.01f, group.Name + "_input");
            addLayerInput.ValidKeys = "0123456789".ToCharArray();
            addLayerButton.Click = () =>
            {
                if (addLayerInput.Text.Length > 0)
                {
                    Game.AddWorldLayer(int.Parse(addLayerInput.Text));
                    addLayerInput.Text = "";
                }
            };
            group.Elements.Add(addLayerButton);
            group.Elements.Add(addLayerInput);
            AddItem(group);
            layerButtonGroups = new List<GroupElement>();
        }
        public void AddLayer(int layer)
        {
            if (Game.GetWorldLayer(layer) != null) return;
            GroupElement group = new GroupElement(Game, new Vector2(0, 0), new Vector2(128, 32), Layer + 0.01f, Name + "_" + layer.ToString());
            ButtonElement layerButton = new ButtonElement(Game, new Vector2(0, 0), new Vector2(96, 32), group.Layer + 0.01f, group.Name + "_button", "layer " + layer.ToString());
            layerButton.Click = () =>
            {
                WorldLayer worldLayer = Game.GetWorldLayer(layer);
                Game.CurrentWorldLayer = worldLayer;
            };
            CheckboxElement layerDisplayCheckbox = new CheckboxElement(Game, new Vector2(layerButton.Size.X, 0), new Vector2(32, 32), group.Layer + 0.01f, group.Name + "_checkbox", true);
            layerDisplayCheckbox.Tick = (ticked) =>
            {
                WorldLayer worldLayer = Game.GetWorldLayer(layer);
                if (worldLayer != null)
                {
                    worldLayer.IsVisible = ticked;
                }
            };
            group.Elements.Add(layerButton);
            group.Elements.Add(layerDisplayCheckbox);
            AddItem(group);
            layerButtonGroups.Add(group);
        }
        public void ClearLayers()
        {
            for(int i = layerButtonGroups.Count - 1; i >= 0; i--)
            {
                GroupElement element = layerButtonGroups[i];
                RemoveItem(element);
                Game.RemoveUIElement(element);
                element.RemoveAllChildren(true);
                layerButtonGroups.RemoveAt(i);
            }
        }
    }
}
