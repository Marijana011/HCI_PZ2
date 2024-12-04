using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Data
{
    public class ComboBoxItemData
    {
        public static Dictionary<string, string> Type = new Dictionary<string, string>()
        {
            { "CableSensor", "pack://application:,,,/Images/cableSensor.jpg"},
            { "DigitalMonometer", "pack://application:,,,/Images/DigitalMonometer.jpg"}

        };

    }
}
