Тестовое задание
Heroes of Code
Краткое описание
Игра представляет собой ограниченный набор механик из игры Heroes of Might and Magic III. В распоряжении игрока на старте игры имеется одна армия, состоящая из отрядов юнитов одного типа. Игрок может перемещать армию по карте и вступать в бой с вражескими армиями. В игре представлены два основных режима: глобальная карта и экран боя. Между запусками игра не должна хранить изменения, произошедшие в игровом процессе.
Карта
Глобальная карта состоит из квадратных клеток. 
Армия игрока может перемещаться по 8 направлениям.
Клетка может быть проходимой или непроходимой.
Переход хода (дни в HoMMIII) не осуществляется, очков движения нет.

Игрок может управлять движением армии: 
игрок указывает клетку, куда должна переместиться армия
игра отображает маршрут движения
игрок подтверждает проложенный маршрут
армия игрока перемещается в выбранную клетку по маршруту
Во время движения армии по маршруту остановить её или изменить маршрут нельзя.

На карте расположены вражеские армии. Минимум следует расположить три армии. 

Если одна из клеток маршрута армии игрока является соседней с той, на которой расположена вражеская армия, игра переходит в состояние “Бой”, когда армия игрока появляется на этой клетке.

***
Так как переход хода не осуществляется, можно реализовать клетки карты, движением по которым изменяет скорость движения армии (болота - замедляют, дороги - ускоряют).
Армия
Армия - это набор отрядов, состоящих из юнитов одного типа.
Отряд считается уничтоженным, если суммарное здоровье юнитов в отряде меньше, либо равно нулю. В отряде могут быть только юниты одного типа.

Юнит - составляющая отряда. У каждого юнита есть здоровье (максимум и текущее значение в бою), показатель атаки. У некоторых юнитов может быть активный навык, применяемый только в бою.

Необходимо предоставить возможность изменять стартовый состав армии игрока.

Пример расчета: 

Допустим, у юнита Гоблин значение здоровья - 10, а показатель атаки - 1. Значит, в каждом из гоблинских отрядов по 4000 и 2500 хп и отряды наносят по 400 и 250 урона соответственно. В бою первому отряду наносится урон в размере 15 единиц. В таком случае, количество юнитов в нем сокращается до 399, при этом суммарное здоровье отряда становится 3985. При расчете атаки такого отряда наносимый урон будет равен 399. 
Бой
Отряды армии игрока и вражеской армии располагаются напротив друг друга. В отличие от оригинала, перемещать отряды по полю боя нельзя. 

Отряды армий ходят по-очереди. Первый ход - первый отряд игрока, затем первый отряд вражеской армии, потом следующий отряд игрока и так далее.

В свой ход игрок может выбрать цель для атаки текущим отрядом или, если текущий отряд состоит из юнитов с активным навыком, то игрок может применить его.

Цель для атаки “с руки” - любой “живой” вражеский отряд. Цель для активного навыка определяется особенностями навыка.

Бой заканчивается, когда в одной из армий не осталось отрядов.

Вражеские отряды не используют активные навыки.
Активные навыки
Отряд может использовать активный навык только один раз за бой.
Некоторым юнитам нужно назначить следующие активные навыки для применения в бою:
Подлечить выбранный дружественный отряд на процент от максимального значения здоровья юнита, из которого состоит отряд.
Нанести урон выбранному вражескому отряду в размере C + k * “урон, нанесенный отрядом в этом бою”. C - константа, k - некоторый коэффициент.
Нанести фиксированное значение урона всем (как вражеским, так и дружественным) отрядам.

При желании этот обязательный список можно расширить по собственному усмотрению.
Условие победы
Все вражеские армии уничтожены.
Условие поражения
Армия игрока уничтожена.



