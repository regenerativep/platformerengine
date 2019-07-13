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
        public WorldLayerListElement(PlatformerEditor game, Vector2 position, Vector2 size, float layer, string name) : base(game, position, size, layer, name)
        {
            GroupElement group = new GroupElement(Game, new Vector2(0, 0), new Vector2(128, 32), Layer, Name + "_add");
            ButtonElement addLayerButton = new ButtonElement(Game, new Vector2(0, 0), new Vector2(64, 32), Layer, group.Name + "_button", "add layer");
            TextInputElement addLayerInput = new TextInputElement(Game, new Vector2(64, 0), new Vector2(64, 32), Layer, group.Name + "_input");
            addLayerButton.Click = () =>
            {
                if (addLayerInput.Text.Length > 0)
                {
                    AddLayer(int.Parse(addLayerInput.Text));
                }
            };
            group.Elements.Add(addLayerButton);
            group.Elements.Add(addLayerInput);
            AddItem(group);
        }
        public void AddLayer(int layer)
        {
            if (Game.GetWorldLayer(layer) != null) return;
            GroupElement group = new GroupElement(Game, new Vector2(0, 0), new Vector2(128, 32), Layer, Name + "_" + layer.ToString());
            ButtonElement layerButton = new ButtonElement(Game, new Vector2(0, 0), new Vector2(96, 32), Layer, group.Name + "_button", "layer " + layer.ToString());
            group.Elements.Add(layerButton);
            AddItem(group);
        } //TODO: be able to remove layers
    }
}
