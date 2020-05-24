using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.UserControls.Settings;

namespace AutomaticTestingSystem.Framework.Converters
{
    class CommnicationTypeConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var instr = (InstrumentModel)value;

            switch (instr.CommnunicationType)
            {

                case CommunicationType.UDP:
                    return  new UdpType() { DataContext = instr.Config };
                case CommunicationType.TCP:
                    return  new TcpIpType() { DataContext = instr.Config };
                case CommunicationType.SerialPort:
                    return  new SerialType() { DataContext = instr.Config };
                case CommunicationType.GPIB:
                    return  new GPIBType() { DataContext = instr.Config };
                case CommunicationType.USB:
                    return  new USBType() { DataContext = instr.Config };
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
