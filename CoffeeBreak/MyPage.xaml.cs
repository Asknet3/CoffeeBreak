using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;

namespace CoffeeBreak
{
	public partial class MyPage : ContentPage
	{
		static Entry uName = new Entry ();
		static Label lbl_message = new Label();
		static Button btn_saveName = new Button ();
		static StackLayout stackLayout = new StackLayout ();



		public MyPage ()
		{
			InitializeComponent ();

			// Verifico che esista il nome utente. Se esiste, nascondo il campo e vado avanti.
			var db = DependencyService.Get<ISQLite> ().GetConnection ();


			// Verifico che esista la tabella User. Se non esiste, la creo.
			if (!Utility.TableExists<User> (db)) 
				db.CreateTable<User> ();


			// Verifico che esista un utente nel DB. Se non esiste, mostro il campo di inserimento testo 
			// per immettere il nome e il relativo bottone Salva.
			if (Utility.GetUser(db) == null) 
			{	
				uName.Placeholder="Inserisci il tuo nome...";
				uName.TextColor = Color.Black;
				uName.BackgroundColor = Color.Gray;

				btn_saveName.Text = "Save";
				btn_saveName.Clicked += (object sender, EventArgs e) => {
					Utility.SaveUser(db, uName.Text);
				};

				stackLayout.Children.Add (uName);
				stackLayout.Children.Add (btn_saveName);


				// ..........  VAI alla pagina di ricerca Beacon  ................
			}

			stackLayout.Children.Add (lbl_message);
			//stackLayout.Children.Add (myText);

			this.Content = stackLayout;
		}

		public static void SetText(string s,  Color c){
			lbl_message.FontSize = 25;
			lbl_message.Text = s;
			ChangeBackground (c);
		}
			


		public static void ChangeBackground(Color c){
			stackLayout.BackgroundColor = c;
		}

	}
}

