using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine.UserInterface
{
    public interface IElementable
    {
        Dictionary<string, UIElement> UIElements { get; set; }
        void DestroyUIElement(UIElement elem);
    }
}
