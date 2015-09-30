package md51cb45fefca83c2f89cb4ebae101605c8;


public class MessageWrapper
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MemeticaMeDeLaCerda.MessageWrapper, MemeticaMeDeLaCerda, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MessageWrapper.class, __md_methods);
	}


	public MessageWrapper () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MessageWrapper.class)
			mono.android.TypeManager.Activate ("MemeticaMeDeLaCerda.MessageWrapper, MemeticaMeDeLaCerda, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
