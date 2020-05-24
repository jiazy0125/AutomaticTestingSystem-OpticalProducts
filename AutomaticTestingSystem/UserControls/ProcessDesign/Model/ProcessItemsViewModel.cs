using AutomaticTestingSystem.Framework.Controller;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{
    class ProcessItemsViewModel
    {
        public ProcessItemsModel PPModel { get; }
        public ProcessItemsController PopUpController { get; set; }

        public ProcessItemsViewModel()
        {
            try
            {
                PPModel = new ProcessItemsModel();
                if (PopUpController == null)
                {
                    PopUpController = new ProcessItemsController(PPModel);
                }
            }
            catch { }
        
        }


    }
}
