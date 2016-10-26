namespace Subscriber
{
    using System;
    using System.ServiceModel;
    using System.Windows.Forms;

    public partial class Form1 : Form, PublisherServiceReference.IMessagePublisherCallback
    {
        public Form1()
        {
            InitializeComponent();
        }

        private PublisherServiceReference.MessagePublisherClient proxy;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                proxy = new PublisherServiceReference.MessagePublisherClient(new InstanceContext(this));
                proxy.Subscribe();
            }
            catch (Exception ex)
            {
                listBox1.Items.Add(ex.ToString());
            }
        }
        public void OnMessageReceived(string message)
        {
            listBox1.Items.Add(message);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
