using Diary.Commands;
using Diary.Models.Domains;
using Diary.Models.Wrappers;
using Diary.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
    class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            AddStudentCommand = new RelayCommand(AddEditStudent);
            EditStudentCommand = new RelayCommand(AddEditStudent, CanEditDeleteStudent);
            DeleteStudentCommand = new AsyncRelayCommand(DeleteStudent, CanEditDeleteStudent);
            RefreshStudentCommand = new RelayCommand(RefreshStudent);
            DBConfigCommand = new RelayCommand(SetDBConfig);

            CheckDBConnection();
        }

        private Repsoitory _repository = new Repsoitory();
        private StudentWrapper _selectedStudent;
        private int _selectedGroupId;
        private ObservableCollection<StudentWrapper> _students;
        private ObservableCollection<Group> _groups;

        public ICommand RefreshStudentCommand { get; set; }
        public ICommand AddStudentCommand { get; set; }
        public ICommand EditStudentCommand { get; set; }
        public ICommand DeleteStudentCommand { get; set; }
        public ICommand DBConfigCommand { get; set; }

        public StudentWrapper SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<StudentWrapper> Students
        {
            get { return _students; }
            set
            {
                _students = value;
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

        private async Task DeleteStudent(object obj)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            var dialog = await metroWindow.ShowMessageAsync("Usunięcie ucznia", $"Czy chcesz usunąć {SelectedStudent.FirstName} {SelectedStudent.LastName}", MessageDialogStyle.AffirmativeAndNegative);

            if (dialog == MessageDialogResult.Negative)
                return;

            _repository.DeleteStudent(SelectedStudent.Id);

            RefreshDiary();
        }

        private bool CanEditDeleteStudent(object obj)
        {
            return SelectedStudent != null;
        }

        private void SetDBConfig(object obj)
        {
            var dbConfigView = new DBConfigView();
            dbConfigView.ShowDialog();
        }

        private void AddEditStudent(object obj)
        {
            var addEditStudentView = new AddEditStudentView(obj as StudentWrapper);
            addEditStudentView.Closed += AddEditStudentView_Closed;
            addEditStudentView.ShowDialog();
        }

        private async void CheckDBConnection()
        {
            using (DiaryDbContext dbContext = new DiaryDbContext())
            {
                var canConnect = dbContext.Database.Exists();

                if (!canConnect)
                {
                    var metroWindow = Application.Current.MainWindow as MetroWindow;
                    var dialog = await metroWindow.ShowMessageAsync("Błąd połączenia z bazą", $"Nie udało się połączyć z bazą, czy chcesz zmienić ustawienia?", MessageDialogStyle.AffirmativeAndNegative);
                    if (dialog == MessageDialogResult.Affirmative)
                    {
                        var dbConfigView = new DBConfigView(true);
                        dbConfigView.ShowDialog();
                    }
                    else
                        Application.Current.Shutdown();
                }
                else
                {
                    RefreshDiary();
                    InitGroups();
                }
            }
        }

        private void AddEditStudentView_Closed(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void RefreshStudent(object obj)
        {
            RefreshDiary();
        }

        private void InitGroups()
        {
            var groups = _repository.GetGroups();
            groups.Insert(0, new Group { Id = 0, Name = "Wszystkie" });

            Groups = new ObservableCollection<Group>(groups);

            SelectedGroupId = Groups[0].Id;
        }

        private void RefreshDiary()
        {
            var students = _repository.GetStudents(SelectedGroupId);
            Students = new ObservableCollection<StudentWrapper>(students);
        }
    }
}
