using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LABB3_Bordsbokning_Neliz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bokare bokning = new Bokare();
        public MainWindow()
        {
            //anropa metod för att fylla på txt fil OM filen är tom, för att undvika dubletter
            InitializeComponent();
            bokning.FixaTomFil();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private async void Boka_Click(object sender, RoutedEventArgs e)
        {
            //lite felhantering
            if (datePick.SelectedDate != null && Tid.Text != "" && bordTyp.Text != "" && inputName.Text != "")
            {
                var input = $"|{datePick.SelectedDate.Value.Date.ToShortDateString()} {Tid.Text.ToString()} {bordTyp.Text.ToString()}|{inputName.Text.ToString()}";
                var bokningsförfrågan = new string[] { datePick.SelectedDate.Value.Date.ToShortDateString(), Tid.Text.ToString(), bordTyp.Text.ToString(), inputName.Text.ToString() };
                await bokning.BokningAsync(bokningsförfrågan, input);
                datePick.SelectedDate = null;
                Tid.Text = "";
                bordTyp.Text = "";
                inputName.Text = "";
            }
            else
            {
                MessageBox.Show("Vänligen fyll först i alla uppgifter !");
            }
        }
        private void VisaBokningar_Click(object sender, RoutedEventArgs e)
        {
           
            listbox.Items.Clear();
            bokning.LäsFrånFil(listbox);
            if (listbox.Items.Count == 0)
            {
                MessageBox.Show("Det finns inga bokade tider!");
            }
        }
        private void Avboka_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string input = listbox.SelectedItem.ToString();
                bokning.TaBortFrånFil(input);
                listbox.Items.Remove(listbox.SelectedItem);
                MessageBox.Show("Avbokningen har genomförts!");
            }
            catch
            {
                MessageBox.Show("Välj först en bokning ur listan som ska avbokas!");
            }
        }
        private void listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void VisaBokningar_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {

        }
    }
}
