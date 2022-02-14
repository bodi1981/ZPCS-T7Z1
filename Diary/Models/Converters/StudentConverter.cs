using Diary.Models.Domains;
using Diary.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models.Converters
{
    public static class StudentConverter
    {
        public static StudentWrapper ToWrapper(this Student student)
        {
            return new StudentWrapper
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Comments = student.Comments,
                Group = new GroupWrapper { Id = student.Group.Id, Name = student.Group.Name },
                Maths = string.Join(", ", student.Ratings.Where(x => x.SubjectId == (int)Subject.Maths).Select(x => x.Rate)),
                Technology = string.Join(", ", student.Ratings.Where(x => x.SubjectId == (int)Subject.Technology).Select(x => x.Rate)),
                Physics = string.Join(", ", student.Ratings.Where(x => x.SubjectId == (int)Subject.Physics).Select(x => x.Rate)),
                Polish = string.Join(", ", student.Ratings.Where(x => x.SubjectId == (int)Subject.Polish).Select(x => x.Rate)),
                Foreign = string.Join(", ", student.Ratings.Where(x => x.SubjectId == (int)Subject.Foreign).Select(x => x.Rate)),
                Activities = student.Activities
            };
        }

        public static Student ToDAO(this StudentWrapper studentWrapper)
        {
            return new Student
            {
                Id = studentWrapper.Id,
                FirstName = studentWrapper.FirstName,
                LastName = studentWrapper.LastName,
                Comments = studentWrapper.Comments,
                Activities = studentWrapper.Activities,
                GroupId = studentWrapper.Group.Id
            };
        }

        public static List<Rating> ToRatingsDAO(this StudentWrapper studentWrapper)
        {
            var ratings = new List<Rating>();
            
            if (!string.IsNullOrWhiteSpace(studentWrapper.Maths))
                studentWrapper.Maths.Split(',').ToList().ForEach(x => 
                    ratings.Add(
                        new Rating { 
                            Rate = int.Parse(x), 
                            StudentId = studentWrapper.Id, 
                            SubjectId = (int)Subject.Maths 
                        }));

            if (!string.IsNullOrWhiteSpace(studentWrapper.Technology))
                studentWrapper.Technology.Split(',').ToList().ForEach(x =>
                    ratings.Add(
                        new Rating
                        {
                            Rate = int.Parse(x),
                            StudentId = studentWrapper.Id,
                            SubjectId = (int)Subject.Technology
                        }));

            if (!string.IsNullOrWhiteSpace(studentWrapper.Physics))
                studentWrapper.Physics.Split(',').ToList().ForEach(x =>
                    ratings.Add(
                        new Rating
                        {
                            Rate = int.Parse(x),
                            StudentId = studentWrapper.Id,
                            SubjectId = (int)Subject.Physics
                        }));

            if (!string.IsNullOrWhiteSpace(studentWrapper.Polish))
                studentWrapper.Polish.Split(',').ToList().ForEach(x =>
                    ratings.Add(
                        new Rating
                        {
                            Rate = int.Parse(x),
                            StudentId = studentWrapper.Id,
                            SubjectId = (int)Subject.Polish
                        }));

            if (!string.IsNullOrWhiteSpace(studentWrapper.Foreign))
                studentWrapper.Foreign.Split(',').ToList().ForEach(x =>
                    ratings.Add(
                        new Rating
                        {
                            Rate = int.Parse(x),
                            StudentId = studentWrapper.Id,
                            SubjectId = (int)Subject.Foreign
                        }));

            return ratings;
        }
    }
}
