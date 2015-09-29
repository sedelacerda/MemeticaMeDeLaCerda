package md51cb45fefca83c2f89cb4ebae101605c8;


public class ToDoActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_AddItem:(Landroid/view/View;)V:__export__\n" +
			"";
		mono.android.Runtime.register ("MemeticaMeDeLaCerda.ToDoActivity, MemeticaMeDeLaCerda, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ToDoActivity.class, __md_methods);
	}


	public ToDoActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ToDoActivity.class)
			mono.android.TypeManager.Activate ("MemeticaMeDeLaCerda.ToDoActivity, MemeticaMeDeLaCerda, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void AddItem (android.view.View p0)
	{
		n_AddItem (p0);
	}

	private native void n_AddItem (android.view.View p0);

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
