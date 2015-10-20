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


namespace MemeticaMeDeLaCerda
{
	
	[Activity (MainLauncher = true, 
		Icon="@drawable/ContactsMenu", Label="@string/app_name",
		Theme="@style/AppTheme")]		
	public class InitActivity : Activity
	{
		//Mobile Service Client reference
		private MobileServiceClient client;

		//Mobile Service sync table used to access data
		private IMobileServiceSyncTable<User> UserTable;

		//Adapter to map the items list to the view
		//private ToDoItemAdapter adapter;
		//#############################################################################
		const string applicationURL = @"https://memeticamedelacerda.azure-mobile.net/";
		const string applicationKey = @"itRtJEADXzxEJleWjnHnmaIQGXZPsI38";

		const string localDbFilename = "localstore.db";


		private TextView TVMemeticame;
		private TextView TVNombreUsuario;
		private TextView TVNumeroTelefono;
		private EditText ETNombreUsuario;
		private EditText ETNumeroTelefono;
		private Button BTRegistrar;


		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Init);

			TVMemeticame = FindViewById<TextView> (Resource.Id.TVMemeticame);
			TVNombreUsuario = FindViewById<TextView> (Resource.Id.TVNombreUsuario);
			TVNombreUsuario.Visibility = ViewStates.Gone;
			TVNumeroTelefono = FindViewById<TextView> (Resource.Id.TVNumeroTelefono);
			TVNumeroTelefono.Visibility = ViewStates.Gone;
			ETNombreUsuario = FindViewById<EditText> (Resource.Id.ETNombreUsuario);
			ETNombreUsuario.Visibility = ViewStates.Gone;
			ETNumeroTelefono = FindViewById<EditText> (Resource.Id.ETNumeroTelefono);
			ETNumeroTelefono.Visibility = ViewStates.Gone;
			BTRegistrar = FindViewById<Button> (Resource.Id.BTRegistrar);
			BTRegistrar.Visibility = ViewStates.Gone;


			BTRegistrar.Click += (object sender, EventArgs e) => {
				Tools.usuario.Username = ETNombreUsuario.Text;
				Tools.usuario.PhoneNumber = ETNumeroTelefono.Text;
				Tools.usuario.DeviceID = getDeviceID();
				AddItem(Tools.usuario.Username, Tools.usuario.DeviceID, Tools.usuario.PhoneNumber);
				StartActivity(typeof(ContactsActivity));
			};


			CurrentPlatform.Init ();

			// Create the Mobile Service Client instance, using the provided
			// Mobile Service URL and key
			client = new MobileServiceClient (applicationURL, applicationKey);
			await InitLocalStoreAsync();

			// Get the Mobile Service sync table instance to use
			UserTable = client.GetSyncTable <User> ();

			/*
			AddItem ("agustin", "34lkndjh23jhb4kskj4", "+56992320387");
			AddItem ("pedrod", "hskjhdfg897djhsdmk3", "+56989995553");
			AddItem ("pedros", "kjndfn8s934mn4mn2n", "+56982474868");
			*/

			OnRefreshItemsSelected();

		}

		private string getDeviceID ()
		{
			string android_id = Android.Provider.Settings.Secure.GetString(Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
			return android_id;
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
				var list = await UserTable.ToListAsync ();
				Tools.contactos.Clear();

				bool userExists = false;
				foreach (User current in list){
					if(current.DeviceID != getDeviceID()){
						Tools.contactos.Add(current);
						Console.WriteLine("###"+current.PhoneNumber+"###");
					}
					else{
						Tools.usuario.Username = current.Username;
						Tools.usuario.DeviceID = getDeviceID();
						Tools.usuario.PhoneNumber = current.PhoneNumber;
						userExists = true;
					}
				}

				if(userExists){
					StartActivity(typeof(ContactsActivity));
				}
				else{
					showSignUpScreen();
				}


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
			store.DefineTable<User>();

			// Uses the default conflict handler, which fails on conflict
			// To use a different conflict handler, pass a parameter to InitializeAsync. For more details, see http://go.microsoft.com/fwlink/?LinkId=521416
			await client.SyncContext.InitializeAsync(store);
		}

		private async Task SyncAsync()
		{
			try {
				await client.SyncContext.PushAsync();
				await UserTable.PullAsync("allUsers", UserTable.CreateQuery()); // query ID is used for incremental sync
			} catch (Java.Net.MalformedURLException) {
				CreateAndShowDialog (new Exception ("There was an error creating the Mobile Service. Verify the URL"), "Error");
			} catch (Exception e) {
				CreateAndShowDialog (e, "Error");
			}
		}

		[Java.Interop.Export()]
		public async void AddItem (string username, string deviceid, string phonenumber)
		{
			if (client == null || string.IsNullOrWhiteSpace ("sdfsdf")) {
				return;
			}

			// Create a new item
			var user = new User {
				Username = username,
				DeviceID = deviceid,
				PhoneNumber = phonenumber
			};

			try {
				await UserTable.InsertAsync(user); // insert the new item into the local database
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

		private void showSignUpScreen(){
			
			TVMemeticame.Visibility = ViewStates.Gone;
			TVNombreUsuario.Visibility = ViewStates.Visible;
			TVNumeroTelefono.Visibility = ViewStates.Visible;
			ETNombreUsuario.Visibility = ViewStates.Visible;
			ETNumeroTelefono.Visibility = ViewStates.Visible;
			BTRegistrar.Visibility = ViewStates.Visible;

			Console.WriteLine ("Todo Bien");
		}
	}
}

