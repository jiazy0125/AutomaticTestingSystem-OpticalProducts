using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutomaticTestingSystem.Framework.Common;

namespace AutomaticTestingSystem.Framework.Model
{
    public class PropertyChangedModel : INotifyPropertyChanged, IModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged<TField>(ref TField field, TField newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TField>.Default.Equals(field, newValue)) return ;
            field = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        private Dictionary<string, bool> _changesFlags ;
        private List<int> _columnIndex;
        private object _oldData;
        public void CreateChangesFlag(object obj)
        {
            _changesFlags = new Dictionary<string, bool>();
            _columnIndex = new List<int>();
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                try
                {
                    var attr = property.GetCustomAttribute<ColumnAttribute>();
                    if (attr != null)
                    {
                        _changesFlags.Add(attr.Name, false);
                        _columnIndex.Add(attr.Index);
                    }
                }
                catch { }
            }
        }

        public void CacheOldData(PropertyChangedModel obj)
        {
            _oldData = obj.MemberwiseClone();
        }

        public bool UpdateChangeFlag<TField>(TField value, [CallerMemberName] string propertyName = null)
        {
            if (_oldData == null) return false;

            _changesFlags[propertyName] = !EqualityComparer<TField>.Default.Equals(value, this.GetValue<TField>(_oldData, propertyName));

            var flag = false;
            foreach (var item in _changesFlags)
            {
                flag |= item.Value;            
            }
            return flag;
        }

        public void ReInitChangesFlags()
        {
            var keys = new string[_changesFlags.Count];
            _changesFlags.Keys.CopyTo(keys, 0);
            for (var i = 0; i < _changesFlags.Count; i++)
            {
                _changesFlags[keys[i]] = false;
            
            }
        }

        public int[] ChangedIndexs
        { 
            get
            {
                var indexs = new List<int>();
                var a = 0;
                if (_changesFlags == null) return null;
                foreach (var item in _changesFlags)
                {
                    if (item.Value == true) indexs.Add(_columnIndex[a]);
                    a++;
                }
                if (indexs.Count <= 0) return null;
                return indexs.ToArray();
            }
        
        }
    }
}
