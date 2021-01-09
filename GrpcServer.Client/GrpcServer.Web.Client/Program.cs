using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer.Web.Protos;

namespace GrpcServer.Web.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new EmployeeService.EmployeeServiceClient(channel);

            var option = int.Parse(args[0]);
            switch (option)
            {
                case 1:
                    await GetByNoAsync(client);
                    break;
                case 2:
                    await GetAllAsync(client);
                    break;
                case 3:
                    await AddPhotoAsync(client);
                    break;
                case 5:
                    await SaveAllAsync(client);
                    break;
                default:
                    break;
            }

            Console.WriteLine($"Press any key to exit.");
            Console.ReadKey();
        }

        private static async Task SaveAllAsync(EmployeeService.EmployeeServiceClient client)
        {
            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = 111,
                    FirstName = "Monica",
                    LastName = "Geller",
                    Salary = 7890.1f,
                },
                new Employee
                {
                    No = 222,
                    FirstName = "Joey",
                    LastName = "Tribbiani",
                    Salary = 500
                }
            };


            var call = client.SaveAll();

            //因为两端都是用流的方式传输，所以需要获取两个流示例
            var requestStream = call.RequestStream;
            var responseStream = call.ResponseStream;

            //创建一个响应流Task,用于接收服务器端返回的消息
            var responseTask = Task.Run(async () =>
            {
                while (await responseStream.MoveNext())
                {
                    Console.WriteLine($"Saved:{responseStream.Current.Employee}");
                }
            });

            //像之前一样，向请求流中写入数据
            foreach (var employee in employees)
            {
                await requestStream.WriteAsync(new EmployeeRequest
                {
                    Employee = employee
                });
            }

            //顺序不可调换，否则会一直收不到服务器端返回的消息
            await requestStream.CompleteAsync();
            await responseTask;
        }

        private static async Task GetAllAsync(EmployeeService.EmployeeServiceClient client)
        {
            using var call = client.GetAll(new GetAllRequest());
            var responseStream = call.ResponseStream;
            while (await responseStream.MoveNext())
            {
                Console.WriteLine(responseStream.Current.Employee);
            }
        }

        public static async Task GetByNoAsync(EmployeeService.EmployeeServiceClient client)
        {
            var md = new Metadata
            {
                {"username", "dave"},
                {"role", "administrator"}
            };

            var result = await client.GetByNoAsync(
                new GetByNoRequest
                {
                    No = 1994
                }, md);
            Console.WriteLine($"Response message: {result}");
        }



        public static async Task AddPhotoAsync(EmployeeService.EmployeeServiceClient client)
        {
            //这里也可以传输元数据
            var md = new Metadata
            {
                {"username", "dave"},
                {"role", "administrator"}
            };

            //首先读取数据
            FileStream fs = File.OpenRead("logo.png");
            var call = client.AddPhoto(md);
            //创建请求数据流
            var stream = call.RequestStream;

            while (true)
            {
                byte[] buffer = new byte[1024];
                //读取数据长度
                int numRead = await fs.ReadAsync(buffer, 0, buffer.Length);
                //判断是否已经读取完
                if (numRead == 0)
                {
                    break;
                }

                //若读取到长度小于buffer，则将buffer重置为numRead大小
                if (numRead < buffer.Length)
                {
                    Array.Resize(ref buffer, numRead);
                }

                //将图片读写到流中
                await stream.WriteAsync(new AddPhotoRequest {Data = ByteString.CopyFrom(buffer)});
            }

            //告知服务器上传完成
            await stream.CompleteAsync();
            //接收服务器返回的消息
            var res = await call.ResponseAsync;
            Console.WriteLine(res);
        }
    }
}