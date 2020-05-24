using AutomaticTestingSystem.Framework.Common;

namespace AutomaticTestingSystem.UserControls.MessageDialog
{
    public class MessageDialogViewModel
    {

        [Property("Message Dialog Model", PropertyType = Property.Model)]
        public MessagDialogModel MsgModel { get; }

        public MessageDialogViewModel()
        {
            MsgModel = new MessagDialogModel();

        }

    }
}
