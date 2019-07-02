using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    public interface ICommand
    {
        int ArgumentCount { get; }
        string Name { get; }
        string CallCommand { get; }
        void Invoke(params string[] args);
    }
}
