
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
	[Activity (Label = "SignUpActivity")]			
	public class SignUpActivity : Activity
	{
		private EditText NombreUsuario;
		private EditText NumeroTelefono;
		private Button BotonRegistrar;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Init);

			NombreUsuario = FindViewById<EditText> (Resource.Id.ETNombreUsuario);
			NumeroTelefono = FindViewById<EditText> (Resource.Id.ETNumeroTelefono);
			BotonRegistrar = FindViewById<Button> (Resource.Id.BTRegistrar);

			BotonRegistrar.Click += (object sender, EventArgs e) => {
				//AddItem(NombreUsuario.Text, getDeviceID(), NumeroTelefono.Text);
			};

			// Create your application here
		}
	}
}

