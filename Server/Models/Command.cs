using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models;

[Serializable]
public class Command
{
    public HttpMethods Method { get; set; }
    public Car? Car { get; set; }
}