

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding {

	public static List<PathfindingNode> Find(PathfindingNode start, PathfindingNode end, PathfindingNode[,] map, int width, int height)
	{
		int x, y, cost = 0, step = 0;

		map[end.X, end.Y].Cost = 0; // начало поиска с точки назначения

		if(start.X - 1 >= 0)
		{
			if(map[start.X-1, start.Y].Cost == -2) step++;
		}
		else step++;

		if(start.Y-1 >= 0)
		{
			if(map[start.X, start.Y-1].Cost == -2) step++;
		}
		else step++;

		if(start.X+1 < width)
		{
			if(map[start.X+1, start.Y].Cost == -2) step++;
		}
		else step++;

		if(start.Y+1 < height)
		{
			if(map[start.X, start.Y+1].Cost == -2) step++;
		}
		else step++;

		// проверка на доступность (например, юнит окружен)
		if(step == 4) return null; else step = 0;

		while(true) // цикл поиска
		{
			for(y = 0; y < height; y++)
			{
				for(x = 0; x < width; x++)
				{
					if(map[x, y].Cost == step) // находим клетку, соответствующую текущему шагу
					{
						if(x-1 >= 0) // если не выходим за границы массива
						if(map[x-1, y].Cost == -1) // если клетка еще не проверялась
						{
							cost = step + 1; // сохраняем стоимость клетки
							map[x-1, y].Cost = cost; // назначаем стоимость
						}

						if(y-1 >= 0)
						if(map[x, y-1].Cost == -1)
						{
							cost = step + 1;
							map[x, y-1].Cost = cost;
						}

						if(x+1 < width)
						if(map[x+1, y].Cost == -1)
						{
							cost = step + 1;
							map[x+1, y].Cost = cost;
						}

						if(y+1 < height)
						if(map[x, y+1].Cost == -1)
						{
							cost = step + 1;
							map[x, y+1].Cost = cost;
						}
					}
				}
			}

			step++; // следующий шаг/волна

			if(map[start.X, start.Y].Cost != -1) break; // если путь найден, выходим из цикла
			if(step != cost || step > width * height) return null; // если путь найти невозможно, возврат
		}

		List<PathfindingNode> result = new List<PathfindingNode>(); // массив пути

		// начало поиска со старта
		x = start.X;
		y = start.Y;
        step = map[x, y].Cost; // определяем базовую стоимость

		while(x != end.X || y != end.Y) // прокладка пути
		{
			if(x-1 >= 0 && y-1 >= 0) // если не выходим за границы массива
			if(map[x-1, y-1].Cost >= 0) // если клетка проходима
			if(map[x-1, y-1].Cost < step) // если эта клетка дешевле, базовой стоимости
			{
				step = map[x-1, y-1].Cost; // новая базовая стоимость
				result.Add(map[x-1, y-1]); // добавляем клетку в массив пути
				x--;
				y--;
				continue; // переходим на следующий цикл
			}
			if(y-1 >= 0 && x+1 < width)
			if(map[x+1, y-1].Cost >= 0)
			if(map[x+1, y-1].Cost < step)
			{
				step = map[x+1, y-1].Cost;
				result.Add(map[x+1, y-1]);
				x++;
				y--;
                        continue;
			}

			if(y+1 < height && x+1 < width)
			if(map[x+1, y+1].Cost >= 0)
			if(map[x+1, y+1].Cost < step)
			{
				step = map[x+1, y+1].Cost;
				result.Add(map[x+1, y+1]);
				x++;
				y++;
                continue;
			}

			if(y+1 < height && x-1 >= 0)
			if(map[x-1, y+1].Cost >= 0)
			if(map[x-1, y+1].Cost < step)
			{
				step = map[x-1, y+1].Cost;
				result.Add(map[x-1, y+1]);
				x--;
				y++;
                continue;
			}

			if(x-1 >= 0)
			if(map[x-1, y].Cost >= 0)
			if(map[x-1, y].Cost < step)
			{
				step = map[x-1, y].Cost;
				result.Add(map[x-1, y]);
				x--;
                continue;
			}

			if(y-1 >= 0)
			if(map[x, y-1].Cost >= 0)
			if(map[x, y-1].Cost < step)
			{
				step = map[x, y-1].Cost;
				result.Add(map[x, y-1]);
				y--;
                continue;
			}

			if(x+1 < width)
			if(map[x+1, y].Cost >= 0)
			if(map[x+1, y].Cost < step)
			{
				step = map[x+1, y].Cost;
				result.Add(map[x+1, y]);
				x++;
                continue;
			}

			if(y+1 < height)
			if(map[x, y+1].Cost >= 0)
			if(map[x, y+1].Cost < step)
			{
				step = map[x, y+1].Cost;
				result.Add(map[x, y+1]);
				y++;
                continue;
			}
            return null; // текущая клетка не прошла проверку, ошибка пути, возврат
		}

		return result; // возвращаем найденный маршрут
	}
    public static List<PathfindingNode> GetSurroundNodes(PathfindingNode node, PathfindingNode[,] map, int width, int height)
    {
        List<PathfindingNode> result = new List<PathfindingNode>();
        if (node.X - 1 >= 0 && node.Y - 1 >= 0)
                    result.Add(map[node.X - 1, node.Y - 1]);
        if (node.Y - 1 >= 0 && node.X + 1 < width)
                    result.Add(map[node.X + 1, node.Y - 1]);

        if (node.Y + 1 < height && node.X + 1 < width)
                    result.Add(map[node.X + 1, node.Y + 1]);

        if (node.Y + 1 < height && node.X - 1 >= 0)
                    result.Add(map[node.X - 1, node.Y + 1]);

        if (node.X - 1 >= 0)
                    result.Add(map[node.X - 1, node.Y]);

        if (node.Y - 1 >= 0)
                    result.Add(map[node.X, node.Y - 1]);

        if (node.X + 1 < width)
                    result.Add(map[node.X + 1, node.Y]);

        if (node.Y + 1 < height)
                    result.Add(map[node.X, node.Y + 1]);
        return result;
    }
}
