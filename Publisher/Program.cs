using PublisherService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
namespace Spike
{
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var serviceHost = new ServiceHost(typeof(MessagePublisherService));
            serviceHost.Closing += new EventHandler(host_Closing);
            serviceHost.Open();
            Console.WriteLine("Service Started");
            Console.WriteLine("Enter message to send....(X to exit)..");
            var message = Console.ReadLine();

            while (message.ToUpper() != "X")
            {
                try
                {
                    if (MessagePublisherService.MessageReceived != null)
                    {
                        Console.WriteLine(MessagePublisherService.MessageReceived.GetInvocationList().Length.ToString() + " Subscribers");
                        MessagePublisherService.SendMessage(message);
                        Console.WriteLine("Sent: " + message);
                    }
                    else
                    {
                        Console.WriteLine("0 Subscribers");
                        Console.WriteLine("Skipped: " + message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                Console.WriteLine("Enter message to send....(X to exit)..");
                message = Console.ReadLine();
            }

            serviceHost.Close();
        }

        static void host_Closing(object sender, EventArgs e)
        {
            try
            {
                MessagePublisherService.NotifyServiceStop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
