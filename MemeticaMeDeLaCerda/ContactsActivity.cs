
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MemeticaMeDeLaCerda
{
	[Activity (Label = "ContactsActivity")]			
	public class ContactsActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.ContactsList);

			/* Definimos el adaptador utilizado para el ListView ContactsListView */
			var contactsAdapter = new ContactsAdapter (this);
			var contactsListView = FindViewById<ListView> (Resource.Id.ContactsListView);
			contactsListView.Adapter = contactsAdapter;

			/* Se define qué hacer cuando se presiona en un elemento de la lista de ContactsListView */
			contactsListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {

				/* Primero obtenemos la posicion de la casilla que se selecciono */
				int position = e.Position;

				/* Ahora obtenemos el nombre del contacto ubicado en la casilla presionada */
				string contactName = contactsAdapter.GetItemName(position);

				/* Iniciamos una actividad del tipo ChatActivity y le entregamos el nombre del contacto
				 * para que pueda ser mostrado en la vista del chat */
				var chat = new Intent (this, typeof(ChatActivity));
				chat.PutExtra ("ContactName", contactName);
				Tools.currentContactDeviceID = Tools.contactosUtiles[position].DeviceID;
				StartActivity (chat);
			};
		}
	}
}

