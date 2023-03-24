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

public partial class GetCar : Window
{
    public Car Car { get; set; }
    public GetCar(Car car)
    {
        InitializeComponent();
        DataContext= this;
        Car = car;
        //Car.Id = car.Id;
        //Car.Make = car.Make;
        //Car.Model = car.Model;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
}
