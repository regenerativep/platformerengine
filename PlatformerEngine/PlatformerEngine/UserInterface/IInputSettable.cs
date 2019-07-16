using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine.UserInterface
{
    public interface IInputSettable
    {
        IInputable CurrentInput { get; set; }
    }
}
