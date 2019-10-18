using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server
{
	public interface IServer
	{
		void Start();
		void Stop();
	}

	public class Server : IServer
	{
		public Server()
		{
		}

		public async Task Listen()
		{
			var listener = new TcpListener(localhost, port);
			var buffer = new byte[256];
			string data;

			Log("[listen] listener starting");
			listener.Start();

			while (shouldListen)
			{
				Log("[listen] listening for clients");

				var client = await listener.AcceptTcpClientAsync();
				Log("[listen] client connected");
				var stream = client.GetStream();
				int i;

				// Loop to receive all the data sent by the client.
				while ((i = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
				{
					// Translate data bytes to a ASCII string.
					data = System.Text.Encoding.ASCII.GetString(buffer, 0, i);
					Log(data);

					// Process the data sent by the client.
					data = "[server_echo] " + data.ToUpper();

					byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

					// Send back a response.
					stream.Write(msg, 0, msg.Length);
				}
				Log("[listen] client disconnected");
				client.Close();
			}

			listener.Stop();
		}

		bool shouldListen;


		public void Log(string msg)
		{
			NewData.Invoke(msg);
		}

		public void Start()
		{
			Log("[start]");
			shouldListen = true;
			_ = Listen();
		}

		public void Stop()
		{
			Log("[stop]");
			shouldListen = false;
		}

		int port = 19191;
		IPAddress localhost = IPAddress.Parse("127.0.0.1");

		public delegate void SafeCallDelegate(string text);
		public event SafeCallDelegate NewData;

		//List<TcpConnection> connections;

	}
}
