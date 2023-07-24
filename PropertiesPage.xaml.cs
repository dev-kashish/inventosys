using InventoryMangementSystem.Class;
using System.Windows;
using System.Windows.Controls;

namespace InventoryMangementSystem
{
    /// <summary>
    /// Interaction logic for PropertiesPage.xaml
    /// </summary>
    public partial class PropertiesPage : Page
    {
        public PropertiesPage()
        {
            InitializeComponent();
        }

        private void changePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            newPasswordText.Password = newPasswordText.Password.Trim();
            oldPasswordText.Password = oldPasswordText.Password.Trim();
            confirmPasswordText.Password = confirmPasswordText.Password.Trim();
            if (oldPasswordText.Password != string.Empty && newPasswordText.Password == confirmPasswordText.Password && newPasswordText.Password != string.Empty && confirmPasswordText.Password != string.Empty)
            {
                UserValidation userValidation = new UserValidation();
                if (userValidation.validatePassword(oldPasswordText.Password))
                {
                    userValidation.changePassword(newPasswordText.Password);
                    oldPasswordText.Password = string.Empty;
                    newPasswordText.Password = string.Empty;
                    confirmPasswordText.Password = string.Empty;
                    changePassword.IsOpen = false;
                }
                else
                {
                    oldPasswordText.Password = string.Empty;
                    newPasswordText.Password = string.Empty;
                    confirmPasswordText.Password = string.Empty;
                    MessageBox.Show("Invalid Password!");
                }
            }
            else if (newPasswordText.Password != confirmPasswordText.Password)
            {
                MessageBox.Show("New Password and Confirmation Password do not match!");
            }
            else
            {
                MessageBox.Show("All fields are required!");
            }
        }

        private void changeUsernameBtn_Click(object sender, RoutedEventArgs e)
        {
            oldUsernameText.Text = oldUsernameText.Text.Trim();
            newUsernameText.Text = newUsernameText.Text.Trim();
            PasswordText.Password = PasswordText.Password.Trim();
            if (newUsernameText.Text != string.Empty && oldUsernameText.Text != string.Empty && PasswordText.Password != string.Empty)
            {
                UserValidation userValidation = new UserValidation();
                if (userValidation.validateUser(oldUsernameText.Text, PasswordText.Password))
                {
                    userValidation.changeUsername(newUsernameText.Text);
                    oldUsernameText.Text = string.Empty;
                    newUsernameText.Text = string.Empty;
                    PasswordText.Password = string.Empty;
                    changeUsername.IsOpen = false;
                }
                else
                {
                    oldUsernameText.Text = string.Empty;
                    newUsernameText.Text = string.Empty;
                    PasswordText.Password = string.Empty;
                    MessageBox.Show("Invalid Username or Password!");
                }
            }
            else
            {
                MessageBox.Show("Allow fields are required!");
            }
        }

        private void changePassPopBtn_Click(object sender, RoutedEventArgs e)
        {
            changePassword.IsOpen = true;
        }

        private void changeUserPopBtn_Click(object sender, RoutedEventArgs e)
        {
            changeUsername.IsOpen = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppAbout.Text = "InventoSys is a simple inventory management system which is made with the objective of providing the user with a simple interface to keep track of the electronic devices used in various labs and classrooms of Computer Department. InventoSys is developed with sole aim to digitize the process of keeping record of all the equipment acquired by the Computer Department. However, it also allows the user to understand the current inventory condition on the basis of various parameters such as Categories, Assigned To and Status of all the items. \n\nSignificant emphasis was also given to security by ensuring that no unauthorized user can login and access the application. The security of the application is hardened by the attempt to apply the concept of 'zero knowledge' into the application. This is done by saving the user credentials (username & password) as hash values using the SHA256 Hashing algorithm. This makes sure that even if the user data is leaked into the public domain, the true values of the credentials cannot be accessed.\n\nThe Dashboard Page allows the user to have a more condensed and graphical view of the current inventory categorized on various basis such as Category, Status etc. This allows the user to understand the current cicumstances and identify any issues that need to be covered.\n\nThe Overview Page provides the user a detailed view of every single item and all its parameters such as category, status, assigned to etc along with the ability to edit each item's parameters as well as delete any item. New items can also be added with the help of the 'Add Item' button on the top right where the user may choose from existing values or enter new values ot the various parameters of the item. The user may also filter the contents of the data grid by entering the filter text into the search boxes in the top bar of the main area, which filters out the data in realtime. The user can then export this filtered data in the form of a PDF Report.\n\nThis project is made using the '.Net Framework 6 LTS' and 'Windows Presentation Foundation (WPF)' UI Framework. The UI is designed using Extensible Application Markup Language (XAML) and the backend logic is programed using C#. The database is implemented using Microsoft SQL Express (Local DB).";

            DevAbout.Text = "InventoSys is our first ever application development experience. This project provided us with an oppurtunity to dive deeper into some real world development. We tried to grab as much exposure and experience from this oppurtunity as possible by utilizing modern and production ready frameworks and technologies. We gave this project the best shot possible, pushing the boundaries of our knowledge and experience to our greatest capabilities.";
        }
    }
}
