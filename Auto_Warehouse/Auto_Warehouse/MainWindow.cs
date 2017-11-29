using System;
using Gtk;
using Auto_Warehouse;
using System.Collections.Generic;


public partial class MainWindow: Gtk.Window
{
	List<StructOfProduct> _ProductsList;
	StructOfProduct _SelectProduct;

	ReadDB readDB;
	Serach serch;
	Bufer buf;

	ListStore model = new ListStore (typeof(string),typeof(string),typeof(string));
	bool ifFirstReload;

	string nameDB="Wharehouser";



	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();	   

		Initialize ();

	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}




//MY FUNCTION


	private void Initialize()
	{
		ifFirstReload = true;

		buf = new Bufer ();
		Auto_Warehouse.PasswordToBDWindow win = new PasswordToBDWindow (buf);


	}
	private void StartConfig()
	{		
		readDB = new ReadDB (nameDB, buf);
		serch = new Serach (nameDB, buf);

		GetDataFromDB ();
		


		TopLineOfTable ();

		List<string> ParametrsForSearchList = new List<string> ();

		ParametrsForSearchList.Add ("По Номеру");
		ParametrsForSearchList.Add ("По Названию");
		ParametrsForSearchList.Add ("По Параметру");

		Set_ListOfSearchParams (ParametrsForSearchList);


		label4.Visible = false;
		tb_ValueForSearch_2.Visible = false;
		bn_Search.Visible = false;
	}


	private void TopLineOfTable()
	{

		treeView_MainData.Model = model;

		treeView_MainData.AppendColumn ("  Номер   ",new CellRendererText(),"text",0);
		treeView_MainData.AppendColumn ("  Название   ",new CellRendererText(),"text",1);
		treeView_MainData.AppendColumn ("  Количество   ",new CellRendererText(),"text",2);


	}
	private void Clear_CurrentData()
	{		
		model.Clear ();
	}

	private void Set_ListOfSearchParams(List<string> ParametrsForSearchList)
	{
		foreach (string parametr in ParametrsForSearchList) 
		{
			cb_SearchParametr.AppendText (parametr);
		}
	}

	private void GetDataFromDB()
	{
		_ProductsList = readDB.Get_All_Products ();

		model.AppendValues();//Add EMPTY LINE

		for (int i = 0; i < _ProductsList.Count; i++)
		{
			model.AppendValues (_ProductsList[i].Number, _ProductsList[i].Name,_ProductsList[i].Count.ToString());
		}
	}
	private void GetDataFromDB(List<StructOfProduct> _ProductsList)
	{		
		model.AppendValues();//Add EMPTY LINE

		for (int i = 0; i < _ProductsList.Count; i++)
		{
			model.AppendValues (_ProductsList[i].Number, _ProductsList[i].Name,_ProductsList[i].Count.ToString());
		}
	}
	private void SearchData(string textForSerch,string valueParam,string bywhatSearch)
	{		
		Validation validation = new Validation ();
		serch = new Serach (nameDB,buf);

		switch(bywhatSearch)
		{
		case "По Номеру":

			if (!validation.IsEmpty (textForSerch))
			{
				Clear_CurrentData ();
				GetDataFromDB (serch.Finde_ByNumber(textForSerch));		
			}

			break;
		case "По Названию":

			if (!validation.IsEmpty (textForSerch))
			{
				Clear_CurrentData ();
				GetDataFromDB (serch.Finde_ByName (textForSerch));	
			}

			break;
		case "По Параметру":

			if (!validation.IsEmpty (textForSerch)) 
			{
				if (!validation.IsEmpty (valueParam))
				{
					Clear_CurrentData ();
					GetDataFromDB(serch.Finde_ByParametr (textForSerch,valueParam));
				}
				else
					MessageBox.Show ("Не увазано значение параметра ");
			} 

			break;
		default:

			break;
		}

	}






//ACTIONS //

	protected void OnAction1Activated (object sender, EventArgs e)
	{
		Auto_Warehouse.Change_Window win = new Change_Window (_ProductsList,_SelectProduct,nameDB,buf);
	}
	protected void OnAction2Activated (object sender, EventArgs e)
	{
		Auto_Warehouse.Add_Item_Window win = new Add_Item_Window (_ProductsList,nameDB,buf);
	}

	protected void OnAction3Activated (object sender, EventArgs e)
	{
		Auto_Warehouse.Delete_Item_Window win= new Delete_Item_Window (_ProductsList,nameDB,buf);
	}

// TOP MENU BUTTONS

	protected void OnRefreshActionActivated (object sender, EventArgs e)
	{
		if (ifFirstReload) 
		{
			StartConfig ();
			ifFirstReload = false;
		}
		else 
		{
			Clear_CurrentData ();
			GetDataFromDB ();
		}
	}
	protected void OnPasteActionActivated (object sender, EventArgs e)
	{
		Auto_Warehouse.History_Window win = new History_Window (nameDB,buf);
	}
	protected void OnDialogQuestionActionActivated (object sender, EventArgs e)
	{
		Auto_Warehouse.Help_Window win = new Help_Window ();
	}


	/////////////////////////////
	/// 
	protected void OnCbSearchParametrChanged (object sender, EventArgs e)
	{
		switch(cb_SearchParametr.ActiveText)
		{
		case "По Номеру":

			label4.Visible = false;
			tb_ValueForSearch_2.Visible = false;
			bn_Search.Visible = false;

			break;
		case "По Названию":

			label4.Visible = false;
			tb_ValueForSearch_2.Visible = false;
			bn_Search.Visible = false;
			break;
		case"По Параметру":

			label4.Visible = true;
			tb_ValueForSearch_2.Visible = true;
			bn_Search.Visible = true;

			break;
		default:

			label4.Visible = false;
			tb_ValueForSearch_2.Visible = false;
			bn_Search.Visible = false;

			break;
		}
	}

	//tb FUNCTION
	protected void OnTbValueForSearch1Changed (object sender, EventArgs e)
	{
		if(cb_SearchParametr.ActiveText!="По Параметру")
			SearchData (tb_ValueForSearch_1.Text,tb_ValueForSearch_2.Text,cb_SearchParametr.ActiveText);
	}

	//TREE VIEW FUNCTIONS

	protected void OnTreeViewMainDataRowActivated (object o, RowActivatedArgs args)
	{
		try
		{
			TreePath[] c= treeView_MainData.Selection.GetSelectedRows();

			if(c [0].Indices[0]>0)
			{
				_SelectProduct=_ProductsList[c [0].Indices[0]-1];

				Auto_Warehouse.Parametr_Window window = new Parametr_Window (_SelectProduct);
			}
		}
		catch
		{}
	}
	protected void OnTreeViewMainDataCursorChanged (object sender, EventArgs e)
	{
		TreePath[] f= treeView_MainData.Selection.GetSelectedRows();

		try
		{
			if(f [0].Indices[0]>0)
			{
				_SelectProduct=_ProductsList[f [0].Indices[0]-1];
			}
		}
		catch
		{
		}
	}























	protected void OnBnShowAllDataClicked (object sender, EventArgs e)
	{
		Clear_CurrentData ();
		GetDataFromDB ();
	}

	protected void OnBnSearchClicked (object sender, EventArgs e)
	{
		SearchData (tb_ValueForSearch_1.Text,tb_ValueForSearch_2.Text,cb_SearchParametr.ActiveText);
	}
}
