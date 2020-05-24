using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.Home
{
    [Database("UserInfo")]
    public class UserModel: PropertyChangedModel
    {
        public UserModel()
        { }
        [Column("GUID", 0)]
        public string GUID { get; set; }
        private string _name = "Guest";

        [Column("UserName", 1)]
        public string Name
        {
            get => _name;
            set => NotifyPropertyChanged(ref _name, value); 
        }
        [Column("Password", 2)]
        public string Pwd { get; set; }
        [Column("Authority", 3)]
        public Authority Authority { get; set; }
        [Column("Remark", 4)]
        public string Remark { get; set; }

    }
}
