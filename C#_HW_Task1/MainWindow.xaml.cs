using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StudentManager
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Student[] _students = Array.Empty<Student>();
        private int _nextId = 1;

        public Student[] Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ClearText(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "Enter your Name..." || textBox.Text == "Enter your Age..." || textBox.Text == "Enter your Address...")
                {
                    textBox.Text = string.Empty;
                }
            }
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtAge.Text, out int age))
            {
                MessageBox.Show("Please enter a valid age.");
                return;
            }

            Student newStudent = new Student
            {
                Id = _nextId++,
                Name = txtName.Text,
                Age = age,
                Address = txtAddress.Text
            };

            Student[] newArray = new Student[Students.Length + 1];
            Array.Copy(Students, newArray, Students.Length);
            newArray[^1] = newStudent;
            Students = newArray;

            // Reset text boxes with placeholder text
            txtName.Text = "Enter your Name...";
            txtAge.Text = "Enter your Age...";
            txtAddress.Text = "Enter your Address...";
        }

        private void SearchByName_Click(object sender, RoutedEventArgs e)
        {
            string searchName = txtSearchName.Text;
            var matches = Array.FindAll(Students, s => s.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase));
            ShowSearchResults(matches, "name", searchName);

            // Clear the search text box
            txtSearchName.Text = "";
        }

        private void SearchByAge_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtSearchAge.Text, out int age))
            {
                MessageBox.Show("Please enter a valid age.");
                return;
            }

            var matches = Array.FindAll(Students, s => s.Age == age);
            ShowSearchResults(matches, "age", age.ToString());

            // Clear the search text box
            txtSearchAge.Text = "";
        }

        private void ShowSearchResults(Student[] matches, string searchType, string searchTerm)
        {
            if (matches.Length == 0)
            {
                MessageBox.Show($"No students found with {searchType} '{searchTerm}'.");
                return;
            }

            string message = $"Found {matches.Length} student(s) with {searchType} '{searchTerm}':\n";
            message += string.Join("\n", matches.Select(s => $"{s.Name}, Age: {s.Age}, Address: {s.Address}"));
            MessageBox.Show(message);
        }

        private void RemoveByName_Click(object sender, RoutedEventArgs e)
        {
            string nameToRemove = txtRemoveName.Text;
            var remaining = Array.FindAll(Students, s => !s.Name.Equals(nameToRemove, StringComparison.OrdinalIgnoreCase));
            UpdateStudentsAfterRemoval(remaining, "name", nameToRemove);

            // Clear the remove text box
            txtRemoveName.Text = "";
        }

        private void RemoveByAge_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtRemoveAge.Text, out int age))
            {
                MessageBox.Show("Please enter a valid age.");
                return;
            }

            var remaining = Array.FindAll(Students, s => s.Age != age);
            UpdateStudentsAfterRemoval(remaining, "age", age.ToString());

            // Clear the remove text box
            txtRemoveAge.Text = "";
        }

        private void UpdateStudentsAfterRemoval(Student[] remaining, string removalType, string removalTerm)
        {
            int removedCount = Students.Length - remaining.Length;
            if (removedCount == 0)
            {
                MessageBox.Show($"No students found with {removalType} '{removalTerm}'.");
                return;
            }

            Students = remaining;
            MessageBox.Show($"Removed {removedCount} student(s) with {removalType} '{removalTerm}'.");
        }

        private void SortByAge_Click(object sender, RoutedEventArgs e)
        {
            Student[] sorted = Students.ToArray();
            Array.Sort(sorted, (a, b) => a.Age.CompareTo(b.Age));
            Students = sorted;
        }

        private void SortByName_Click(object sender, RoutedEventArgs e)
        {
            Student[] sorted = Students.ToArray();
            Array.Sort(sorted, (a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
            Students = sorted;
        }
    }

    public struct Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
    }
}