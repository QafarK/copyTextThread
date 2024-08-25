using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Win32;
namespace copyTextThread;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.Filter = "Text files (*.txt)|*.txt";

        if (dialog.ShowDialog() == true)
            txtbox1.Text = dialog.FileName;
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.Filter = "Text files (*.txt)|*.txt";

        if (dialog.ShowDialog() == true)
            txtbox2.Text = dialog.FileName;
    }

    private void Button_Click_Copy(object sender, RoutedEventArgs e)
    {
        if (File.Exists(txtbox1.Text))
        {
            string? fromFileName = txtbox1.Text;
            string? toFileName = txtbox2.Text;
            FileStream? file = new FileStream(fromFileName, FileMode.Open, FileAccess.Read);
            byte[] bytes = new byte[1];

            long len = 4096;
            if (file.Length < len)
                len = file.Length;

            prgBar.Maximum = len;

            if (!File.Exists(txtbox2.Text))
            {
                new Thread(() =>
                {
                    try
                    {
                        using FileStream? fileWrite = new FileStream(toFileName, FileMode.Append, FileAccess.Write);
                        using StreamWriter writer = new StreamWriter(fileWrite);
                        for (int i = 0; i < len; i++)
                        {
                            file.Read(bytes, 0, 1);
                            string text = Encoding.UTF8.GetString(bytes);
                            writer.Write(text);
                            writer.Flush();
                            prgBar.Dispatcher.Invoke(() => prgBar.Value = i + 1);
                            Thread.Sleep(10);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        file?.Dispose();
                        MessageBox.Show("Operation ended !");
                    }
                }).Start();
            }
            else
            {
                new Thread(() =>
                {
                    try
                    {
                        using FileStream? fileWrite = new FileStream(toFileName, FileMode.Create, FileAccess.Write);
                        using StreamWriter writer = new StreamWriter(fileWrite);
                        for (int i = 0; i < len; i++)
                        {
                            file.Read(bytes, 0, 1);
                            string text = Encoding.UTF8.GetString(bytes);
                            writer.Write(text);
                            writer.Flush();
                            prgBar.Dispatcher.Invoke(() => prgBar.Value = i + 1);
                            Thread.Sleep(10);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        file?.Dispose();
                        MessageBox.Show("Operation ended !");
                    }
                }).Start();
            }
        }
    }
}