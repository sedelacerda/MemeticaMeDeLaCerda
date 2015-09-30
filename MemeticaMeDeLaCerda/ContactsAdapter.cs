using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Provider;
using Android.Database;

namespace MemeticaMeDeLaCerda
{
	public class ContactsAdapter : BaseAdapter
	{

		/* En contactsList se almacenaran todos los contactos en clase tipo Contact */
		List<Contact> contactsList;
		Activity activity;

		public ContactsAdapter (Activity currentActivity)
		{
			this.activity = currentActivity;
			FillContacts ();
		}

		private void FillContacts ()
		{
			string[] projection = {
				ContactsContract.Contacts.InterfaceConsts.Id,
				ContactsContract.Contacts.InterfaceConsts.DisplayName,
				ContactsContract.Contacts.InterfaceConsts.PhotoId,
				ContactsContract.CommonDataKinds.Phone.Number
			};

			/* Creamos un cursor para poder recorrer todos los contactos almacenados en el telefono
			 * y ademas ordenamos la cola que se revisara alfabeticamente segun nombre de contacto */
			var cursor = activity.ManagedQuery (ContactsContract.CommonDataKinds.Phone.ContentUri, 
				projection, null, null, ContactsContract.Contacts.InterfaceConsts.DisplayName);

			contactsList = new List<Contact> ();


			if (cursor.MoveToFirst ()) {
				do {
					/* Creamos los contactos y los agregamos a la lista contactsList */
					contactsList.Add (new Contact{
						Id = cursor.GetLong (
							cursor.GetColumnIndex (projection [0])),
						DisplayName = cursor.GetString (
							cursor.GetColumnIndex (projection [1])),
						PhotoId = cursor.GetString (
							cursor.GetColumnIndex (projection [2])),
						PhoneNumber = cursor.GetString(
							cursor.GetColumnIndex (projection [3])).Replace(" ",string.Empty)
					});
				} while (cursor.MoveToNext());
			}

			//filtramos los contactos segun si calzan con los contactos de la bdd de azure

			List<Contact> aux = new List<Contact> ();

			foreach (Contact cont in contactsList) {
				bool fits = false;
				foreach (User user in Tools.contactos) {
					if (cont.PhoneNumber.Equals (user.PhoneNumber))
						fits = true;
				}
				if (fits)
					aux.Add (cont);
			}
			contactsList = aux;

			List<Contact> aux2 = new List<Contact> ();
			foreach (Contact con in contactsList)
				aux2.Add (con);
			
			Recursion:
			if (aux2.Count != contactsList.Count) {
				contactsList.Clear ();
				foreach (Contact c in aux2)
					contactsList.Add (c);
			}
			for (int i=0; i<contactsList.Count; i++) {
				for (int j=0; j<aux2.Count; j++) {
					if (contactsList[i].PhoneNumber == aux2[j].PhoneNumber && i != j) {
						aux2.RemoveAt(j);
						goto Recursion;
					}
				}
			}

			//guardamos los contactos en forma de user
			foreach (Contact cont in contactsList) {
				foreach (User user in Tools.contactos) {
					if (cont.PhoneNumber == user.PhoneNumber) {
						Tools.contactosUtiles.Add (user);
					}
				}
			}
		}

		/* Clase utilizada para representar a los contactos como objetos, posee: Id, nombre y foto de perfil */
		private class Contact
		{
			public long Id { get; set; }
			public string DisplayName{ get; set; }
			public string PhotoId { get; set; }
			public string PhoneNumber { get; set; }
		}

		/* Count retorna la cantidad de contactos almacenados en contactsList */
		public override int Count {
			get { return contactsList.Count; }
		}

		/* GetItem es un metodo escrito solo por obligacion de hacer override, no se utiliza */
		public override Java.Lang.Object GetItem(int position) {
			return null;
		}

		/* GetItemName retorna el nombre del contacto ubicado en la posicion 'position' de contactsList */
		public string GetItemName (int position) {

			if (0 <= position && position < contactsList.Count) {
				return contactsList [position].DisplayName;
			}

			/* Si la posicion esta fuera de los limites posibles se retorna null */
			else
				return null;
		}

		/* GetItemId retorna el Id del contacto ubicado en la posicion 'position' de contactsList */
		public override long GetItemId (int position) {
			return contactsList [position].Id;
		}

		/* GetView es el metodo encargado de insertar un contacto al layout Main...en otras palabras, crea y define (nombre y foto de perfil)
		 * una vista de tipo ContactListItem y la inserta al layout Main */
		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? activity.LayoutInflater.Inflate (Resource.Layout.ContactListItem, parent, false);

			/* Extraemos la vista del nombre y la vista de la imagen del contacto */
			var name = view.FindViewById<TextView> (Resource.Id.ContactName);
			var image = view.FindViewById<ImageView> (Resource.Id.ContactImage);

			/* Definimos el nombre */
			name.Text = contactsList [position].DisplayName;

			/* Si se hace click en la foto de perfil de un contacto se muestra el chat de este mismo*/
			image.Click += (object sender, EventArgs e) => {
				string contactName = name.Text;
				var chat = new Intent (activity, typeof(ChatActivity));
				chat.PutExtra ("ContactName", contactName);
				activity.StartActivity (chat);
			};

			/* Si el contacto no tiene foto de perfil se le pone una por defecto */
			if (contactsList [position].PhotoId == null) {
				image.SetImageResource (Resource.Drawable.MissingContact);
			}  

			/* Si el contacto tiene foto de perfil, entonces solo se pone*/
			else {
				var contactUri = ContentUris.WithAppendedId (ContactsContract.Contacts.ContentUri, contactsList [position].Id);
				var contactPhotoUri = Android.Net.Uri.WithAppendedPath (contactUri, Contacts.Photos.ContentDirectory);
				image.SetImageURI (contactPhotoUri);
			}

			return view;
		}
	}
}




