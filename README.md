<h2>Запуск</h2>

1) Клон репозитория : https://github.com/tertotyer/CatsTaskProject/new/master?filename=README.md <br />
2) Запись "The Cat API" пользовательского api ключа в <b>user-secrets</b> : <br/>
Команды:
```
dotnet user-secrets init
```
```
dotnet user-secrets set "x-api-key" "YOUR_API_KEY"
```
<p align="center">
   <img width="856" alt="image_2025-02-14_07-43-29" src="https://github.com/user-attachments/assets/316c0b43-ebc4-454d-b034-4cd6a22f796d" />
</p>

<h2>Описание</h2>
<h4>Работа с API</h4>
Осуществляется с помощью <b>HttpClient</b>. Все запросы к API расположены в классе <b>CatAPIManager</b>;
<h4>Получение списка котов</h4>
- Первоначально загружается 20 пород котов с основной картинкой из API;  <br/>
- При пролистывании списка вниз за 2 итерации пролистывания до последнего элмента отправляется запрос к API на выборку 20 следующих пород;

<h4>Загрузка картинок</h4>
1) Получение от API json; <br/>
2) Десереализция картинки </br>
3) Проверка наличия уже загруженной картинки в папке;
4) При отсутствии идет асинзронная загрузка в папку этой картинки. <br/><br/>

To be continued...
