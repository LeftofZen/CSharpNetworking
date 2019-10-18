using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Client
{
	public interface IClient
	{
		void Connect();
		void Disconnect();

		void Send(byte[] bytes);
	}

	public class Client //: IClient
	{
		public Client()
		{
			client = new TcpClient();
		}

		public void Connect()
		{
			_ = client.ConnectAsync(localhost, port);
			Log("[connect] connection state: " + client.Connected);
			_ = Listen();
		}

		public void Disconnect()
		{
			client.Close();
			Log("[disconnect] connection state: " + client.Connected);
		}

		public async Task Send(byte[] bytes)
		{
			var stream = client.GetStream();
			await stream.WriteAsync(bytes, 0, bytes.Length);
			await stream.FlushAsync();
		}
		public void Send(string msg)
		{
			Log("[send] " + msg);
			_ = Send(System.Text.Encoding.ASCII.GetBytes(msg));
		}

		public void Log(string msg)
		{
			NewData.Invoke(msg);
		}

		public async Task Listen()
		{
			var buffer = new byte[256];
			string data;
			Log("[listen] listening...");
			var stream = client.GetStream();
			int i;

			// Loop to receive all the data sent by the client.
			while ((i = await client.GetStream().ReadAsync(buffer, 0, buffer.Length)) != 0)
			{
				// Translate data bytes to a ASCII string.
				data = System.Text.Encoding.ASCII.GetString(buffer, 0, i);
				Log("[server_msg] " + data);

				// Process the data sent by the client.
				//data = "[server_echo] " + data.ToUpper();

				//byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

				//// Send back a response.
				//stream.Write(msg, 0, msg.Length);
			}
			Log("[listen]");

		}


		public delegate void SafeCallDelegate(string text);
		public event SafeCallDelegate NewData;

		TcpClient client;
		int port = 19191;
		IPAddress localhost = IPAddress.Parse("127.0.0.1");
	}
}
