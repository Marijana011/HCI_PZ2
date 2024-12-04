using NetworkService.Model;
using NetworkService.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace NetworkService.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        private bool file;
        private int id;
        private double value;

        

        private int count = 15; // Inicijalna vrednost broja objekata u sistemu
                                // ######### ZAMENITI stvarnim brojem elemenata
                                //           zavisno od broja entiteta u listi

        public MyICommand<string> NavCommand { get; private set; }
        public MyICommand UndoCommand { get; set; }
        public MyICommand CycleTabsCommand { get; set; }

        private NetworkEntitiesViewModel networkEntitiesViewModel = new NetworkEntitiesViewModel();
        private NetworkDisplayViewModel networkDisplayViewModel = new NetworkDisplayViewModel();
        private MeasurementGraphViewModel measurementGraphViewModel = new MeasurementGraphViewModel();
        private BindableBase currentViewModel;
        private BindableBase lastViewModel;
        public static Stack<SaveState<CommandType, object>> UndoStack { get; set; }
        public MainWindowViewModel()
        {
            createListener(); //Povezivanje sa serverskom aplikacijom

            UndoStack = new Stack<SaveState<CommandType, object>>();

            NavCommand = new MyICommand<string>(OnNav);
            UndoCommand = new MyICommand(OnUndo);
            
            CurrentViewModel = networkEntitiesViewModel;
            SelectedContent = new NetworkEntitiesViewModel();

        
        }


       


        private object _selectedContent;
        public object SelectedContent
        {
            get => _selectedContent;
            set
            {
                SetProperty(ref _selectedContent, value);
                
            }
        }

        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "NetworkEntitiesView":
                    lastViewModel = CurrentViewModel;
                    CurrentViewModel = networkEntitiesViewModel;
                    break;
                case "NetworkDisplayView":
                    lastViewModel = CurrentViewModel;
                    CurrentViewModel = networkDisplayViewModel;
                    break;
                case "MeasurementGraphView":
                    lastViewModel = CurrentViewModel;
                    CurrentViewModel = measurementGraphViewModel;
                    break;
            }
        }


        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null)
            {
                foreach(Pressure newPressure in e.NewItems)
                {
                    if(!networkDisplayViewModel.Pressures.Contains(newPressure))
                    {
                        networkDisplayViewModel.Pressures.Add(newPressure);
                    }
                }
            }
            if(e.OldItems != null)
            {
                foreach (Pressure oldPressure in e.OldItems)
                {
                    if(networkDisplayViewModel.Pressures.Contains(oldPressure))
                    {
                        networkDisplayViewModel.Pressures.Remove(oldPressure);
                    }
                    else
                    {
                        int canvasIndex = networkDisplayViewModel.GetCanvasIndexForEntityId(oldPressure.ID);
                        networkDisplayViewModel.DeleteEntityFromCanvas(oldPressure);
                    }
                }

            }
        }

        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25675);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */
                            //int c = ViewModel.NetworkViewViewModel.monitor;
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(count.ToString());
                            stream.Write(data, 0, data.Length);
                            file = false;
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            Console.WriteLine(incomming); //Na primer: "Entitet_1:272"

                            //################ IMPLEMENTACIJA ####################
                            // Obraditi poruku kako bi se dobile informacije o izmeni
                            // Azuriranje potrebnih stvari u aplikaciji
                            string[] split = incomming.Split('_', ':');
                            id = Int32.Parse(split[1]);
                            value = Double.Parse(split[2]);

                            string incommingEntityId = incomming.Substring(0, incomming.IndexOf(':'));
                            double newValue = double.Parse(incomming.Substring(incomming.IndexOf(':') + 1));

                            for (int idx = 0; idx < networkEntitiesViewModel.PressuresToShow.Count; idx++)
                            {
                                string currentEntityId = $"Entitet_{idx}";
                                if (currentEntityId == incommingEntityId)
                                {
                                    networkEntitiesViewModel.PressuresToShow[idx].MeasurementValue = newValue;

                                    using (StreamWriter writer = File.AppendText("Log.txt"))
                                    {
                                        DateTime dateTime = DateTime.Now;
                                        writer.WriteLine($"{dateTime}: {networkEntitiesViewModel.PressuresToShow[idx].Name}, {newValue}");
                                    }

                                    
                                    //measurementGraphViewModel.OnShow();

                                    break;
                                }
                            }

                        }
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }

        private void OnUndo()
        {
            CurrentViewModel = lastViewModel;
            
        }
    }
}
