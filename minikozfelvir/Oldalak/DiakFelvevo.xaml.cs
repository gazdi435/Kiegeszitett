using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using minikozfelvir.Classok;
using Rgx = minikozfelvir.Classok.Regexek;

namespace minikozfelvir;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class DiakFelvevo : Window
{
    private readonly bool[] helyesek;
    private static readonly ColorAnimation anim = new() { To = Colors.Red, Duration = TimeSpan.FromMilliseconds(100), RepeatBehavior = new(10), AutoReverse = true };

    public DiakFelvevo(Diak diak, double top, double left)
    {
        InitializeComponent();

        Top = top + 70;
        Left = left + 130;

        txtOMAzonosito.Focus();
        txtOMAzonosito.SelectAll();

        DataContext = diak;

        helyesek = new[]{
            diak.OM_Azonosito != "",
            diak.Neve != "",
            diak.Email != "",
            true,
            diak.ErtesitesiCime != "",
            true,
            true
        };
    }

    #region __Inputok__
    private void OmAzonosito_Input(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Rgx.OM_PTI.IsMatch(e.Text);
    }
    private void Nev_Input(object sender, TextCompositionEventArgs e)
    {
        if(sender is TextBox tb)
            e.Handled = !Rgx.Nev_PTI.IsMatch(tb.Text + e.Text);
    }
    private void Email_Input(object sender, TextCompositionEventArgs e)
    {
        if (sender is TextBox tb)
            e.Handled = !Rgx.Email_PTI.IsMatch(tb.Text + e.Text);
    }
    private void Datum_Input(object sender, TextCompositionEventArgs e)
    {
        if (sender is TextBox tb)
            e.Handled = !Rgx.Datum_PTI.IsMatch(tb.Text + e.Text);
    }
    private void ErtesitesiCim_Input(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Rgx.Cim_PTI.IsMatch(e.Text);
    }
    private void Szam_Input(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Rgx.Szam_PTI.IsMatch(e.Text);
    }

    private void OMAzonosito_Validate(object sender, RoutedEventArgs e) => Validator<TextBox>(sender as TextBox, Rgx.OM_V);
    private void Nev_Validate(object sender, RoutedEventArgs e) => Validator<TextBox>(sender as TextBox, Rgx.Nev_V);
    private void Email_Validate(object sender, RoutedEventArgs e) => Validator<TextBox>(sender as TextBox, Rgx.Email_V);
    private void Datum_Validate(object sender, RoutedEventArgs e) => Validator<DatePickerTextBox>(sender as DatePickerTextBox, Rgx.Datum_V);
    private void ErtesitesiCim_Validate(object sender, RoutedEventArgs e) => Validator<TextBox>(sender as TextBox, Rgx.Cim_V);
    private void Szam_Validate(object sender, RoutedEventArgs e) => Validator<TextBox>(sender as TextBox, Rgx.Szam_V);

    private void Validator<T>(T? sender, Regex regex) where T : TextBox
    {
        if(sender is not null)
        {
            bool match = regex.IsMatch(sender.Text);
            if (!match && !string.IsNullOrEmpty(sender.Text))
                Animate(sender);
            helyesek[sender.TabIndex] = match;
        }
    }
    #endregion

    #region __Click__
    private void Kesz_Click(object sender, RoutedEventArgs e)
    {
        if (helyesek.All(G => G) && MessageBox.Show("Biztos, hogy ez így jó?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            Close();
        else
        {
            txtOMAzonosito.Focus();
            txtOMAzonosito.SelectAll();
        }
    }
    private void Torles_Click(object sender, RoutedEventArgs e)
    {
        txtOMAzonosito.Text = "";
        txtNev.Text = "";
        txtEmail.Text = "";
        dpSzuletesiDatum.Text = DateTime.Now.ToString();
        txtErtesitesiCim.Text = "";
        txtMatekEredmeny.Text = "0";
        txtMagyarEredmeny.Text = "0";

        txtOMAzonosito.Focus();
    }
    #endregion

    private void DragNDrop(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        if(sender is CheckBox checkBox)
        {
            if(checkBox.IsChecked is true)
            {
                txtMatekEredmeny.Text = (txtMatekEredmeny.Tag ?? 0).ToString();
                txtMatekEredmeny.Visibility = Visibility.Visible;

                txtMagyarEredmeny.Text = (txtMagyarEredmeny.Tag ?? 0).ToString();
                txtMagyarEredmeny.Visibility = Visibility.Visible;
                
                lblMagyarEredmeny.Visibility = Visibility.Visible;
                lblMatekEredmeny.Visibility = Visibility.Visible;

                grid.RowDefinitions.Add(new());
                grid.RowDefinitions.Add(new());
                Grid.SetRowSpan(stackpanel, 6);
            }
            else
            {
                txtMatekEredmeny.Tag = txtMatekEredmeny.Text;
                txtMatekEredmeny.Text = "-1";
                txtMatekEredmeny.Visibility = Visibility.Hidden;

                txtMagyarEredmeny.Tag = txtMagyarEredmeny.Text;
                txtMagyarEredmeny.Text = "-1";
                txtMagyarEredmeny.Visibility = Visibility.Hidden;
                
                lblMatekEredmeny.Visibility = Visibility.Hidden;
                lblMagyarEredmeny.Visibility = Visibility.Hidden;

                grid.RowDefinitions.RemoveRange(5, 2);
                Grid.SetRowSpan(stackpanel, 4);
            }
        }
    }

    private void Animate(TextBox sender)
    {
        SolidColorBrush originalBrush = FindResource("Sotet") as SolidColorBrush;
        Color originalColor = originalBrush?.Color ?? Colors.Transparent;

        ColorAnimation animClone = anim.Clone(); // Create a clone to avoid modifying the original animation

        Storyboard.SetTarget(animClone, sender);
        Storyboard.SetTargetProperty(animClone, new PropertyPath("(Background).(SolidColorBrush.Color)"));

        SolidColorBrush animatedBrush = new(originalColor);
        sender.Background = animatedBrush;

        Storyboard storyboard = new();
        storyboard.Children.Add(animClone);

        sender.BeginStoryboard(storyboard);
    }
}
