using NetworkService.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NetworkService.ViewModel
{
    public class NetworkEntitiesViewModel : BindableBase
    {
        public static List<string> PressureTypes { get; set; } = new List<string> { "Cable sensor", "Digital monometer" };
        public static List<Pressure> Pressuress { get; set; }
        public static ObservableCollection<Pressure> SearchedPressure { get; set; }
        public ObservableCollection<Pressure> FilteredPressures { get; set; }
        public ObservableCollection<Pressure> PressuresToShow { get; set; }




        private bool greaterCheck;
        private bool lowerCheck;
        private bool equalCheck;
        private int filterID;
        private Pressure chosenPressures;
        

        private int cableSCount = 0;
        private int digMonCount = 0;

        public ObservableCollection<Pressure> Press { get; set; } = new ObservableCollection<Pressure>();
        

  

        private EnumType.type pType;
        private string selectedEntityTypeToFilter;
        private string idText;
        private string nameText;
        private ImageSource imageSource;
        private double measurementValue;
        private Pressure selectedPressure;




        public MyICommand AddCommand { get; set; }
        public MyICommand DeleteCommand { get; set; }

        public MyICommand FilterCommand { get; set; }
        public MyICommand ResetCommand { get; set; }

        public NetworkEntitiesViewModel()
        {
            greaterCheck = true;
            lowerCheck = false;
            equalCheck = false;
            SearchedPressure = new ObservableCollection<Pressure>();          
            
            LoadPressure();
            
            AddCommand = new MyICommand(OnAdd, CanAdd);
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
            PressureTypes = new List<string>(Enum.GetNames(typeof(EnumType.type)));
            FilterCommand = new MyICommand(Filter);
            ResetCommand = new MyICommand(RestoreFilter);          
            SelectedPressure = null;
        }


        public void LoadPressure()
        {
            ObservableCollection<Pressure> pressures = new ObservableCollection<Pressure>();
            pressures.Add(new Pressure { ID = 1, Name = "CS1", MeterType = EnumType.type.CableSensor, MeasurementValue = 5.54, Image = AppDomain.CurrentDomain.BaseDirectory + "Images\\cableSensor.jpg" });
            pressures.Add(new Pressure { ID = 2, Name = "DM1", MeterType = EnumType.type.DigitalMonometer, MeasurementValue = 9.11, Image = AppDomain.CurrentDomain.BaseDirectory + "Images\\DigitalMonometer.jpg" });
            pressures.Add(new Pressure { ID = 3, Name = "DM2", MeterType = EnumType.type.DigitalMonometer, MeasurementValue = 12.12, Image = AppDomain.CurrentDomain.BaseDirectory + "Images\\DigitalMonometer.jpg" });
            pressures.Add(new Pressure { ID = 4, Name = "CS2", MeterType = EnumType.type.CableSensor, MeasurementValue = 7.89, Image = AppDomain.CurrentDomain.BaseDirectory + "Images\\cableSensor.jpg" });
            pressures.Add(new Pressure { ID = 5, Name = "CS3", MeterType = EnumType.type.CableSensor, MeasurementValue = 6.44, Image = AppDomain.CurrentDomain.BaseDirectory + "Images\\cableSensor.jpg" });
            pressures.Add(new Pressure { ID = 6, Name = "CS4", MeterType = EnumType.type.CableSensor, MeasurementValue = 5.44, Image = AppDomain.CurrentDomain.BaseDirectory + "Images\\cableSensor.jpg" });
            pressures.Add(new Pressure { ID = 7, Name = "CS5", MeterType = EnumType.type.CableSensor, MeasurementValue = 6.44, Image = AppDomain.CurrentDomain.BaseDirectory + "Images\\cableSensor.jpg" });

            Pressuress = pressures.ToList();
            SearchedPressure = pressures;
        }     

        public bool GreaterCheck
        {
            get
            {
                return greaterCheck;
            }

            set
            {
                if (greaterCheck != value)
                {
                    greaterCheck = value;

                    if (greaterCheck)
                    {
                        LowerCheck = false;
                        EqualCheck = false;

                        OnPropertyChanged("LowerCheck");
                        OnPropertyChanged("EqualCheck");
                    }

                    OnPropertyChanged("GreaterCheck");
                }
            }
        }

        public bool LowerCheck
        {
            get
            {
                return lowerCheck;
            }

            set
            {
                if (lowerCheck != value)
                {
                    lowerCheck = value;

                    if (lowerCheck)
                    {
                        GreaterCheck = false;
                        EqualCheck = false;

                        OnPropertyChanged("GreaterCheck");
                        OnPropertyChanged("EqualCheck");
                    }

                    OnPropertyChanged("LowerCheck");
                }
            }
        }

        public bool EqualCheck
        {
            get
            {
                return equalCheck;
            }

            set
            {
                if (equalCheck != value)
                {
                    equalCheck = value;

                    if (equalCheck)
                    {
                        GreaterCheck = false;
                        lowerCheck = false;

                        OnPropertyChanged("GreaterCheck");
                        OnPropertyChanged("LowerCheck");
                    }

                    OnPropertyChanged("EqualCheck");
                }
            }
        }

        public int FilterID
        {
            get
            {
                return filterID;
            }
            set
            {
                if (filterID != value)
                {
                    filterID = value;
                    OnPropertyChanged("FilterID");
                }
            }

        }

        public string IDText
        {
            get
            {
                return idText;
            }
            set
            {
                if (idText != value)
                {
                    idText = value;
                    OnPropertyChanged("IDText");
                    AddCommand.RaiseCanExecuteChanged();
                }
            }

        }

        public double MeasurementValueText
        {
            get
            {
                return measurementValue;
            }
            set
            {
                if (measurementValue != value)
                {
                    measurementValue = value;
                    OnPropertyChanged("MeasurementValueText");

                }
            }

        }

        public string NameText
        {
            get
            {
                return nameText;
            }

            set
            {
                if (nameText != value)
                {
                    nameText = value;
                    OnPropertyChanged("NameText");
                    AddCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public ImageSource ImageSource
        {
            get
            {
                return imageSource;
            }

            set
            {
                if (imageSource != value)
                {
                    imageSource = value;
                    OnPropertyChanged("ImageSource");
                }
            }
        }

        public EnumType.type PType
        {
            get
            {
                return pType;
            }
            set
            {
                if (pType != value)
                {
                    pType = value;
                    OnPropertyChanged("PType");
                }
                if (value == EnumType.type.CableSensor)
                {
                    ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/cableSensor.jpg", UriKind.Absolute));
                }
                else
                {
                    ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/DigitalMonometer.jpg", UriKind.Absolute));
                }

            }


        }


        public string SelectedEntityTypeToFilter
        {
            get
            {
                return selectedEntityTypeToFilter;
            }
            set
            {
                if (selectedEntityTypeToFilter != value)
                {
                    selectedEntityTypeToFilter = value;
                    OnPropertyChanged("SelectedEntityTypeToFilter");
                }

            }
        }

        private bool CanAdd()
        {
            if (IDText != null && NameText != null)
            {
                return true;
            }
            return false;
        }

        private void OnAdd()
        {
            int tempID = 0;
            try
            {
                tempID = Int32.Parse(IDText);
            }
            catch
            {
                System.Windows.MessageBox.Show("ID must be a number!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string tempS = NameText;
            if (string.IsNullOrWhiteSpace(tempS))
            {
                System.Windows.MessageBox.Show("The name must not be a white space!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            bool exists = false;
            foreach (Pressure p in SearchedPressure)
            {
                if (p.ID == tempID)
                {
                    exists = true;
                }
            }
            if (exists)
            {
                System.Windows.MessageBox.Show("ID must be unique!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            SearchedPressure.Add(new Pressure { ID = tempID, Name = NameText, MeasurementValue = 5, MeterType = PType });
            System.Windows.MessageBox.Show("Entity entered successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            
        }

        private bool CanDelete()
        {
            return SelectedPressure != null;
        }

        private void OnDelete()
        {

            if (MessageBox.Show("Are you sure you want to delete the entity?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SearchedPressure.Remove(SelectedPressure);
            }
            else
            {
                return;
            }
        }

        public Pressure SelectedPressure
        {
            get
            {
                return selectedPressure;
            }
            set
            {
                selectedPressure = value;
                DeleteCommand.RaiseCanExecuteChanged();
            }

        }


        private void RestoreFilter()
        {
            if (MessageBox.Show("Are you sure you want to restore all fields", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                EqualCheck = false;
                GreaterCheck = false;
                LowerCheck = false;
                FilterID = 0;
                SelectedEntityTypeToFilter = null;
                SearchedPressure.Clear();
                foreach(Pressure p in Pressuress)
                {
                    SearchedPressure.Add(p);
                }
            }
            else
            {
                return;
            }
        }

        

        private void Filter()
        {
            SearchedPressure.Clear();
            var filteredByType = new List<Pressure>();

            if(!string.IsNullOrEmpty(SelectedEntityTypeToFilter))
            {
                foreach(Pressure p in Pressuress)
                {
                    if(p.MeterType.ToString().Equals(SelectedEntityTypeToFilter))
                    {
                        filteredByType.Add(p);
                    }
                }
            }
            else
            {
                filteredByType.AddRange(Pressuress);
            }


            foreach(Pressure p in filteredByType)
            {
                bool matches = false;
                if(LowerCheck && p.ID < filterID)
                {
                    matches = true;
                }
                if(EqualCheck && p.ID == filterID)
                {
                    matches= true;
                }
                if(GreaterCheck  && p.ID > filterID)
                {
                    matches = true;
                }

                if(matches)
                {
                    SearchedPressure.Add(p);
                }
            }

            
            MessageBox.Show("Successful filtering!", "Filtered", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        

    }
}
