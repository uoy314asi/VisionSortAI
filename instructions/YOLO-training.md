### Инструкция по обучению модели YOLO

#### Установка и настройка окружения

1. Установите необходимые библиотеки:
    ```sh
    pip install torch torchvision
    pip install yolov5
    ```

2. Клонируйте репозиторий YOLOv5:
    ```sh
    git clone https://github.com/ultralytics/yolov5.git
    cd yolov5
    ```

3. Подготовьте ваш датасет:
    - Создайте структуру папок `dataset/images/train` и `dataset/images/val` для тренировочных и валидационных изображений.
    - Создайте папки `dataset/labels/train` и `dataset/labels/val` для аннотаций в формате YOLO.

4. Создайте конфигурационный файл `dataset.yaml`:
    ```yaml
    train: ./dataset/images/train
    val: ./dataset/images/val

    nc: 1  # Number of classes
    names: ['person']  # Class names
    ```

#### Обучение модели

1. Запустите процесс обучения:
    ```sh
    python train.py --img 416 --batch 16 --epochs 25 --data dataset.yaml --cfg yolov5s.yaml --weights yolov5s.pt
    ```

2. Мониторинг обучения:
    - Во время обучения будут отображаться метрики и графики в консоли.
    - Для более детального анализа используйте TensorBoard:
        ```sh
        tensorboard --logdir runs/train
        ```

#### Оценка модели

1. После завершения обучения модель будет сохранена в папке `runs/train/exp`.
2. Для оценки модели на тестовых данных запустите:
    ```sh
    python val.py --data dataset.yaml --weights runs/train/exp/weights/best.pt --img 416
    ```

---
