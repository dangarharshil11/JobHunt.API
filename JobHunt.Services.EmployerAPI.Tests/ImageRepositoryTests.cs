using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace JobHunt.Services.EmployerAPI.Tests
{
    [TestFixture]
    public class ImageRepositoryTests
    {
        private ImageRepository _imageRepository;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;

        [SetUp]
        public void Setup()
        {
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _webHostEnvironmentMock.Setup(m => m.ContentRootPath).Returns("D:\\JobHunt\\API\\JobHunt\\JobHunt.Services.EmployerAPI.Tests");
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _imageRepository = new ImageRepository(_webHostEnvironmentMock.Object, _httpContextAccessorMock.Object);
        }

        [Test]
        public async Task Upload_WithValidFile_ShouldReturnCompanyLogoDTO()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Returns(Task.CompletedTask);

            var companyLogoDTO = new CompanyLogoDTO
            {
                FileName = "test",
                FileExtension = ".jpg"
            };

            var httpRequestMock = new DefaultHttpContext().Request;
            httpRequestMock.Scheme = "http";
            httpRequestMock.Host = new HostString("example.com");
            httpRequestMock.PathBase = new PathString("/basepath");

            _httpContextAccessorMock.Setup(m => m.HttpContext.Request).Returns(httpRequestMock);

            // Act
            var result = await _imageRepository.Upload(fileMock.Object, companyLogoDTO);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Url, Is.EqualTo("http://example.com/basepath/Images/test.jpg"));
        }
    }
}
