using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.Framework.Controller
{
    public class Controller:IController
    {
        public Controller(PropertyChangedModel model)
        {
            Model = model;
        }

        public PropertyChangedModel Model { get; }

    }
}
