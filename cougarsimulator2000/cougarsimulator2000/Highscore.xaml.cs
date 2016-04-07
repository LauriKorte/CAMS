using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace cougarsimulator2000
{
    // Copied code from http://stackoverflow.com/questions/19374471/wpf-datagrid-create-a-datagridnumericcolumn-in-wpf
    public class DataGridNumericColumn : DataGridTextColumn
    {
        protected override object PrepareCellForEdit(System.Windows.FrameworkElement editingElement, System.Windows.RoutedEventArgs editingEventArgs)
        {
            TextBox edit = editingElement as TextBox;
            edit.PreviewTextInput += OnPreviewTextInput;

            return base.PrepareCellForEdit(editingElement, editingEventArgs);
        }

        void OnPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                Convert.ToInt32(e.Text);
            }
            catch
            {
                // Show some kind of error message if you want

                // Set handled to true
                e.Handled = true;
            }
        }
    }

    /// <summary>
    /// Interaction logic for highscore.xaml
    /// </summary>
    public partial class Highscore : Window
    {
        public Highscore()
        {
            InitializeComponent();
        }

        public void addPlayerScore(int score)
        {
            PlayerNameDialog pnd = new PlayerNameDialog();
            pnd.ShowDialog();

            string scoreStr = highScoreXML.Source.OriginalString;
            XmlDocument doc = highScoreXML.Document;
            XmlNode node = doc.SelectSingleNode("/highscores");

            XmlNode newScore = doc.CreateElement("highscore");
            XmlNode name = doc.CreateElement("nickname");
            XmlNode s = doc.CreateElement("score");
            name.InnerText = pnd.tbPlayerName.Text;

            newScore.InnerText = score.ToString();
            s.AppendChild(newScore);
            s.AppendChild(name);

            node.AppendChild(s);
            highScoreXML.Document.Save(scoreStr);
        }
    }
}
