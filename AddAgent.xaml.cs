using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Zadanie1
{
    public partial class AddAgent : Window
    {
        private Agents currentAgent = new();

        public AddAgent(Agents? agent)
        {
            InitializeComponent();

            Pleasant_rustle_DBEntities db = new();

            List<FilterModel> filterList = db.Type_Agents.ToArray().Select(x => new FilterModel(x.ID, x.type)).OrderBy(x => x.ID_Type_agent).ToList();

            AgentType.ItemsSource = filterList;
            currentAgent.ID_Type_Agents = filterList[0].ID_Type_agent;
            if (agent is not null)
            {
                AgentType.SelectedIndex = agent.ID_Type_Agents - 1;
                img_Product.Source = new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory, @"../../" + agent.logo)));
                currentAgent = agent;
            }

            else
                AgentType.SelectedIndex = 0;

            DataContext = currentAgent;
            db.Dispose();
        }

        private void AddAgentButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new();

            if (string.IsNullOrWhiteSpace(currentAgent.name))
                errors.AppendLine("Укажите имя агента");

            if (string.IsNullOrWhiteSpace(currentAgent.phone))
                errors.AppendLine("Укажите номер телефона");

            if (string.IsNullOrWhiteSpace(currentAgent.email))
                errors.AppendLine("Выберите категорию");

            if (string.IsNullOrWhiteSpace(currentAgent.adress))
                errors.AppendLine("Введите адрес");

            if (currentAgent.priority == 0)
                errors.AppendLine("Введите приоритет");

            if (string.IsNullOrWhiteSpace(currentAgent.director))
                errors.AppendLine("Укажите директора");

            if (string.IsNullOrWhiteSpace(currentAgent.INN))
                errors.AppendLine("Укажите ИНН");

            if (string.IsNullOrWhiteSpace(currentAgent.KPP))
                errors.AppendLine("Укажите КПП");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            Pleasant_rustle_DBEntities db = new();

            if (currentAgent.ID_Agent == 0)
            {
                try
                {
                    if (img_Product.Source is null)
                    {
                        currentAgent.logo = "picture.png";
                    }

                    currentAgent.ID_Agent = db.Agents.ToArray().Last().ID_Agent + 1;

                    db.Agents.Add(currentAgent);
                    db.SaveChanges();
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                // TAKS/TODO: Доделать редактирование(чтобы обновлялось в базе, после закрытия подтягивались новые записи)

                var agent = db.Agents.Where(p => p.ID_Agent == currentAgent.ID_Agent).FirstOrDefault();
                agent = currentAgent;
                db.SaveChanges();
                Close();
            }

            db.Agents.Add(currentAgent);
        }

        private void AgentType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            FilterModel item = AgentType.SelectedItem as FilterModel;
            currentAgent.ID_Type_Agents = item.ID_Type_agent;
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new();
            dlg.InitialDirectory = "";
            dlg.Filter = "Image files (*.jpg,*.png,*.bmp)|*.jpg;*.png;*.bmp|All Files (*.*)|*.*";

            string randomString = GenerateRandomString();

            if (dlg.ShowDialog() == true)
            {
                string selectedFileName = dlg.FileName;
                string fileExtension = GetFileExtension(selectedFileName);
                string imageName = $"agent_{randomString}.{fileExtension}";

                string destFile = Path.Combine(Environment.CurrentDirectory, @"../../storage", imageName);

                img_Product.Source = new BitmapImage(new Uri(selectedFileName));

                File.Copy(selectedFileName, destFile);

                currentAgent.logo = "storage/" + imageName;

                MessageBox.Show("Картинка продукта добавлена");
            }
        }

        public static string GetFileExtension(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            return extension.StartsWith(".") ? extension.Substring(1) : extension;
        }

        private String GenerateRandomString()
        {
            Random randomChar = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, 6).Select(s => s[randomChar.Next(s.Length)]).ToArray());
        }
    }
}
