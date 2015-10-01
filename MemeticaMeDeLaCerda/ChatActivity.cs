using System;
using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.IO;
using Android.Telephony;
using Android.Content;
using System.Collections.Generic;
using System.Timers;

namespace MemeticaMeDeLaCerda
{
	[Activity (Label = "ChatActivity", Icon="@drawable/ContactsMenu")]			
	public class ChatActivity : Activity
	{
		//Mobile Service Client reference
		private MobileServiceClient client;

		//Mobile Service sync table used to access data
		private IMobileServiceSyncTable<Message> MessageTable;


		//Adapter to map the items list to the view
		//private ToDoItemAdapter adapter;

		const string applicationURL = @"https://memeticamedelacerda.azure-mobile.net/";
		const string applicationKey = @"dRFJDonNeDsjdNiuJGZMXIVJZDIwnD44";

		const string localDbFilename = "localstore.db";

		string contactName = "";
		string contactDeviceID = "";
		List<Message> mensajes = new List<Message> ();
		ListView TVmessages;


		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Chat);
			TVmessages = FindViewById<ListView> (Resource.Id.messagesContainer);
			/* Recibimos el parametro (nombre del contacto con que se va a chatear) */
			contactName = Intent.GetStringExtra ("ContactName") ?? "No Name";
			contactDeviceID = Tools.currentContactDeviceID;

			/* Mostramos el nombre del contacto en la parte superior de la vista */
			this.Title = contactName;

			CurrentPlatform.Init ();

			// Create the Mobile Service Client instance, using the provided
			// Mobile Service URL and key
			client = new MobileServiceClient (applicationURL, applicationKey);
			await InitLocalStoreAsync();

			// Get the Mobile Service sync table instance to use
			MessageTable = client.GetSyncTable <Message> ();
			/*
			AddItem ("agustin", "34lkndjh23jhb4kskj4", "+56992320387");
			AddItem ("pedrod", "hskjhdfg897djhsdmk3", "+56989995553");
			AddItem ("pedros", "kjndfn8s934mn4mn2n", "+56982474868");
			*/
			OnRefreshItemsSelected();

			var BTEnviar = FindViewById<Button> (Resource.Id.BTEnviar);
			var ETMessage = FindViewById<EditText> (Resource.Id.messageEdit);
			BTEnviar.Click += (object sender, EventArgs e) => {
				
				AddItem(Tools.usuario.DeviceID, contactDeviceID, ETMessage.Text);
				ETMessage.Text = "";
			};


		}

		// Refresca la lista de contactos
		private async void OnRefreshItemsSelected ()
		{
			await SyncAsync(); // get changes from the mobile service
			await RefreshItemsFromTableAsync(); // refresh view using local database
		}

		//Refresca los contactos de herramientas segun la bdd local
		private async Task RefreshItemsFromTableAsync ()
		{
			try {
				// Get the items that weren't marked as completed and add them in the adapter
				mensajes = await MessageTable.Where (item => 
					(item.Emisor == Tools.usuario.DeviceID && item.Receptor == contactDeviceID) | 
					(item.Emisor == contactDeviceID && item.Receptor == Tools.usuario.DeviceID) ).ToListAsync ();
				
				string[] textos = new string[mensajes.Count];

				for(int i=0; i<mensajes.Count; i++){
					if(mensajes[i].Emisor == Tools.usuario.DeviceID){
						textos[i] = "Yo: "+ mensajes[i].Texto;
					}
					else{
						textos[i] = contactName+": "+mensajes[i].Texto;
					}
				}

				ArrayAdapter adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1,textos);
				TVmessages.SetAdapter(adapter);

				/*
				mensajes.Clear();

				foreach (Message current in list){
					if((current.Emisor == Tools.usuario.DeviceID	&& 
						current.Receptor == contactDeviceID) || 
						(current.Receptor == Tools.usuario.DeviceID	&& 
							current.Emisor == contactDeviceID)){
						
						mensajes.Add(current);
						Console.WriteLine("###"+current.Emisor+"###");
					}
				}
				*/

			} catch (Exception e) {
				CreateAndShowDialog (e, "Error");
			}
		}

		private async Task InitLocalStoreAsync()
		{
			// new code to initialize the SQLite store
			string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), localDbFilename);

			if (!File.Exists(path))
			{
				File.Create(path).Dispose();
			}

			var store = new MobileServiceSQLiteStore(path);
			store.DefineTable<Message>();

			// Uses the default conflict handler, which fails on conflict
			// To use a different conflict handler, pass a parameter to InitializeAsync. For more details, see http://go.microsoft.com/fwlink/?LinkId=521416
			await client.SyncContext.InitializeAsync(store);
		}

		private async Task SyncAsync()
		{
			try {
				await client.SyncContext.PushAsync();
				await MessageTable.PullAsync("allMessages", MessageTable.CreateQuery()); // query ID is used for incremental sync
			} catch (Java.Net.MalformedURLException) {
				CreateAndShowDialog (new Exception ("There was an error creating the Mobile Service. Verify the URL"), "Error");
			} catch (Exception e) {
				CreateAndShowDialog (e, "Error");
			}
		}

		[Java.Interop.Export()]
		public async void AddItem (string emisor, string receptor, string texto)
		{
			if (client == null || string.IsNullOrWhiteSpace ("sdfsdf")) {
				return;
			}

			// Create a new item
			var message = new Message {
				Emisor = emisor,
				Receptor = receptor,
				Texto = texto
			};

			try {
				await MessageTable.InsertAsync(message); // insert the new item into the local database
				await SyncAsync(); // send changes to the mobile service

			} catch (Exception e) {
				CreateAndShowDialog (e, "Error");
			}
		}

		private void CreateAndShowDialog (Exception exception, String title)
		{
			CreateAndShowDialog (exception.Message, title);
		}

		private void CreateAndShowDialog (string message, string title)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (this);

			builder.SetMessage (message);
			builder.SetTitle (title);
			builder.Create ().Show ();
		}
	}
}

