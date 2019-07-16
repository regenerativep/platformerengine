using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine.UserInterface
{
    public interface IInputable
    {
        string Text { get; set; }
        char[] ValidKeys { get; }
    }
}
