
### Инструкция по обучению модели LLaVa

#### Установка и настройка окружения

1. Проверьте наличие GPU:
    ```sh
    !nvidia-smi
    ```

2. Установите необходимые библиотеки:
    ```sh
    pip install -U transformers peft bitsandbytes
    pip install -U trl
    ```

#### Загрузка предварительно обученной модели

1. Задайте идентификатор модели:
    ```python
    model_id = "4bit/llava-v1.5-7b-5GB"
    ```

2. Настройте конфигурацию квантования:
    ```python
    from transformers import BitsAndBytesConfig, LlavaForConditionalGeneration
    quantization_config = BitsAndBytesConfig()
    model = LlavaForConditionalGeneration.from_pretrained(
        model_id,
        quantization_config=quantization_config,
        torch_dtype=torch.float16
    )
    ```

#### Подготовка данных и обучение

1. Создайте шаблон чата:
    ```python
    LLAVA_CHAT_TEMPLATE = """A chat between a curious user and an artificial intelligence assistant. The assistant gives helpful, detailed, and polite answers to the user's questions. {% for message in messages %}{% if message['role'] == 'user' %}USER: {% else %}ASSISTANT: {% endif %}{% for item in message['content'] %}{% if item['type'] == 'text' %}{{ item['text'] }}{% elif item['type'] == 'image' %}<image>{% endif %}{% endfor %}{% if message['role'] == 'user' %} {% else %}{{eos_token}}{% endif %}{% endfor %}"""
    tokenizer.chat_template = LLAVA_CHAT_TEMPLATE
    ```

2. Загрузите датасет и выберите 1% данных для обучения и тестирования:
    ```python
    from datasets import load_dataset
    raw_datasets = load_dataset("HuggingFaceH4/llava-instruct-mix-vsft")
    one_percent_train = int(0.01 * len(raw_datasets['train']))
    one_percent_eval = int(0.01 * len(raw_datasets['test']))
    train_dataset = raw_datasets['train'].select(range(one_percent_train))
    eval_dataset = raw_datasets['test'].select(range(one_percent_eval))
    ```

3. Настройте параметры обучения:
    ```python
    from transformers import TrainingArguments
    training_args = TrainingArguments(
        output_dir="llava-1.5-7b-hf-ft-mix-vsft",
        report_to="tensorboard",
        learning_rate=1.4e-5,
        per_device_train_batch_size=4,
        gradient_accumulation_steps=1,
        logging_steps=5,
        num_train_epochs=1,
        push_to_hub=True,
        gradient_checkpointing=True,
        remove_unused_columns=False,
        fp16=True,
        bf16=False
    )
    ```

4. Настройте конфигурацию LoRA:
    ```python
    from peft import LoraConfig
    lora_config = LoraConfig(
        r=64,
        lora_alpha=16,
        target_modules="all-linear"
    )
    ```

5. Инициализируйте тренера и начните обучение:
    ```python
    from trl import SFTTrainer
    trainer = SFTTrainer(
        model=model,
        args=training_args,
        train_dataset=train_dataset,
        eval_dataset=eval_dataset,
        peft_config=lora_config,
        dataset_text_field="text",
        tokenizer=tokenizer,
        data_collator=data_collator,
        dataset_kwargs={"skip_prepare_dataset": True}
    )
    trainer.train()
    trainer.push_to_hub()
    ```

---
