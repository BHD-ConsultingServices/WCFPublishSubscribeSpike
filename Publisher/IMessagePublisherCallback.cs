namespace PublisherService
{
    using System.ServiceModel;

    public interface IMessagePublisherCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnMessageReceived(string message);
    }
}
