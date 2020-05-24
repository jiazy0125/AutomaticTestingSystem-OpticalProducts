using System;
using System.Globalization;
using System.Windows.Data;
using AutomaticTestingSystem.Framework.Common;

namespace AutomaticTestingSystem.Framework.Converters
{
    class LimitConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换器:根据限值操作符返回限值范围字符串
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return null;
            var lop = (OperatorEnum)values[0];
            var uop = (OperatorEnum)values[2];

            if (lop == OperatorEnum.None | uop == OperatorEnum.Equal) 
            {
                switch (uop)
                {
                    case OperatorEnum.None:
                        return null;
                    case OperatorEnum.LessThan:
                    case OperatorEnum.LessThanAndEqual:
                        return $"[ -∞  {values[3].ToString()} ]";
                    case OperatorEnum.Equal:
                        return $"[ {values[3].ToString()} ]";
                }
            }
            else
            {
                if (uop == OperatorEnum.None)
                    return $"[ {values[1].ToString()}  +∞ ]";
                else return $"[ {values[1].ToString()}  {values[3].ToString()} ]";                             
            }
            return null;
        }



        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
