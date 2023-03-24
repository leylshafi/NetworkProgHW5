using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;
using Server.Models;
using System;

List<Car> cars = new List<Car>();
int id = 0;

var ip = IPAddress.Parse("127.0.0.1");
var port = 27001;

var listener = new TcpListener(ip, port);
listener.Start();
while (true)
{
    var client = listener.AcceptTcpClient();


    var clientStream = client.GetStream();
    var br = new BinaryReader(clientStream);
    var bw = new BinaryWriter(clientStream);

    while (true)
    {
        var jsonStr = br.ReadString();
        var command = JsonSerializer.Deserialize<Command>(jsonStr);

        Console.WriteLine(command.Method);

        if (command is null)
            continue;


        switch (command.Method)
        {
            case HttpMethods.GET:
                {
                    if (command.Car is not null)
                    {
                        int idCar = command.Car.Id;
                        var jsonResponse = JsonSerializer.Serialize(GetById(idCar));
                        bw.Write(jsonResponse);
                    }
                    break;
                }
            case HttpMethods.POST:
                if (command.Car is not null)
                {
                    command.Car.Id = ++id;
                    bw.Write(Add(command.Car));
                }
                else bw.Write(false);
                break;
            case HttpMethods.PUT:
                break;
            case HttpMethods.DELETE:
                break;
            default:
                break;
        }
    }
}




bool Add(Car car)
{
    if (car is not null)
    {
        cars.Add(car);
        return true;
    }
    return false;
}

bool Delete(int id)
{
    if (cars[id] is not null)
    {
        cars.RemoveAt(id);
        return true;
    }
    return false;
}

Car? GetById(int id)
{
    foreach (var car in cars)
    {
        if (car.Id == id)
        {
            return car;
        }
        else return new Car();
    }
    return new Car();
}

List<Car>? GetAll()
{
    return cars;
}

bool Update(Car car)
{
    return true;
}
