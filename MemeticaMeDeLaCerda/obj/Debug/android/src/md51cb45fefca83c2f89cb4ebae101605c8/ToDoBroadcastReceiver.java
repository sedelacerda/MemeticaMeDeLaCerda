package md51cb45fefca83c2f89cb4ebae101605c8;


public class ToDoBroadcastReceiver
	extends md5214eafb7e7b3b7fcc363a68a6358563f.GcmBroadcastReceiverBase_1
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MemeticaMeDeLaCerda.ToDoBroadcastReceiver, MemeticaMeDeLaCerda, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ToDoBroadcastReceiver.class, __md_methods);
	}


	public ToDoBroadcastReceiver () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ToDoBroadcastReceiver.class)
			mono.android.TypeManager.Activate ("MemeticaMeDeLaCerda.ToDoBroadcastReceiver, MemeticaMeDeLaCerda, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
