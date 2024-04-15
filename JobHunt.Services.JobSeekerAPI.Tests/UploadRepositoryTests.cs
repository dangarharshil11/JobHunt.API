using JobHunt.Services.JobSeekerAPI.Models.Dto;
using JobHunt.Services.JobSeekerAPI.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace JobHunt.Services.JobSeekerAPI.Tests
{
    [TestFixture]
    public class UploadRepositoryTests
    {
        private UploadRepository _uploadRepository;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;

        [SetUp]
        public void Setup()
        {
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _webHostEnvironmentMock.Setup(m => m.ContentRootPath).Returns("D:\\JobHunt\\API\\JobHunt\\JobHunt.Services.JobSeekerAPI.Tests\\StaticFiles\\");
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _uploadRepository = new UploadRepository(_webHostEnvironmentMock.Object, _httpContextAccessorMock.Object);
        }

        [Test]
        public async Task UploadResume_WithValidFile_ShouldReturnResumeUrl()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Returns(Task.CompletedTask);

            var uploadDto = new UploadDto
            {
                FileName = "testresume",
                FileExtension = ".pdf"
            };

            var httpRequestMock = new DefaultHttpContext().Request;
            httpRequestMock.Scheme = "http";
            httpRequestMock.Host = new HostString("example.com");
            httpRequestMock.PathBase = new PathString("/basepath");

            _httpContextAccessorMock.Setup(m => m.HttpContext.Request).Returns(httpRequestMock);

            // Act
            var result = await _uploadRepository.UploadResume(fileMock.Object, uploadDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Url, Is.EqualTo("http://example.com/basepath/Resumes/testresume.pdf"));
        }

        [Test]
        public async Task UploadImage_WithValidFile_ShouldReturnImageUrl()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Returns(Task.CompletedTask);

            var uploadDto = new UploadDto
            {
                FileName = "testimage",
                FileExtension = ".jpg"
            };

            var httpRequestMock = new DefaultHttpContext().Request;
            httpRequestMock.Scheme = "http";
            httpRequestMock.Host = new HostString("example.com");
            httpRequestMock.PathBase = new PathString("/basepath");

            _httpContextAccessorMock.Setup(m => m.HttpContext.Request).Returns(httpRequestMock);

            // Act
            var result = await _uploadRepository.UploadImage(fileMock.Object, uploadDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Url, Is.EqualTo("http://example.com/basepath/Images/testimage.jpg"));
        }
    }
}
