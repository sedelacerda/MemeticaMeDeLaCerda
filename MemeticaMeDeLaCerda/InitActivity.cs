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


namespace MemeticaMeDeLaCerda
{
	
	[Activity (MainLauncher = true, 
		Icon="@drawable/ic_launcher", Label="@string/app_name",
		Theme="@style/AppTheme")]		
	public class InitActivity : Activity
	{
		//Mobile Service Client reference
		private MobileServiceClient client;

		//Mobile Service sync table used to access data
		private IMobileServiceSyncTable<User> UserTable;

		//Adapter to map the items list to the view
		//private ToDoItemAdapter adapter;

		const string applicationURL = @"https://memeticamedelacerda.azure-mobile.net/";
		const string applicationKey = @"dRFJDonNeDsjdNiuJGZMXIVJZDIwnD44";

		const string localDbFilename = "localstore.db";

		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Init);

			CurrentPlatform.Init ();

			// Create the Mobile Service Client instance, using the provided
			// Mobile Service URL and key
			client = new MobileServiceClient (applicationURL, applicationKey);
			await InitLocalStoreAsync();

			// Get the Mobile Service sync table instance to use
			UserTable = client.GetSyncTable <User> ();

			//AddItem ("sedelacerda", getDeviceID(), "+56995333605");

			var list = await UserTable.ToListAsync();
			bool userExists = false;
			foreach (User u in list) {
				if (u.DeviceID.Equals (getDeviceID ()))
					userExists = true;
			}
			Console.WriteLine ("######################################### " + userExists + " ############################################");
		}

		private string getDeviceID ()
		{
			string android_id = Android.Provider.Settings.Secure.GetString(Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
			return android_id;
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
	}
}

