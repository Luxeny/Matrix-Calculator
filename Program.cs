using System;
using static System.Console;

public class Program
{
    static void Main(string[] args)
    {
        Write("Укажите размер ваших матриц (до 10): ");
        int sizeMatrix = Convert.ToInt32(Console.ReadLine());
        if (sizeMatrix > 10)
        {
            WriteLine("Размер матрицы не должен превышать 10.");
            return;
        }

        Random random = new Random();
        SquareMatrix firstMatrix = SquareMatrix.MatrixConstructor(sizeMatrix, random);
        SquareMatrix secondMatrix = SquareMatrix.MatrixConstructor(sizeMatrix, random);

        while (true)
        {
            Clear();
            WriteLine("-----------------------------");
            WriteLine("=== МАТРИЧНЫЙ КАЛЬКУЛЯТОР ===");
            WriteLine("-----------------------------");
            WriteLine("Первая матрица:");
            firstMatrix.ToString();
            WriteLine("\nВторая матрица:");
            secondMatrix.ToString();
            WriteLine("\nМеню:\n1) Операция +" +
                "\n2) Операция *" +
                "\n3) Операция >" +
                "\n4) Операция <" +
                "\n5) Операция >=" +
                "\n6) Операция <=" +
                "\n7) Операция ==" +
                "\n8) Операция !=" +
                "\n9) Операция true / false" +
                "\n10) Найти детерминанты матриц" +
                "\n11) Найти обратные матрицы" +
                "\n12) Сравнения детерминантов" +
                "\n13) Проверка равенства матриц" +
                "\n14) Получить хэш-коды матриц" +
                "\n0) Выйти");
            Write("Выберите действие: ");
            string input = ReadLine();

            if (string.IsNullOrEmpty(input)) continue;

            if (!int.TryParse(input, out int choice)) continue;

            if (choice == 0) break;
            WriteLine();

            try
            {
                switch (choice)
                {
                    case 1:
                        WriteLine("Результат сложения: ");
                        SquareMatrix sumMatrix = firstMatrix + secondMatrix;
                        sumMatrix.ToString();
                        break;
                    case 2:
                        WriteLine("Результат произведения: ");
                        SquareMatrix productMatrix = firstMatrix * secondMatrix;
                        productMatrix.ToString();
                        break;
                    case 3:
                        WriteLine("Результат сравнения: ");
                        WriteLine(firstMatrix > secondMatrix);
                        break;
                    case 4:
                        WriteLine("Результат сравнения: ");
                        WriteLine(firstMatrix < secondMatrix);
                        break;
                    case 5:
                        WriteLine("Результат сравнения: ");
                        WriteLine(firstMatrix >= secondMatrix);
                        break;
                    case 6:
                        WriteLine("Результат сравнения: ");
                        WriteLine(firstMatrix <= secondMatrix);
                        break;
                    case 7:
                        WriteLine("Результат сравнения: ");
                        WriteLine(firstMatrix == secondMatrix);
                        break;
                    case 8:
                        WriteLine("Результат сравнения: ");
                        WriteLine(firstMatrix != secondMatrix);
                        break;
                    case 9:
                        if (firstMatrix)
                        {
                            Console.WriteLine("Первая матрица невырождена (определитель не равен нулю).");
                        }
                        else
                        {
                            Console.WriteLine("Первая матрица вырождена (определитель равен нулю).");
                        }

                        if (secondMatrix)
                        {
                            Console.WriteLine("Вторая матрица невырождена (определитель не равен нулю).");
                        }
                        else
                        {
                            Console.WriteLine("Вторая матрица вырождена (определитель равен нулю).");
                        }
                        break;
                    case 10:
                        WriteLine("Детерминант первой матрицы: " + firstMatrix.Determinant());
                        WriteLine("Детерминант второй матрицы: " + secondMatrix.Determinant());
                        break;
                    case 11:
                        WriteLine("Обратная матрица для первой матрицы:");
                        SquareMatrix firstInverseMatrix = firstMatrix.Inverse();
                        firstInverseMatrix.ToString();

                        WriteLine("Обратная матрица для второй матрицы:");
                        SquareMatrix secondInverseMatrix = secondMatrix.Inverse();
                        secondInverseMatrix.ToString();
                        break;
                    case 12:
                        WriteLine("Результаты сравнения детерминантов через CompareTo:");
                        WriteLine($"Детерминант первой матрицы: {firstMatrix.Determinant()}");
                        WriteLine($"Детерминант второй матрицы: {secondMatrix.Determinant()}");
                        WriteLine(firstMatrix.CompareTo(secondMatrix) > 0 ? "Первая матрица больше." : firstMatrix.CompareTo(secondMatrix) < 0 ? "Первая матрица меньше." : "Матрицы равны.");
                        break;
                    case 13:
                        WriteLine("Результаты проверки равенства матриц через Equals:");
                        WriteLine($"Матрица A: {firstMatrix.Determinant()}");
                        WriteLine($"Матрица B: {secondMatrix.Determinant()}");
                        WriteLine(firstMatrix.Equals(secondMatrix) ? "Матрицы равны." : "Матрицы не равны.");
                        break;
                    case 14:
                        WriteLine($"Хэш-код первой матрицы: {firstMatrix.GetHashCode()}");
                        WriteLine($"Хэш-код второй матрицы: {secondMatrix.GetHashCode()}");
                        break;
                    default:
                        WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }

            catch (Exception ex)
            {
                WriteLine($"Ошибка: {ex.Message}");
            }
            
            WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
            ReadKey();
        }
    }
}
