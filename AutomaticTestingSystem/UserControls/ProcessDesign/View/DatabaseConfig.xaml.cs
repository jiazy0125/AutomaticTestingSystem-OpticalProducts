using System.Data;
using System.Text;
using System.Windows.Controls;
using AutomaticTestingSystem.Framework.Database;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{
    /// <summary>
    /// DatabaseConfig.xaml 的交互逻辑
    /// </summary>
    public partial class DatabaseConfig : UserControl
    {
        public DatabaseConfig()
        {
            InitializeComponent();
        }

        private void dTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb= (ComboBox)e.OriginalSource;
            if (cb.SelectedItem != null)
            {
                SystemSettings.Columns.Clear();
                var sql = new StringBuilder();
                sql.Append("SELECT ColName = a.name, Description = isnull(g.[value], '')");
                sql.Append(" FROM syscolumns a left join systypes b");
                sql.Append(" on a.xusertype = b.xusertype inner join sysobjects d");
                sql.Append(" on a.id = d.id  and d.xtype = 'U' and d.name <> 'dtproperties'");
                sql.Append(" left join syscomments e on a.cdefault = e.id");
                sql.Append(" left join sys.extended_properties g");
                sql.Append(" on a.id = G.major_id and a.colid = g.minor_id");
                sql.Append(" left join sys.extended_properties f");
                sql.Append(" on d.id = f.major_id and f.minor_id = 0");
                sql.Append($" where d.name = '{((ComboBoxItemModel)cb.SelectedItem).Name}'");
                sql.Append(" order by a.id, a.colorder");

                DataTable dt = DbFactory.Execute().ExecuteTable(sql.ToString(), CommandType.Text, null);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SystemSettings.Columns.Add(new ComboBoxItemModel(dr["ColName"].ToString(), dr["Description"].ToString()));
                    }
                }



            }
        }
    }
}
