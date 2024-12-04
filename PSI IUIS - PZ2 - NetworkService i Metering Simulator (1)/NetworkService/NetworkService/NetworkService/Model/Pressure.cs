using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NetworkService.Model
{
    public class Pressure : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private EnumType.type type;
        private string image;

        private double measurementValue;

        public int ID
        {
            get => id;
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(ID));
                }
            }

        }

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }

        }

        public string Image
        {
            get => image;
            set
            {
                if (image != value)
                {
                    image = value;
                    OnPropertyChanged(nameof(Image));
                }
            }

        }

        public EnumType.type MeterType
        {
            get => type;
            set
            {
                if (type != value)
                {
                    type = value;
                    
                    OnPropertyChanged(nameof(MeterType));
                }
            }

        }

        public double MeasurementValue
        {
            get => measurementValue;
            set
            {
                if (measurementValue != value)
                {
                    measurementValue = value;
                    OnPropertyChanged(nameof(MeasurementValue));
                }
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsValueValidForType()
        {
            bool isValid = true;

            switch (MeterType.ToString())
            {
                case "CableSensor":
                    if (MeasurementValue > 5)
                    {
                        isValid = false;
                    }
                    break;
                case "DigitalMonometer":
                    if (MeasurementValue > 10)
                    {
                        isValid = false;
                    }
                    break;
            }

            return isValid;
        }

    }
}
