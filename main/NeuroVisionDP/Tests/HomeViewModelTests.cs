/*
 * Этот файл содержит тесты для класса HomeViewModel, который управляет коллекцией изображений и их фильтрацией.
 *
 * Что тестируем:
 * 1. FilterImages_ShouldFilterImagesCorrectly:
 *    - Проверяет, что метод FilterImages корректно фильтрует изображения на основе текста поиска.
 * 
 * 2. FilterImages_ShouldReturnAllImagesIfSearchTextIsEmpty:
 *    - Проверяет, что если текст поиска пуст, метод FilterImages возвращает все изображения.
 *
 * Как это работает:
 * - Тестовая среда использует специальный конструктор HomeViewModel, принимающий коллекцию тестовых изображений, чтобы избежать зависимости от реальных файлов изображений.
 * - Тесты проверяют результаты фильтрации, сравнивая их с ожидаемыми значениями.
 * - Это позволяет изолировать логику фильтрации и тестировать ее независимо от фактических файлов изображений и окружения.
 */


using Xunit;
using System.Collections.ObjectModel;
using NeuroVisionDP.MVVM.ViewModel;

namespace NeuroVisionDP.Tests
{
    public class HomeViewModelTests
    {
        private HomeViewModel _viewModel;

        public HomeViewModelTests()
        {
            var testImages = new ObservableCollection<ImageInfo>
            {
                new ImageInfo { FileName = "test_image.jpg" },
                new ImageInfo { FileName = "woman_flower.jpg" },
                new ImageInfo { FileName = "man_eyes.jpg" },
                new ImageInfo { FileName = "cat_purple_light.jpg" }
            };
            _viewModel = new HomeViewModel(testImages);
        }

        [Fact]
        public void FilterImages_ShouldFilterImagesCorrectly()
        {
            // Arrange
            _viewModel.SearchText = "woman";

            // Act
            _viewModel.FilterImages();

            // Assert
            Assert.Equal(1, _viewModel.Images.Count);
            Assert.Equal("woman_flower.jpg", _viewModel.Images[0].FileName);
        }

        [Fact]
        public void FilterImages_ShouldReturnAllImagesIfSearchTextIsEmpty()
        {
            // Arrange
            _viewModel.SearchText = string.Empty;

            // Act
            _viewModel.FilterImages();

            // Assert
            Assert.Equal(4, _viewModel.Images.Count); // Учитывая 4 тестовых изображения
        }
    }
}
