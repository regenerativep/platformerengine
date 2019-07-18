using PlatformerEngine.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PlatformerEditor
{
    public class LevelTab : HardGroupElement
    {
        public LevelTab(UIManager uiManager, Vector2 position, Vector2 size, float layer, string name) : base(uiManager, position, size, layer, name)
        {
            PlatformerEditor actualGame = (PlatformerEditor)UIManager.Game;
            Elements.Add(new LevelElement(UIManager, new Vector2(0, 0), new Vector2(Size.X, Size.Y), 0.3f, "level"));
            actualGame.WorldLayerListElement = new WorldLayerListElement(UIManager, new Vector2(0, 0), new Vector2(128, 256), 0.4f, "list_layers");
            Elements.Add(actualGame.WorldLayerListElement);
            actualGame.ObjectListElement = new WorldItemListElement(UIManager, new Vector2(0, 0), new Vector2(128, 240), 0.4f, "list_objects");
            actualGame.TileListElement = new WorldItemListElement(UIManager, new Vector2(0, 0), new Vector2(128, 240), 0.4f, "list_tiles");
            TabbedElement worldItemTabs = new TabbedElement(UIManager, new Vector2(0, 256), new Vector2(128, 256), 0.4f, "tabs_worlditems", 16);
            worldItemTabs.AddTab("objects", actualGame.ObjectListElement, 64);
            worldItemTabs.AddTab("tiles", actualGame.TileListElement, 64);
            Elements.Add(worldItemTabs);
            TextInputElement filenameInputElement = new TextInputElement(UIManager, new Vector2(0, 512), new Vector2(128, 24), 0.4f, "input_filename");
            ButtonElement loadButton = new ButtonElement(UIManager, new Vector2(0, 536), new Vector2(48, 24), 0.4f, "button_load", "load");
            loadButton.Click = () =>
            {
                string filename = filenameInputElement.Text;
                actualGame.LoadLevel(filename);
            };
            ButtonElement saveButton = new ButtonElement(UIManager, new Vector2(48, 536), new Vector2(48, 24), 0.4f, "button_save", "save");
            saveButton.Click = () =>
            {
                string filename = filenameInputElement.Text;
                actualGame.SaveLevel(filename);
            };
            Elements.Add(filenameInputElement);
            Elements.Add(loadButton);
            Elements.Add(saveButton);
            TextInputElement snapXInput = new TextInputElement(UIManager, new Vector2(0, 560), new Vector2(56, 24), 0.4f, "input_snap_x");
            TextInputElement snapYInput = new TextInputElement(UIManager, new Vector2(56, 560), new Vector2(56, 24), 0.4f, "input_snap_y");
            ButtonElement setSnapButton = new ButtonElement(UIManager, new Vector2(0, 584), new Vector2(56, 20), 0.4f, "button_snap_set", "set snap");
            setSnapButton.Click = () =>
            {
                LevelElement levelElement = (LevelElement)UIManager.GetUIElement("level");
                levelElement.Snap = new Vector2(int.Parse(snapXInput.Text), int.Parse(snapYInput.Text));
            };
            Elements.Add(snapXInput);
            Elements.Add(snapYInput);
            Elements.Add(setSnapButton);
        }
    }
}
