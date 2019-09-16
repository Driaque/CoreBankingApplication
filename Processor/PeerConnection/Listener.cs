using System;
using Trx.Messaging.Channels;
using Trx.Messaging.FlowControl;
using Trx.Messaging.Iso8583;

namespace Processor.PeerConnection
{
    public class Listener
    {
        private string localInterface = string.Empty; //IPAdress
        private int port = 0; //port
        public Listener()
        {
            try
            {
                //Check App.config for ip and port
                localInterface = System.Configuration.ConfigurationManager.AppSettings["AcquirerHostName"];
                port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AcquirerPort"]);
            }
            catch
            {
                //if not found, set to this
                localInterface = "127.0.0.1";
                port = 10000;
            }
        }

        public void StartListener()
        {
            TcpListener tcpListener = new TcpListener(port);
            tcpListener.LocalInterface = localInterface;
            tcpListener.Start();

            ListenerPeer listenerPeer = new ListenerPeer("Switch", new TwoBytesNboHeaderChannel(
                new Iso8583Ascii1987BinaryBitmapMessageFormatter()), tcpListener);

            //logger.Log("Phantom FEP now listening at " + localInterface + " on " + port);

            listenerPeer.Connected += new PeerConnectedEventHandler(ListenerPeerConnected);
            listenerPeer.Receive += new PeerReceiveEventHandler(ListenerPeerReceive);
            listenerPeer.Disconnected += new PeerDisconnectedEventHandler(ListenerPeerDisconnected);

            listenerPeer.Connect();
        }

        private void ListenerPeerReceive(object sender, ReceiveEventArgs e)
        {
            ListenerPeer listenerPeer = sender as ListenerPeer;
            Iso8583Message message = e.Message as Iso8583Message;

            if (message == null) { return; }

            //logger.Log("Receiving Message >>>>");

            Processor processor = new Processor();
            Iso8583Message response;
            try
            {
                processor.LogTransaction(message);

                if (message.IsReversalOrChargeBack())
                {
                    response = processor.Process(message);
                }
                else
                {
                    response = processor.Process(message);
                }

                processor.LogTransaction(response);
                //logger.Log("Sending Response >>>>");
            }
            catch (Exception ex)
            {
                message.Fields.Add(39, "06");
                message.SetResponseMessageTypeIdentifier();
                processor.LogTransaction(message);
                response = message;

                //processor.LogTransaction(response);
                //logger.Log("Error, Something went wrong somewhere");
            }

            listenerPeer.Send(response);
            listenerPeer.Close();
            listenerPeer.Dispose();

        }
        private void ListenerPeerConnected(object sender, EventArgs e)
        {
            ListenerPeer listenerPeer = sender as ListenerPeer;
            if (listenerPeer == null) return;
            //logger.Log("Listener Peer connected to " + listenerPeer.Name);
        }
        private void ListenerPeerDisconnected(object sender, EventArgs e)
        {
            ListenerPeer listenerPeer = sender as ListenerPeer;
            if (listenerPeer == null) return;
            //logger.Log("Listener Peer disconnected from " + listenerPeer.Name);

            StartListener();
        }
    }
}
