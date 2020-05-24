using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.Framework.Controller
{
    public interface IController
    {

       PropertyChangedModel Model { get; }
    }
}
