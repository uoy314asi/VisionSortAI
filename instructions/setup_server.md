
### Инструкция по установке и запуску сервера

#### Установка зависимостей

1. Установите необходимые библиотеки:
    ```sh
    pip install gradio
    pip install transformers
    ```

#### Настройка и запуск сервера

1. Создайте Python скрипт для запуска сервера:
    ```python
    import gradio as gr
    from transformers import pipeline
    from PIL import Image
    import os

    def process_input(image=None):
        if image is not None:
            if not os.path.exists('images'):
                os.makedirs('images')
            image_pil = Image.fromarray(image.astype('uint8'), 'RGB')
            image_path = f"images/image_{len(os.listdir('images')) + 1}.png"
            image_pil.save(image_path)
        prompt = 'USER: <image>\nGive me a list of what is located in the image MOST IMPORTANT things. In this format ["obj", "obj2", "obj3", "obj4", "obj5"] Maximum 5 obj without repeating.\nASSISTANT:'
        outputs = pipeline("image-to-text", model="llava")(image_path)
        return outputs[0]["generated_text"]

    iface = gr.Interface(
        fn=process_input,
        inputs=[gr.Image(label="Upload Image")],
        outputs="text",
        title="Image Description with LLaVa",
        description="Upload an image or enter an image URL to get a description."
    )

    iface.launch(share=True, debug=True)
    ```

2. Запустите сервер:
    ```sh
    python server.py
    ```

#### Проверка работоспособности

1. Откройте в браузере ссылку, которую вы получите после запуска сервера.
2. Загрузите изображение и проверьте, что сервер правильно обрабатывает его и возвращает результаты анализа.

Эти инструкции помогут вам настроить и запустить необходимые компоненты для обучения моделей и развертывания сервера. Убедитесь, что все зависимости установлены корректно, а сервер успешно обрабатывает запросы, чтобы обеспечить правильную работу приложения.