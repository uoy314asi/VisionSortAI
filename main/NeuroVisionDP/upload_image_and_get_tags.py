import sys
import io
import os
import json
import shutil
from gradio_client import Client, file

# Установка стандартного вывода и вывода ошибок на UTF-8
sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding="utf-8")
sys.stderr = io.TextIOWrapper(sys.stderr.buffer, encoding="utf-8")

URL = "https://fbcb1a0c4e38e75e85.gradio.live/"

def upload_image_and_get_tags(image_path):
    """
    Загружает изображение на сервер и получает результаты анализа.

    Аргументы:
        image_path (str): Путь к изображению.

    Возвращает:
        str: Результат анализа изображения.
    """
    print(f"Uploading image: {image_path}")
    client = Client(URL)
    result = client.predict(image=file(image_path), api_name="/predict")
    print(f"Received result: {result}")
    return result


def format_result(result):
    """
    Форматирует результат анализа изображения в список объектов.

    Аргументы:
        result (str): Строка с результатом анализа.

    Возвращает:
        list: Список объектов, найденных на изображении.
    """
    print("Formatting result")
    start_index = result.find("ASSISTANT: [")
    end_index = result.find("]", start_index) + 1
    objects_list = result[start_index + 11 : end_index]
    formatted_result = json.loads(objects_list)
    print(f"Formatted result: {formatted_result}")
    return formatted_result


def sort_image(image_path, tags, base_directory):
    """
    Сортирует изображение в соответствующую директорию на основе тегов.

    Аргументы:
        image_path (str): Путь к изображению.
        tags (list): Список тегов, найденных на изображении.
        base_directory (str): Базовая директория для сортировки изображений.

    Возвращает:
        str: Путь к директории с отсортированными изображениями.
    """
    print(f"Sorting image: {image_path}")
    sorted_images_directory = os.path.join(base_directory, "SortedImages")

    if not os.path.exists(sorted_images_directory):
        os.makedirs(sorted_images_directory)

    new_file_name = "_".join(tags) + os.path.splitext(image_path)[1]
    new_file_path = os.path.join(sorted_images_directory, new_file_name)

    shutil.copy(image_path, new_file_path)
    return sorted_images_directory


if __name__ == "__main__":
    try:
        if len(sys.argv) < 2:
            print(
                "Usage: python upload_image_and_get_tags.py <image_path>",
                file=sys.stderr,
            )
            sys.exit(1)

        image_path = sys.argv[1]
        print(f"Processing image: {image_path}")
        result = upload_image_and_get_tags(image_path)
        tags = format_result(result)
        base_directory = os.path.dirname(os.path.abspath(__file__))
        sorted_directory = sort_image(image_path, tags, base_directory)
        print(f"Image sorted into directories: {tags}")
        print(f"Sorted images can be found in: {sorted_directory}")
    except Exception as e:
        print(f"Error: {str(e)}", file=sys.stderr)
