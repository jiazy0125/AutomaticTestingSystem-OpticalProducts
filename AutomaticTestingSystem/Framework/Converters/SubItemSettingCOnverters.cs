using System;
using System.Windows.Data;
using System.Globalization;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.UserControls.ProcessDesign;
using Newtonsoft.Json;
using System.Linq;
using AutomaticTestingSystem.Framework.Model;
using AutomaticTestingSystem.UserControls.Settings;

namespace AutomaticTestingSystem.Framework.Converters
{

    public class DatabaseConfigConverter : IValueConverter
    {
        private DatabaseConfigModel dcm;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null) return null;
            var para = (string)parameter;
            try
            {
                dcm = JsonConvert.DeserializeObject<DatabaseConfigModel>((string)value);
                switch (para.ToLower())
                {
                    case "table":
                        return SystemSettings.DbTables.First(t => t.Name == this.GetValue<string>(dcm, para));
                    case "column":
                        return SystemSettings.Columns.First(t => t.Name == this.GetValue<string>(dcm, para));
                    default:
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = (ComboBoxItemModel)value;
            if (dcm is null) dcm = new DatabaseConfigModel();
            this.SetValue(dcm, (string)parameter, data?.Name);
            return JsonConvert.SerializeObject(dcm);
        }
    }
    
    public class ModuleConfigConverter : IValueConverter
    {
        private ModuleConfigModel mcm;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null) return null;
            var para = (string)parameter;
            try
            {
                mcm = JsonConvert.DeserializeObject<ModuleConfigModel>((string)value);
                //return this.GetValue<string>(mcm, (string)parameter);
                switch (para.ToLower())
                {
                    case "page":
                        //return this.GetValue<string>(mcm, para);
                        //return SystemSettings.MdlTables.First(t => t.Name == this.GetValue<string>(mcm, para));
                    case "address":
                    case "datalength":
                        return this.GetValue<string>(mcm, para);
                    default:
                        return null;
                }
            }
            catch{ return null; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (mcm is null) mcm = new ModuleConfigModel();
            if (value is ComboBoxItemModel cb)
            {
                this.SetValue(mcm, (string)parameter, cb.Name);
            }
            else
                this.SetValue(mcm, (string)parameter, value);
            return JsonConvert.SerializeObject(mcm);
        }
    }

    public class InstrumentConfigConverter : IValueConverter
    {
        private InstrumentConfigModel icm;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null) return null;
            var para = (string)parameter;
            try
            {
                icm = JsonConvert.DeserializeObject<InstrumentConfigModel>((string)value);
                switch (para.ToLower())
                {
                    case "name":
                        return SystemSettings.InstrumentsList.First(t => t.Name == this.GetValue<string>(icm, para));
                    case "slot":
                    case "channel":
                        return this.GetValue<string>(icm, para);
                    default:
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (icm is null) icm = new InstrumentConfigModel();
            if (value is InstrumentModel instr)
            {
                this.SetValue(icm, (string)parameter, instr.Name);
            }
            else
                this.SetValue(icm, (string)parameter, value);
            return JsonConvert.SerializeObject(icm);
        }
    }
    /// <summary>
    /// Variant Converter
    /// </summary>
    public class VariantConfigConverter : IValueConverter
    {
        private VariantConfigModel vcm;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null) return null;
            var para = (string)parameter;
            try
            {
                vcm = JsonConvert.DeserializeObject<VariantConfigModel>((string)value);
                switch (para.ToLower())
                {
                    case "path":
                        return SystemSettings.Variants.First(t => t.Name == this.GetValue<string>(vcm, para));
                    default:
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (vcm is null) vcm = new VariantConfigModel();
            if (value is ComboBoxItemModel cb)
            {
                this.SetValue(vcm, (string)parameter, cb.Name);
            }
            else
                this.SetValue(vcm, (string)parameter, value);
            return JsonConvert.SerializeObject(vcm);
        }
    }

    public class FileConfigConverter : IValueConverter
    {
        private FileConfigModel fcm;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null) return null;
            try
            {
                fcm = JsonConvert.DeserializeObject<FileConfigModel>((string)value);
                return this.GetValue<string>(fcm, (string)parameter);
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (fcm is null) fcm = new FileConfigModel();
            this.SetValue(fcm, (string)parameter, value);
            return JsonConvert.SerializeObject(fcm);
        }
    }

    public class SubItemContentConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null) return null;
            OptionType st;
            try
            {
                st = (OptionType)value;
            }
            catch { return null; }

            switch (st)
            {
                case OptionType.Default:
                    return null;
                case OptionType.Database:
                    return new DatabaseConfig();
                case OptionType.Module:
                    return new ModuleConfig();
                case OptionType.Instrument:
                    return new InstrumentConfig();
                case OptionType.File:
                    return new FileConfig();
                case OptionType.Variant:
                    return new VariantConfig();

            }

            return null;
        }    

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class SubItemOperatorConverter : IValueConverter
    {
        private object _operator = "＝";
        private object _value = "";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var _isOperater = (string)parameter == "Operater";
            if (value == null)
            {
                _operator = _isOperater ? "≥" : "≤";
                _value = "";
            }
            else
            {
                try
                {
                    _operator = ((string)value).Substring(0, 1);
                    _value = ((string)value).Substring(1);
                    return _isOperater ? _operator : _value;
                }
                catch 
                {
                    return null;
                }
            }
            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var _isOperater = (string)parameter == "Operater";

            if (_isOperater) _operator = value;
            else _value = value;
            return _operator.ToString() + _value.ToString();

        }
    }

}
