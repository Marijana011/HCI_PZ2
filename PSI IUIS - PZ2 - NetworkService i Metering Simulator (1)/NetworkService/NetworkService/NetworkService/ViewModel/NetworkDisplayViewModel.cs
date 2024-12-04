using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NetworkService.ViewModel
{
    public class NetworkDisplayViewModel : BindableBase
    {
        public BindingList<Pressure> Pressures { get; set; }
        public ObservableCollection<Canvas> CanvasList { get; set; } = new ObservableCollection<Canvas>();
        public static Dictionary<string, Pressure> MetersCanvas { get; set; } = new Dictionary<string, Pressure>();

        public ObservableCollection<MyLine> LineCollection { get; set; }

        private int selectedIndex = 0;
        public static Pressure draggedItem = null;
        private bool dragging = false;
        private Canvas selectedCanvas = null;

        private static bool postoji = false;
        private ListView listViewItem;


        public MyICommand<Pressure> SelectionChangedCommand { get; set; }
        public MyICommand<ListView> MouseLeftButtonUpCommand { get; set; }
        public MyICommand<Canvas> DragOverCommand { get; set; }
        public MyICommand<Canvas> DragLeaveCommand { get; set; }
        public MyICommand<Canvas> DropCommand { get; set; }
        public MyICommand<Canvas> FreeCommand { get; set; }
        public MyICommand<Canvas> LoadCommand { get; set; }
        public MyICommand<ListView> LoadListViewCommand { get; set; }
        public MyICommand<Canvas> ConnectCommand { get; set; }

        public MyICommand<Canvas> SelectionChangedCanvasCommand { get; set; }

        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                selectedIndex = value;
                OnPropertyChanged("SelectedIndex");

            }
        }

        public NetworkDisplayViewModel()
        {
            Pressures = new BindingList<Pressure>();

            CanvasList = new ObservableCollection<Canvas>();

            SelectionChangedCommand = new MyICommand<Pressure>(OnSelectionChanged);
            MouseLeftButtonUpCommand = new MyICommand<ListView>(OnMouseLeftButtonUp);
            DragOverCommand = new MyICommand<Canvas>(OnDragOver);
            DragLeaveCommand = new MyICommand<Canvas>(OnDragLeave);
            DropCommand = new MyICommand<Canvas>(OnDrop);
            FreeCommand = new MyICommand<Canvas>(OnFree);
            LoadCommand = new MyICommand<Canvas>(OnLoad);
            LoadListViewCommand = new MyICommand<ListView>(OnLoadListView);
            ConnectCommand = new MyICommand<Canvas>(OnConnect);

            SelectionChangedCanvasCommand = new MyICommand<Canvas>(OnSelectionCanvasChanged);

            Load();
        }

        public void OnLineDel(Canvas canvas)
        {
            var linesToRemove = canvas.Children.OfType<Line>().ToList();

            foreach (var line in linesToRemove)
            {
                canvas.Children.Remove(line);
            }
        }


        public void OnConnect(Canvas canvas)
        {
            if (selectedCanvas == null)
            {
                selectedCanvas = canvas;
            }
            else
            {
                Canvas parentCanvas = FindParentCanvas(canvas);
                double x1 = Canvas.GetLeft(canvas) + canvas.ActualWidth / 2;
                double y1 = Canvas.GetTop(canvas) + canvas.ActualHeight / 2;
                double x2 = Canvas.GetLeft(selectedCanvas) + selectedCanvas.ActualWidth / 2;
                double y2 = Canvas.GetTop(selectedCanvas) + selectedCanvas.ActualHeight / 2;

                Line line = new Line();
                line.X1 = x1;
                line.Y1 = y1;
                line.X2 = x2;
                line.Y2 = y2;
                line.Stroke = Brushes.Black;


                parentCanvas.Children.Add(line);

                selectedCanvas = null;
            }
        }

        //public bool CanConnect(Canvas canvas)
        //{
         //   return canvas.Resources["taken"] != null;
        //}

        public void DeleteEntityFromCanvas(Pressure entity)
        {
            int canvasIndex = GetCanvasIndexForEntityId(entity.ID);

            if (canvasIndex != -1)
            {
                CanvasList[canvasIndex].Background = Brushes.LightGray;
                CanvasList[canvasIndex].Resources.Remove("taken");
                CanvasList[canvasIndex].Resources.Remove("data");
                

                DeleteLinesForCanvas(canvasIndex);
            }
        }

        private void DeleteLinesForCanvas(int canvasIndex)
        {
            List<MyLine> linesToDelete = new List<MyLine>();

            for (int i = 0; i < LineCollection.Count; i++)
            {
                if ((LineCollection[i].Source == canvasIndex) || (LineCollection[i].Destination == canvasIndex))
                {
                    linesToDelete.Add(LineCollection[i]);
                }
            }

            foreach (MyLine line in linesToDelete)
            {
                LineCollection.Remove(line);
            }
        }

        public int GetCanvasIndexForEntityId(int entityId)
        {
            for (int i = 0; i < CanvasList.Count; i++)
            {
                Pressure entity = (CanvasList[i].Resources["data"]) as Pressure;

                if ((entity != null) && (entity.ID == entityId))
                {
                    return i;
                }
            }
            return -1;
        }

        public Canvas FindParentCanvas(UIElement element)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(element);

            while (parent != null && !(parent is Canvas))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as Canvas;
        }
        

        public void OnSelectionChanged(Pressure meter)
        {
            if (!dragging)
            {
                dragging = true;
                draggedItem = meter;
                DragDrop.DoDragDrop(listViewItem, draggedItem, DragDropEffects.Move);
            }
        }

        public void OnMouseLeftButtonUp(ListView lw)
        {
            draggedItem = null;
            lw.SelectedItem = null;
            dragging = false;
        }


        public void OnSelectionCanvasChanged(Canvas canvas)
        {
            if (!dragging)
            {
                if (canvas.Resources["taken"] != null)
                {
                    dragging = true;
                    draggedItem = MetersCanvas[canvas.Name];

                    canvas.Background = Brushes.White;
                    canvas.Resources.Remove("taken");
                    MetersCanvas.Remove(canvas.Name);
                    DragDrop.DoDragDrop(listViewItem, draggedItem, DragDropEffects.Move);
                }
            }
        }


        public void Load()
        {

            foreach (Pressure meter in NetworkEntitiesViewModel.SearchedPressure)
            {
                postoji = false;
                foreach (Pressure meter2 in MetersCanvas.Values)
                {
                    if (meter.ID == meter2.ID)
                    {
                        postoji = true;
                        break;
                    }

                }
                if (postoji == false)
                    Pressures.Add(new Pressure() { ID = meter.ID, Name = meter.Name, MeasurementValue = meter.MeasurementValue, Image = meter.Image, MeterType = meter.MeterType });
            }

        }

        public void OnDragOver(Canvas canvas)
        {
            if (canvas.Resources["taken"] != null)
            {
                ((TextBlock)(canvas).Children[1]).Text = "X";
                ((TextBlock)(canvas).Children[1]).FontSize = 30;
                ((TextBlock)(canvas).Children[1]).FontWeight = System.Windows.FontWeights.ExtraBold;
            }

        }

        public void OnDragLeave(Canvas canvas)
        {
            if (((TextBlock)(canvas).Children[1]).Text == "X")
            {
                ((TextBlock)(canvas).Children[1]).Text = "";
                ((TextBlock)(canvas).Children[1]).Background = Brushes.Transparent;
            }
        }

        public void OnDrop(Canvas canvas)
        {

            if (draggedItem != null)
            {
                if (canvas.Resources["taken"] == null)
                {
                    BitmapImage logo = new BitmapImage();

                    logo.BeginInit();
                    logo.UriSource = new Uri("pack://application:,,,/Images/" + draggedItem.MeterType.ToString() + ".jpg", UriKind.Absolute);
                    logo.EndInit();

                    canvas.Background = new ImageBrush(logo);
                    MetersCanvas[canvas.Name] = draggedItem;
                    ((Label)(canvas).Children[2]).Content = draggedItem.Name;//
                    canvas.Resources.Add("taken", true);
                    Pressures.Remove(Pressures.Single(x => x.ID == draggedItem.ID));
                    SelectedIndex = 0;
                    CheckValue(canvas);

                }
                else
                {
                    ((TextBlock)(canvas).Children[1]).Text = "";
                    ((TextBlock)(canvas).Children[1]).Background = Brushes.Transparent;
                }
                dragging = false;
            }

        }

        public void OnFree(Canvas canvas)
        {
            try
            {
                if (canvas.Resources["taken"] != null)
                {

                    canvas.Background = Brushes.Pink;
                    foreach (Pressure meter in NetworkEntitiesViewModel.SearchedPressure)
                    {
                        if (!Pressures.Contains(meter) && MetersCanvas[canvas.Name].ID == meter.ID)
                            Pressures.Add(meter);
                    }
                    ((Label)(canvas).Children[2]).Content = "";
                    canvas.Resources.Remove("taken");
                    MetersCanvas.Remove(canvas.Name);
                    ConnectCommand.RaiseCanExecuteChanged();
                }
            }
            catch (Exception) { }

        }



        public void OnLoadListView(ListView listview)
        {
            listViewItem = listview;
        }

        public void OnLoad(Canvas canvas)
        {
            if (MetersCanvas.ContainsKey(canvas.Name))
            {

                BitmapImage logo = new BitmapImage();

                logo.BeginInit();
                Pressure temp = MetersCanvas[canvas.Name];
                logo.UriSource = new Uri("pack://application:,,,/Images/" + temp.MeterType.ToString() + ".jpg", UriKind.Absolute);
                logo.EndInit();

                ((Label)(canvas).Children[2]).Content = temp.Name;
                canvas.Background = new ImageBrush(logo);
                ((TextBlock)(canvas).Children[1]).Text = "";
                canvas.Resources.Add("taken", true);

                CheckValue(canvas);

            }
            if (!CanvasList.Contains(canvas))
            {
                CanvasList.Add(canvas);
            }
        }

        private void CheckValue(Canvas canvas)
        {
            Dictionary<int, Pressure> temp = new Dictionary<int, Pressure>();
            foreach (var meter in NetworkEntitiesViewModel.SearchedPressure)
            {
                temp.Add(meter.ID, meter);
            }
            Task.Delay(1000).ContinueWith(_ =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ((Border)(canvas).Children[0]).BorderBrush = Brushes.Transparent;

                    if (MetersCanvas.Count != 0)
                    {
                        if (MetersCanvas.ContainsKey(canvas.Name))
                        {
                            if (!MetersCanvas.ContainsKey(canvas.Name) || !temp.ContainsKey(MetersCanvas[canvas.Name].ID))
                            {
                                return;
                            }
                            else if (temp[MetersCanvas[canvas.Name].ID].MeasurementValue > 16 || temp[MetersCanvas[canvas.Name].ID].MeasurementValue < 5)
                            {
                                ((Border)(canvas).Children[0]).BorderBrush = Brushes.Red;
                            }
                        }
                    }
                });
                CheckValue(canvas);
            });
        }

    }
}

