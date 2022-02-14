using Diary.Commands;
using Diary.Models.Domains;
using Diary.Models.Wrappers;
using Diary.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    class AddEditStudentViewModel : ViewModelBase
    {
        private Repsoitory _repository = new Repsoitory();
        private StudentWrapper _student;
        private bool _isEdit;
        private int _selectedGroupId;
        private ObservableCollection<Group> _groups;
        public AddEditStudentViewModel(StudentWrapper student = null)
        {
            ConfirmCommand = new RelayCommand(Confirm);
            CloseCommand = new RelayCommand(Close);

            if (student == null)
            {
                Student = new StudentWrapper();
            }
            else
            {
                Student = student;
                IsEdit = true;
            }

            InitGroups();
        }

        public ICommand ConfirmCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public StudentWrapper Student
        {
            get { return _student; }
            set
            {
                _student = value;
                OnPropertyChanged();
            }
        }

        public int SelectedGroupId
        {
            get { return _selectedGroupId; }
            set
            {
                _selectedGroupId = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Group> Groups
        {
            get { return _groups; }
            set
            {
                _groups = value;
                OnPropertyChanged();
            }
        }

        public bool IsEdit
        {
            get { return _isEdit; }
            set
            {
                _isEdit = value;
                OnPropertyChanged();
            }
        }

        private void InitGroups()
        {
            var groups = _repository.GetGroups();
            groups.Insert(0, new Group { Id = 0, Name = "Brak" });

            Groups = new ObservableCollection<Group>(groups);

            SelectedGroupId = Student.Group.Id;
        }

        private void Confirm(object obj)
        {
            if (!Student.IsValid)
                return;

            if (IsEdit)
            {
                EditStudent();
            }
            else
                AddStudent();

            CloseWindow(obj as Window);
        }

        private void AddStudent()
        {
            _repository.AddStudent(Student);
        }

        private void EditStudent()
        {
            _repository.EditStudent(Student);
        }

        private void Close(object obj)
        {
            CloseWindow(obj as Window);
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }
    }
}
