using Diary.Models.Domains;
using Diary.Models.Wrappers;
using Diary.Models.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Diary.Models;
using MoreLinq;

namespace Diary
{
    public class Repsoitory
    {
        public List<Group> GetGroups()
        {
            using(var dbContext = new DiaryDbContext())
            {
                return dbContext.Groups.ToList();
            }
        }

        public List<StudentWrapper> GetStudents(int groupId)
        {
            using (var dbContext = new DiaryDbContext())
            {
                var students = dbContext
                    .Students
                    .Include(x => x.Group)
                    .Include(x => x.Ratings);

                if (groupId != 0)
                {
                    students = students.Where(x => x.GroupId == groupId);
                }

                return students.ToList().Select(x => x.ToWrapper()).ToList();
            }
        }

        public void DeleteStudent(int studentId)
        {
            using (var dbContext = new DiaryDbContext())
            {
                var student = dbContext.Students.Where(x => x.Id == studentId).FirstOrDefault();
                dbContext.Students.Remove(student);
                dbContext.SaveChanges();
            }
        }

        public void EditStudent(StudentWrapper student)
        {
            using (var dbContext = new DiaryDbContext())
            {
                var editedStudent = student.ToDAO();
                UpdateStudentProperies(editedStudent, dbContext);

                var editedRatings = student.ToRatingsDAO();
                var dbRatings = dbContext.Ratings.Where(x => x.StudentId == editedStudent.Id).ToList();

                //UpdateRate(Subject.Maths, editedStudent, dbRatings, editedRatings, dbContext);
                //UpdateRate(Subject.Technology, editedStudent, dbRatings, editedRatings, dbContext);
                //UpdateRate(Subject.Physics, editedStudent, dbRatings, editedRatings, dbContext);
                //UpdateRate(Subject.Polish, editedStudent, dbRatings, editedRatings, dbContext);
                //UpdateRate(Subject.Foreign, editedStudent, dbRatings, editedRatings, dbContext);

                UpdateRates(editedStudent, dbRatings, editedRatings, dbContext);

                dbContext.SaveChanges();
            }
        }

        private static void UpdateStudentProperies(Student editedStudent, DiaryDbContext dbContext)
        {
            var dbStudent = dbContext.Students.Where(x => x.Id == editedStudent.Id).FirstOrDefault();

            dbStudent.FirstName = editedStudent.FirstName;
            dbStudent.LastName = editedStudent.LastName;
            dbStudent.Comments = editedStudent.Comments;
            dbStudent.Activities = editedStudent.Activities;
            dbStudent.GroupId = editedStudent.GroupId;
        }

        private static void UpdateRate(Subject subject, Student editedStudent, List<Rating> dbRatings, List<Rating> editedRatings, DiaryDbContext dbContext)
        {
            var dbMathsRatings = dbRatings.Where(x => x.SubjectId == (int)subject).Select(x => x.Rate).ToList();
            var editedMathsRatings = editedRatings.Where(x => x.SubjectId == (int)subject).Select(x => x.Rate).ToList();

            var subjectRatingsToDelete = dbMathsRatings.Except(editedMathsRatings).ToList();
            var subjectRatingsToAdd = editedMathsRatings.Except(dbMathsRatings).ToList();

            subjectRatingsToDelete.ForEach(x =>
            {
                var ratingToDelete = dbContext.Ratings.First(y => y.Rate == x && y.StudentId == editedStudent.Id && y.SubjectId == (int)subject);
                dbContext.Ratings.Remove(ratingToDelete);
            });

            subjectRatingsToAdd.ForEach(x =>
            {
                var ratingToAdd = new Rating
                {
                    Rate = x,
                    StudentId = editedStudent.Id,
                    SubjectId = (int)subject
                };
                dbContext.Ratings.Add(ratingToAdd);
            });
        }

        private static void UpdateRates(Student editedStudent, List<Rating> dbRatings, List<Rating> editedRatings, DiaryDbContext dbContext)
        {
            var ratingsToDelete = dbRatings.ExceptBy(
                editedRatings,
                x => new
                {
                    x.Rate,
                    x.StudentId,
                    x.SubjectId
                }).ToList();

            var ratingsToAdd = editedRatings.ExceptBy(
                dbRatings,
                x => new
                {
                    x.Rate,
                    x.StudentId,
                    x.SubjectId
                }).ToList();

            ratingsToDelete.ForEach(x =>
            {
                var ratingToDelete = dbContext.Ratings.First(y => y.Rate == x.Rate && y.StudentId == x.StudentId && y.SubjectId == x.SubjectId);
                dbContext.Ratings.Remove(ratingToDelete);
            });

            ratingsToAdd.ForEach(x =>
            {
                var ratingToAdd = new Rating
                {
                    Rate = x.Rate,
                    StudentId = x.StudentId,
                    SubjectId = x.SubjectId
                };
                dbContext.Ratings.Add(ratingToAdd);
            });
        }

        public void AddStudent(StudentWrapper student)
        {
            using (var dbContext = new DiaryDbContext())
            {
                var newStudent = student.ToDAO();
                var ratings = student.ToRatingsDAO();

                var dbStudent = dbContext.Students.Add(newStudent);
                ratings.ForEach(x =>
                {
                    x.StudentId = dbStudent.Id;
                    dbContext.Ratings.Add(x);
                });
                
                dbContext.SaveChanges();
            }
        }
    }
}
