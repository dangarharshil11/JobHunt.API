using FluentAssertions;
using JobHunt.Services.JobSeekerAPI.Data;
using JobHunt.Services.JobSeekerAPI.Models;
using JobHunt.Services.JobSeekerAPI.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections;

namespace JobHunt.Services.JobSeekerAPI.Tests
{
    [TestFixture]
    public class QualificationRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> options;

        private Qualification _qualification1;
        private Qualification _updatedQualification1;
        private Qualification _qualification2;

        public QualificationRepositoryTests()
        {
            _qualification1 = new Qualification()
            {
                Id = new Guid("F32A812A-4190-4F00-3A8F-08DC3E97F46E"),
                UserId = new Guid("8EDBD66B-3289-4535-90EE-77448716C03A"),
                QualificationName = "Higher Education",
                University = "City High School",
                YearsOfCompletion = 2018,
                GradeOrScore = "91.17%"
            };

            _qualification2 = new Qualification()
            {
                Id = new Guid("B1279ECE-EC99-4103-3A90-08DC3E97F46E"),
                UserId = new Guid("8EDBD66B-3289-4535-90EE-77448716C03A"),
                QualificationName = "BE in Civil",
                University = "ABC University",
                YearsOfCompletion = 2022,
                GradeOrScore = "8.5 CGPA"
            };

            _updatedQualification1 = new Qualification()
            {
                Id = new Guid("F32A812A-4190-4F00-3A8F-08DC3E97F46E"),
                UserId = new Guid("8EDBD66B-3289-4535-90EE-77448716C03A"),
                QualificationName = "12th [HSC]",
                University = "City School",
                YearsOfCompletion = 2017,
                GradeOrScore = "93.17%"
            };
        }

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "temp_JobSeeker").Options;
        }

        [Test]
        public async Task GetAllByUserIdAsync_Qualification1And2_ReturnsQulalificationListFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.Qualifications.AddAsync(_qualification1);
                await context.Qualifications.AddAsync(_qualification2);
                await context.SaveChangesAsync();
            }
            Guid userId = new Guid("8EDBD66B-3289-4535-90EE-77448716C03A");
            List<Qualification> qualifications = new List<Qualification>() { _qualification1, _qualification2 };

            // Act 
            List<Qualification> qualificationFromDb;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new QualificationRepository(context);
                qualificationFromDb = await repository.GetAllByUserIdAsync(userId);
            }

            // Assert
            CollectionAssert.AreEqual(qualifications, qualificationFromDb, new QualificationCompare());
        }

        [Test]
        public async Task GetByIdAsync_QualificationExists_ReturnQualificationFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.Qualifications.AddAsync(_qualification1);
                await context.SaveChangesAsync();
            }
            Guid Id = new Guid("F32A812A-4190-4F00-3A8F-08DC3E97F46E");

            // Act 
            Qualification qualificationFromDb;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new QualificationRepository(context);
                qualificationFromDb = await repository.GetByIdAsync(Id);
            }

            // Assert
            qualificationFromDb.Should().BeEquivalentTo(_qualification1);
        }

        [Test]
        public async Task GetByUserIdAsync_QaulificationDoesNotExists_ReturnsNull()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }
            Guid Id = new Guid("8A74A371-5BD2-484D-D653-08DC3E924DC5");

            // Act 
            Qualification qualificationFromDb;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new QualificationRepository(context);
                qualificationFromDb = await repository.GetByIdAsync(Id);
            }

            // Assert
            Assert.That(qualificationFromDb, Is.Null);
        }

        [Test]
        public async Task CreateAsync_AddQualification_CheckThatValueFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new QualificationRepository(context);
                await repository.CreateAsync(_qualification1);
            }

            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                var qualificationFromDb = context.Qualifications.FirstOrDefault(u => u.Id == new Guid("F32A812A-4190-4F00-3A8F-08DC3E97F46E"));
                qualificationFromDb.Should().BeEquivalentTo(_qualification1);
            }
        }

        [Test]
        public async Task UpdateAsync_Qualification1Exists_CheckUpdatedValuesFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.Qualifications.AddAsync(_qualification1);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new QualificationRepository(context);
                await repository.UpdateAsync(_updatedQualification1);
            }

            //assert
            using (var context = new ApplicationDbContext(options))
            {
                var qualificationFromDb = context.Qualifications.FirstOrDefault(u => u.Id == new Guid("F32A812A-4190-4F00-3A8F-08DC3E97F46E"));
                qualificationFromDb.Should().BeEquivalentTo(_updatedQualification1);
                qualificationFromDb.Should().NotBeEquivalentTo(_qualification1);
            }
        }

        [Test]
        public async Task UpdateAsync_QualificationDoesNotExists_ReturnNull()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }

            // Act
            Qualification result;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new QualificationRepository(context);
                result = await repository.UpdateAsync(_updatedQualification1);
            }

            //assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DeleteAsync_DeleteQualification1_CheckQualificationDeletedFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.Qualifications.AddAsync(_qualification1);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new QualificationRepository(context);
                await repository.DeleteAsync(_qualification1);
            }

            //assert
            using (var context = new ApplicationDbContext(options))
            {
                var qualificationFromDb = context.Qualifications.FirstOrDefault(u => u.Id == new Guid("F32A812A-4190-4F00-3A8F-08DC3E97F46E"));
                Assert.That(qualificationFromDb, Is.Null);
            }
        }

        private class QualificationCompare : IComparer
        {
            public int Compare(object x, object y)
            {
                var qualification1 = (Qualification)x;
                var qualification2 = (Qualification)y;
                if (qualification1.Id != qualification2.Id)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
