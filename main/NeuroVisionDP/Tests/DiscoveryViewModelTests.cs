/*
 * Этот файл содержит тесты для класса DiscoveryViewModel, который управляет загрузкой изображений и взаимодействием с Python скриптами.
 *
 * Что тестируем:
 * 1. UploadImageAsync_ShouldUpdateResultTextWithSuccessMessage:
 *    - Проверяет, что метод UploadImageAsync корректно обновляет текст результата при успешной загрузке изображения.
 * 
 * 2. UploadImageAsync_ShouldUpdateResultTextWithErrorMessageWhenPythonNotFound:
 *    - Проверяет, что метод UploadImageAsync обновляет текст результата с сообщением об ошибке, если Python интерпретатор не найден.
 * 
 * 3. UploadImageAsync_ShouldUpdateResultTextWithErrorMessageWhenScriptNotFound:
 *    - Проверяет, что метод UploadImageAsync обновляет текст результата с сообщением об ошибке, если Python скрипт не найден.
 *
 * Как это работает:
 * - Тесты используют фиктивные пути для Python и скрипта, чтобы симулировать различные сценарии и проверить обработку ошибок.
 * - Тесты создают фиктивные файлы изображений для проверки загрузки и обработки изображений.
 * - Это позволяет изолировать логику загрузки изображений и взаимодействия с Python скриптами и тестировать их независимо от фактических файлов и окружения.
 */
using Xunit;
using Moq;
using System.IO;
using System.Threading.Tasks;
using NeuroVisionDP.MVVM.ViewModel;
using System.Text;

namespace NeuroVisionDP.Tests
{
    public class DiscoveryViewModelTests
    {
        private DiscoveryViewModel _viewModel;
        private Mock<HomeViewModel> _mockHomeViewModel;

        public DiscoveryViewModelTests()
        {
            _mockHomeViewModel = new Mock<HomeViewModel>();
            _viewModel = new DiscoveryViewModel(_mockHomeViewModel.Object);
        }

        [Fact]
        public async Task UploadImageAsync_ShouldUpdateResultTextWithSuccessMessage()
        {
            // Arrange
            var testFilePath = "test_image.jpg";
            CreateDummyFile(testFilePath);

            // Подмена путей для тестирования
            _viewModel.PythonPath = Path.Combine(Directory.GetCurrentDirectory(), "valid_python.exe");
            _viewModel.ScriptPath = Path.Combine(Directory.GetCurrentDirectory(), "valid_script.py");

            // Mock expected result
            string expectedResult = "Image successfully uploaded and sorted.";

            // Act
            string result = await Task.Run(() => _viewModel.UploadImageAndGetTags(testFilePath));

            // Assert
            Assert.Equal(expectedResult, result);

            // Clean up
            File.Delete(testFilePath);
        }

        [Fact]
        public async Task UploadImageAsync_ShouldUpdateResultTextWithErrorMessageWhenPythonNotFound()
        {
            // Arrange
            var testFilePath = "test_image.jpg";
            CreateDummyFile(testFilePath);
            var expectedError = $"Error: Python interpreter not found at {Path.Combine(Directory.GetCurrentDirectory(), "invalid_python.exe")}";

            // Setup PythonPath to an invalid path for test simulation
            _viewModel.PythonPath = Path.Combine(Directory.GetCurrentDirectory(), "invalid_python.exe");

            // Act
            string result = await Task.Run(() => _viewModel.UploadImageAndGetTags(testFilePath));

            // Assert
            Assert.Equal(expectedError, result);

            // Clean up
            File.Delete(testFilePath);
        }

        [Fact]
        public async Task UploadImageAsync_ShouldUpdateResultTextWithErrorMessageWhenScriptNotFound()
        {
            // Arrange
            var testFilePath = "test_image.jpg";
            CreateDummyFile(testFilePath);
            var expectedError = $"Error: Python script not found at {Path.Combine(Directory.GetCurrentDirectory(), "invalid_script.py")}";

            // Setup ScriptPath to an invalid path for test simulation
            _viewModel.ScriptPath = Path.Combine(Directory.GetCurrentDirectory(), "invalid_script.py");

            // Act
            string result = await Task.Run(() => _viewModel.UploadImageAndGetTags(testFilePath));

            // Assert
            Assert.Equal(expectedError, result);

            // Clean up
            File.Delete(testFilePath);
        }

        private void CreateDummyFile(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                var info = new UTF8Encoding(true).GetBytes("This is a test image.");
                fs.Write(info, 0, info.Length);
            }
        }
    }
}
