using Client.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
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

namespace Client;

public partial class MainWindow : Window
{
    public Car Car { get; set; }
    TcpClient tcpClient;
    BinaryWriter bw;
    BinaryReader br;
    public MainWindow()
    {
        InitializeComponent();
        combobox.ItemsSource = Enum.GetValues(typeof(HttpMethods));
        tcpClient = new TcpClient();

        var ip = IPAddress.Parse("127.0.0.1");
        var port = 27001;
        tcpClient.Connect(ip, port);

        var serverStream = tcpClient.GetStream();
        bw = new BinaryWriter(serverStream);
        br = new BinaryReader(serverStream);

    }


    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (combobox.SelectedItem is null)
        {
            MessageBox.Show("Please select command");
            return;
        }

        if (combobox.SelectedItem is HttpMethods method)
        {
            ExecuteServerCommand(method);
            combobox.SelectedItem = null;
        }
    }

    private async void ExecuteServerCommand(HttpMethods method)
    {
        var stream = tcpClient.GetStream();
        var bw = new BinaryWriter(stream);
        var br = new BinaryReader(stream);

        switch (method)
        {
            case HttpMethods.GET:
                {
                    if (tbTxt.Text is not null)
                    {

                        var Car = new Car();
                        Car.Id = int.Parse(tbTxt.Text);

                        Command command = new Command()
                        {
                            Method = HttpMethods.GET,
                            Car = Car
                        };

                        string jsonString = JsonSerializer.Serialize(command);
                        bw.Write(jsonString);

                        var jsonResponse = br.ReadString();

                        var car = JsonSerializer.Deserialize<Car>(jsonResponse);
                        if (car is not null && car.Make is not null && car.Make.Length>0)
                        {
                            GetCar getCar = new GetCar(car);
                            getCar.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Car couldn't find");
                            
                        }

                        tbTxt.IsEnabled= false;
                        tbTxt.Text=string.Empty;
                    }
                    break;
                }
            case HttpMethods.POST:
                {
                    MessageBox.Show("POST");
                    Car car = new Car();
                    AddCar addCar = new AddCar();
                    addCar.ShowDialog();
                    Command command = new Command()
                    {
                        Method = HttpMethods.POST,
                        Car = addCar.Car
                    };

                    string jsonString = JsonSerializer.Serialize(command);
                    bw.Write(jsonString);

                    MessageBox.Show(br.ReadBoolean() ? "Added Successfully" : "Error Ocurred");
                    break;
                }
            case HttpMethods.PUT:
                {
                    if (tbTxt.Text is not null)
                    {
                        var Car = new Car();
                        Car.Id = int.Parse(tbTxt.Text);

                        Command command = new Command()
                        {
                            Method = HttpMethods.GET,
                            Car = Car
                        };

                        string jsonString = JsonSerializer.Serialize(command);
                        bw.Write(jsonString);

                        var jsonResponse = br.ReadString();
                        var car = JsonSerializer.Deserialize<Car>(jsonResponse);

                        PutCar putCar = new PutCar(car);
                        putCar.ShowDialog();


                        Command commandPut = new Command()
                        {
                            Method = HttpMethods.PUT,
                            Car = putCar.Car
                        };

                        string jsonStringPut = JsonSerializer.Serialize(commandPut);
                        bw.Write(jsonStringPut);

                        MessageBox.Show(br.ReadBoolean() ? "Put Successfully" : "Error Ocurred");
                        bw.Flush();
                       
                    }

                    break;
                }      
            case HttpMethods.DELETE:
                {
                    if (tbTxt.Text is not null)
                    { 
                        var Car = new Car();
                        Car.Id = int.Parse(tbTxt.Text);

                        Command command = new Command()
                        {
                            Method = HttpMethods.DELETE,
                            Car = Car
                        };

                        string jsonString = JsonSerializer.Serialize(command);
                        bw.Write(jsonString);
                        bool isDeleted = br.ReadBoolean();
                        MessageBox.Show(isDeleted ? "Deleted Successfully" : "Error Ocurred");

                        tbTxt.IsEnabled= false;
                        tbTxt.Text = string.Empty;
                       
                    }

                    break;
                }
        }
    }

    private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox cb)
        {
            if (cb.SelectedItem is HttpMethods method && (method == HttpMethods.GET || method == HttpMethods.DELETE || method == HttpMethods.PUT))
                tbTxt.IsEnabled = true;
        }
    }
}
