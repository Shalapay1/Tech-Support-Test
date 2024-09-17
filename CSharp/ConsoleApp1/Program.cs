using System;
using System.Collections.Generic;

class HedgehogPopulation
{
    static void Main()
    {
        // Введi початкових данних
        Console.WriteLine("Введiть кiлькiсть їжакiв кожного кольору:");
        Console.Write("Червоні (0): ");
        int redCount = int.Parse(Console.ReadLine());

        Console.Write("Зелені (1): ");
        int greenCount = int.Parse(Console.ReadLine());

        Console.Write("Синi (2): ");
        int blueCount = int.Parse(Console.ReadLine());

        int[] hedgehogs = { redCount, greenCount, blueCount };

        Console.Write("Введiть бажаний колiр для всiх їжакiв (0 - червоний, 1 - зелений, 2 - синiй): ");
        int targetColor = int.Parse(Console.ReadLine());

        // Виконання алгоритму i вивiд результату
        int result = MinMeetings(hedgehogs, targetColor);
        if (result == -1)
        {
            Console.WriteLine("Неможливо зробити всiх їжакiв одного кольору.");
        }
        else
        {
            Console.WriteLine($"Мiнiмальне кiлькiсть зустрiч для змiни кольору всiх їжакiв: {result}");
        }
    }

    static int MinMeetings(int[] hedgehogs, int targetColor)
    {
        // Якщо всi їжаки вже одного кольору, повертаємо 0
        if ((hedgehogs[0] == 0 && hedgehogs[1] == 0) ||
            (hedgehogs[0] == 0 && hedgehogs[2] == 0) ||
            (hedgehogs[1] == 0 && hedgehogs[2] == 0))
        {
            return 0;
        }

        // Використовуємо BFS для пошуку мiнiмального кiлькiсть зустрiч
        Queue<(int[] state, int steps)> queue = new Queue<(int[] state, int steps)>();
        HashSet<string> visited = new HashSet<string>();

        // Початкове стан популяцiї їжакiв
        queue.Enqueue((new int[] { hedgehogs[0], hedgehogs[1], hedgehogs[2] }, 0));
        visited.Add(string.Join(",", hedgehogs));

        Console.WriteLine("Починаємо пошук оптимального розв'язку...");

        while (queue.Count > 0)
        {
            var (currentState, steps) = queue.Dequeue();
            Console.WriteLine($"Шаг {steps}: Червоні: {currentState[0]}, Зелені: {currentState[1]}, Синi: {currentState[2]}");

            // Якщо всi їжаки одного кольору
            if (currentState[targetColor] == currentState[0] + currentState[1] + currentState[2])
            {
                return steps;
            }

            // Генеруємо можливi зустрiчи (змiну кольору)
            List<int[]> nextStates = GenerateNextStates(currentState);

            foreach (var nextState in nextStates)
            {
                string nextStateKey = string.Join(",", nextState);
                if (!visited.Contains(nextStateKey))
                {
                    queue.Enqueue((nextState, steps + 1));
                    visited.Add(nextStateKey);  // Додаємо в visited, щоб не повторювати
                }
            }
        }

        // Якщо неможливо достичи цілі
        return -1;
    }

    // Генерація всiх можливих змiн кольору
    static List<int[]> GenerateNextStates(int[] state)
{
    List<int[]> nextStates = new List<int[]>();

    // Зустрiч червоного i зеленого -> обидва синi
    if (state[0] > 0 && state[1] > 0)
    {
        // Копiюємо поточне стан i змiнюємо його для нової зустрiчi
        int[] newState = (int[])state.Clone();
        newState[0] -= 1;
        newState[1] -= 1;
        newState[2] += 2;
        nextStates.Add(newState);
    }

    // Зустрiч червоного i синього -> обидва зеленi
    if (state[0] > 0 && state[2] > 0)
    {
        int[] newState = (int[])state.Clone();
        newState[0] -= 1;
        newState[2] -= 1;
        newState[1] += 2;
        nextStates.Add(newState);
    }

    // Зустрiч зеленого та синього -> обидва червоні
    if (state[1] > 0 && state[2] > 0)
    {
        int[] newState = (int[])state.Clone();
        newState[1] -= 1;
        newState[2] -= 1;
        newState[0] += 2;
        nextStates.Add(newState);
    }

    return nextStates;
}

}
