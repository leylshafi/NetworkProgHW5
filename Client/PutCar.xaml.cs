using Client.Models;
using System;
using System.Collections.Generic;
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

namespace Client;

public partial class PutCar : Window
{
    public Car Car { get; set; }
    public PutCar(Car car)
    {
        InitializeComponent();
        DataContext = this;
        Car = car;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Car = new Car
        {
            Model = modelTxt.Text,
            Make = makeTxt.Text,
            Year = ushort.Parse(yearTxt.Text),
            VIN = vinTxt.Text,
            Color = colorTxt.Text
        };


        DialogResult = true;
    }
}
