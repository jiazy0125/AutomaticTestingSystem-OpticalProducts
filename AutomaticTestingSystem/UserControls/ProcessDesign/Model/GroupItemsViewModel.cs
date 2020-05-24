using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{
    public class GroupItemsViewModel
    {
        public GroupItemsModel ItemModel { get; }
        public GroupItemsController ItemController { get; }

        public GroupItemsViewModel()
        {
            try
            {
                ItemModel = new GroupItemsModel();
                if (ItemController == null)
                {
                    ItemController = new GroupItemsController(ItemModel);
                }
            }
            catch { }

        }
    }
}
