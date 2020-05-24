using AutomaticTestingSystem.Framework.Model;
using AutomaticTestingSystem.Framework.Common;

namespace AutomaticTestingSystem.UserControls.InputDialog
{
    class InputDialogViewModel
    {

        [Property("Input Dialog Model", PropertyType = Property.Model)]
        public InputDialogModel InputModel { get; set; }
        [Property("Input Dialog Controller", PropertyType = Property.Controller)]
        public InputDialogController InputController { get; set; }


        public InputDialogViewModel(PropertyChangedModel model=null)
        {
            InputModel = (InputDialogModel)model ?? new InputDialogModel();

            if (InputController == null)
                InputController = new InputDialogController(model);
       
        }





    }
}
