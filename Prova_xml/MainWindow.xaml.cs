using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Xml.Linq;

namespace Prova_xml
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        CancellationTokenSource fermo;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void Btn_Aggiorna_Click(object sender, RoutedEventArgs e)
        {
            Lst_allenamenti.Items.Clear();
            Task.Factory.StartNew(() => CaricaDati());
            fermo = new CancellationTokenSource();
            Btn_Stop.IsEnabled = true;
            Btn_Aggiorna.IsEnabled = false;
            Thread.Sleep(1000);

        }

        private void CaricaDati()
        {
            

            string path = @"Allenamenti.xml";
            XDocument xmlDoc = XDocument.Load(path);
            XElement xmlallenamenti = xmlDoc.Element("allenamenti");
            var xmlallenamento = xmlallenamenti.Elements("allenamento");

            foreach (var item in xmlallenamento)
            {
                XElement xmlTipoAllenamento = item.Element("tipo");
                XElement xmlDurataAllenamento = item.Element("durata");
                XElement xmlCalorieConsumate = item.Element("calorie");
                Allenamneto a = new Allenamneto();
                a.Tipo = xmlTipoAllenamento.Value;
                a.Durata = Convert.ToDouble(xmlDurataAllenamento.Value);
                a.Calorie = Convert.ToInt32(xmlCalorieConsumate.Value);
                Dispatcher.Invoke(() => Lst_allenamenti.Items.Add(a));
              

                if(fermo.IsCancellationRequested)
                {
                    break;
                }
                Thread.Sleep(1000);

              
            }
            Dispatcher.Invoke(() =>
            {
                Btn_Aggiorna.IsEnabled = true;
                Btn_Stop.IsEnabled = false;
                fermo = null;

            }
              );


        }

        private void Btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            fermo.Cancel();
        }

        private void Btn_aggiorna_Click_1(object sender, RoutedEventArgs e)
        {
            Allenamneto a = (Allenamneto)Lst_allenamenti.SelectedItem;
            string valore = Convert.ToString(Txt_Tipo.Text);
            if(a.Tipo != valore)
            {
                a.Tipo = valore;
                MessageBox.Show("Aggiornato");
            }
            Task.Factory.StartNew(scrivi);
        }

        private void Lst_allenamenti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Allenamneto a = (Allenamneto)Lst_allenamenti.SelectedItem;
            if(a!=null)
            {
                Lbl_Durata.Content = a.Durata.ToString();
                Txt_Tipo.Text = a.Tipo.ToString();

            }
        }

        private void scrivi()
        {
            string path = @"Aggiornamenti.xml";

            XElement xmlallenamenti = new XElement("allenamenti");

            foreach (Allenamneto allenamento in Lst_allenamenti.Items)
            {
                XElement xmlallenamento= new XElement("allenamento");
                XElement xmltipo = new XElement("tipo", allenamento.Tipo);
                XElement xmldurata = new XElement("durata", allenamento.Durata);
                XElement xmlcalorie = new XElement("calorie", allenamento.Calorie);
                xmlallenamento.Add(xmltipo);
                xmlallenamento.Add(xmldurata);
                xmlallenamento.Add(xmlcalorie);
                xmlallenamento.Add(xmlallenamento);

            }


            xmlallenamenti.Save(path);
        }
    }
}
