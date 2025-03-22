using System;
using static System.Console;

class SquareMatrix
{
  public int size;
  public int[,] digits;

  // Конструктор для инициализации размера матрицы
  public SquareMatrix(int size)
  {
    this.size = size;
    digits = new int[size, size];
  }

  // Метод для заполнения матрицы случайными числами
  public static SquareMatrix MatrixConstructor(int size, Random random)
  {
    SquareMatrix result = new SquareMatrix(size);

    for (int rowIndex = 0; rowIndex < size; ++rowIndex)
    {
      for (int columnIndex = 0; columnIndex < size; ++columnIndex)
      {
        result.digits[rowIndex, columnIndex] = random.Next(0, 10);
      }
    }

    return result;
  }

  // Метод для вывода матрицы на экран
  public void ToString()
  {
    for (int rowIndex = 0; rowIndex < size; ++rowIndex)
    {
      for (int columnIndex = 0; columnIndex < size; ++columnIndex)
      {
        Write("{0,5}", digits[rowIndex, columnIndex]);
      }
      WriteLine();
    }
  }
}

internal class Program
{
  static void Main(string[] args)
  {
    Write("Укажите размер ваших матриц (до 10): ");
    int sizeMatrix = Convert.ToInt32(Console.ReadLine());

    // Проверка, чтобы размер не превышал 10
    if (sizeMatrix > 10)
    {
      WriteLine("Размер матрицы не должен превышать 10.");
      return;
    }

    // Инициализация матриц с рандомными значениями
    Random random = new Random();
    SquareMatrix firstMatrix = SquareMatrix.MatrixConstructor(sizeMatrix, random);
    SquareMatrix secondMatrix = SquareMatrix.MatrixConstructor(sizeMatrix, random);

    // Демонстрация работы матричного калькулятора
    while (true)
    {
      Clear();
      WriteLine("-----------------------------");
      WriteLine("=== МАТРИЧНЫЙ КАЛЬКУЛЯТОР ===");
      WriteLine("-----------------------------");

      // Вывод матриц и меню на экран
      WriteLine("Первая матрица:");
      firstMatrix.ToString();
      WriteLine("\nВторая матрица:");
      secondMatrix.ToString();
      WriteLine("\nМеню:\n1) Сложение матриц" +
          "\n2) Произведение матриц" +
          "\n3) Операция >" +
          "\n4) Операция <" +
          "\n5) Операция >=" +
          "\n6) Операция <=" +
          "\n7) Операция ==" +
          "\n8) Операция !=" +
          "\n9) true / false" +
          "\n10) The determinant of the matrix" +
          "\n0) Выйти");
      Write("Выберите действие: ");
      int сhoice = Convert.ToInt32(ReadLine());
      if (сhoice == 0) break;
      WriteLine();
    }
    WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
    ReadKey();
  }
}
