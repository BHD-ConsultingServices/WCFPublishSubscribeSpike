namespace PublisherService
{
    using System;
    using System.ServiceModel;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    internal sealed class MessagePublisherService : IMessagePublisher
    {
        public delegate void CallbackDelegate<T>(T t);
        public static CallbackDelegate<string> MessageReceived;

        public void Subscribe()
        {
            var callback = OperationContext.Current.GetCallbackChannel<IMessagePublisherCallback>();
            MessageReceived += callback.OnMessageReceived;
            var callbackCommunicationObject = (ICommunicationObject)callback;
            callbackCommunicationObject.Closed += new EventHandler(EventService_Closed);
            callbackCommunicationObject.Closing += new EventHandler(EventService_Closing);
        }

        void EventService_Closing(object sender, EventArgs e)
        {
            Console.WriteLine("Client Closing...");
        }

        void EventService_Closed(object sender, EventArgs e)
        {
            MessageReceived -= ((IMessagePublisherCallback)sender).OnMessageReceived;
            Console.WriteLine("Closed Client Removed!");

        }

        public void Unsubscribe()
        {
            var callback = OperationContext.Current.GetCallbackChannel<IMessagePublisherCallback>();
            MessageReceived -= callback.OnMessageReceived;
        }

        public static void SendMessage(string Message)
        {
            try
            {
                MessageReceived(Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void NotifyServiceStop()
        {
            SendMessage("Service Stopped!");
        }
    }
}

