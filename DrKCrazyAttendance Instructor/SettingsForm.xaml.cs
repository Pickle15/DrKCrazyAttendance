﻿using DrKCrazyAttendance;
using DrKCrazyAttendance.Properties;
using DrKCrazyAttendance.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace DrKCrazyAttendance_Instructor
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsForm : Window
    {
        private bool locked = true;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!locked)
            {
                Settings.Default.Encrypted = false;
                Settings.Default.SqlServerAddr = txtDbServerAddr.Text;
                Settings.Default.SqlDatabase = txtDatabase.Text;
                Settings.Default.SqlUsername = txtDbUsername.Text;
                if (!string.IsNullOrWhiteSpace(txtDbPassword.Password))
                    Settings.Default.SqlPassword = txtDbPassword.Password;
                else
                    Settings.Default.SqlPassword = SecurityCrypt.AES_Decrypt(Settings.Default.SqlPassword);
                
                Settings.Default.Save();

                MainWindow.Instance.LoadCourses();

                Close();
                
            }
            else
            {
                MessageBox.Show("Settings are locked and can not be saved.");
            }
        }

        private void btnUnlock_Click(object sender, RoutedEventArgs e)
        {
            if (txtPin.Password.Equals(Settings.Default.SecurityPin))
            {
                locked = false;
                txtDatabase.IsEnabled = true;
                txtDbPassword.IsEnabled = true;
                txtDbUsername.IsEnabled = true;
                txtDbServerAddr.IsEnabled = true;

                txtDbServerAddr.Text = SecurityCrypt.AES_Decrypt(Settings.Default.SqlServerAddr);
                txtDatabase.Text = SecurityCrypt.AES_Decrypt(Settings.Default.SqlDatabase);
                txtDbUsername.Text = SecurityCrypt.AES_Decrypt(Settings.Default.SqlUsername);

                txtPin.IsEnabled = false;
                btnUnlock.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("The security pin you entered doesn't match.");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.Instance.Show();
        }
     
    }
}