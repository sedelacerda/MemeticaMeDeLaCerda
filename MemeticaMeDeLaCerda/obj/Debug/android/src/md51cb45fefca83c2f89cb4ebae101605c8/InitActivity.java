package md51cb45fefca83c2f89cb4ebae101605c8;


public class InitActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_AddItem:(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;)V:__export__\n" +
			"";
		mono.android.Runtime.register ("MemeticaMeDeLaCerda.InitActivity, MemeticaMeDeLaCerda, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", InitActivity.class, __md_methods);
	}


	public InitActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == InitActivity.class)
			mono.android.TypeManager.Activate ("MemeticaMeDeLaCerda.InitActivity, MemeticaMeDeLaCerda, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void AddItem (java.lang.String p0, java.lang.String p1, java.lang.String p2)
	{
		n_AddItem (p0, p1, p2);
	}

	private native void n_AddItem (java.lang.String p0, java.lang.String p1, java.lang.String p2);

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
