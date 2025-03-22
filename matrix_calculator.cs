using System;
using static System.Console;

// Исключение для несовпадения размеров матриц
public class MatrixSizeMismatchException : Exception
{
  public MatrixSizeMismatchException() : base("Размеры матриц не совпадают.") { }
  public MatrixSizeMismatchException(string message) : base(message) { }
}

// Исключение для вырожденной матрицы
public class SingularMatrixException : Exception
{
  public SingularMatrixException() : base("Матрица вырождена, операция невозможна.") { }
  public SingularMatrixException(string message) : base(message) { }
}

class SquareMatrix : ICloneable, IComparable<SquareMatrix>
{
  public int size;
  public int[,] digits;

  // Конструктор для инициализации размера матрицы
  public SquareMatrix(int size)
  {
    this.size = size;
    digits = new int[size, size];
  }

  // Конструктор для инициализации матрицы из двумерного массива
  public SquareMatrix(int[,] matrix)
  {
    if (matrix.GetLength(0) != matrix.GetLength(1))
    {
      throw new ArgumentException("Матрица должна быть квадратной.");
    }

    this.size = matrix.GetLength(0);
    this.digits = (int[,])matrix.Clone(); // Глубокая копия массива
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

  // Метод для нахождения суммы элементов матрицы
  public int SumMatrixElements(SquareMatrix matrix)
  {
    int size = matrix.size;
    int resultSum = 0;
    for (int rowIndex = 0; rowIndex < size; ++rowIndex)
    {
      for (int columnIndex = 0; columnIndex < size; ++columnIndex)
      {
        resultSum += matrix.digits[rowIndex, columnIndex];
      }
    }
    return resultSum;
  }

  // Метод для нахождения детерминанта
  public int Determinant()
  {
    if (size == 1)
    {
      return digits[0, 0];
    }
    if (size == 2)
    {
      return digits[0, 0] * digits[1, 1] - digits[0, 1] * digits[1, 0];
    }

    int determinant = 0;
    for (int col = 0; col < size; ++col)
    {
      int[,] subMatrix = new int[size - 1, size - 1];
      for (int row = 1; row < size; ++row)
      {
        int subCol = 0;
        for (int k = 0; k < size; ++k)
        {
          if (k == col) continue;
          subMatrix[row - 1, subCol] = digits[row, k];
          subCol++;
        }
      }
      determinant += (col % 2 == 0 ? 1 : -1) * digits[0, col] * new SquareMatrix(size - 1) { digits = subMatrix }.Determinant();
    }
    return determinant;
  }

  // Неявное приведение к int (детерминанту)
  public static implicit operator int(SquareMatrix matrix)
  {
    return matrix.Determinant();
  }

  // Метод для нахождения обратной матрицы
  public SquareMatrix Inverse()
  {
    int determinant = Determinant();
    if (determinant == 0)
    {
      throw new SingularMatrixException("Матрица вырождена, обратной матрицы не существует.");
    }

    SquareMatrix inverseMatrix = new SquareMatrix(size);

    for (int rowIndex = 0; rowIndex < size; ++rowIndex)
    {
      for (int columnIndex = 0; columnIndex < size; ++columnIndex)
      {
        // Создается минор для элемента (row, column)
        SquareMatrix minorMatrix = new SquareMatrix(size - 1);
        for (int rowDeterminant = 0, minorRow = 0; rowDeterminant < size; ++rowDeterminant)
        {
          if (rowDeterminant == rowIndex)
          {
            continue;
          }
          for (int columnDeterminant = 0, minorColumn = 0; columnDeterminant < size; ++columnDeterminant)
          {
            if (columnDeterminant == columnIndex)
            {
              continue;
            }
            minorMatrix.digits[minorRow, minorColumn] = digits[rowDeterminant, columnDeterminant];
            ++minorColumn;
          }
          ++minorRow;
        }

        // Вычисляем алгебраическое дополнение
        int cofactor = (int)Math.Pow(-1, rowIndex + columnIndex) * minorMatrix.Determinant();

        // Транспонируем и делим на определитель
        inverseMatrix.digits[columnIndex, rowIndex] = cofactor / determinant;
      }
    }
    return inverseMatrix;
  }

  // Перегрузка оператора +
  public static SquareMatrix operator +(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
  {
    if (firstMatrix.size != secondMatrix.size)
    {
      throw new MatrixSizeMismatchException("Матрицы должны быть одного размера для сложения.");
    }

    int size = firstMatrix.size;
    SquareMatrix result = new SquareMatrix(size);
    for (int rowIndex = 0; rowIndex < size; ++rowIndex)
    {
      for (int columnIndex = 0; columnIndex < size; ++columnIndex)
      {
        result.digits[rowIndex, columnIndex] = firstMatrix.digits[rowIndex, columnIndex] + secondMatrix.digits[rowIndex, columnIndex];
      }
    }
    return result;
  }

  // Перегрузка оператора *
  public static SquareMatrix operator *(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
  {
    if (firstMatrix.size != secondMatrix.size)
    {
      throw new MatrixSizeMismatchException("Матрицы должны быть одного размера для умножения.");
    }

    int size = firstMatrix.size;
    SquareMatrix result = new SquareMatrix(size);
    for (int rowIndex = 0; rowIndex < size; ++rowIndex)
    {
      for (int columnIndex = 0; columnIndex < size; ++columnIndex)
      {
        result.digits[rowIndex, columnIndex] = 0;

        // Вычисляет сумму произведений
        for (int step = 0; step < size; ++step)
        {
          result.digits[rowIndex, columnIndex] += firstMatrix.digits[rowIndex, step] * secondMatrix.digits[step, columnIndex];
        }
      }
    }
    return result;
  }

  // Перегрузка оператора >
  public static bool operator >(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
  {
    return firstMatrix.Determinant() > secondMatrix.Determinant();
  }

  // Перегрузка оператора <
  public static bool operator <(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
  {
    return firstMatrix.Determinant() < secondMatrix.Determinant();
  }

  // Перегрузка оператора >=
  public static bool operator >=(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
  {
    return firstMatrix.Determinant() >= secondMatrix.Determinant();
  }

  // Перегрузка оператора <=
  public static bool operator <=(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
  {
    return firstMatrix.Determinant() <= secondMatrix.Determinant();
  }

  // Перегрузка оператора ==
  public static bool operator ==(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
  {
    if (ReferenceEquals(firstMatrix, secondMatrix))
    {
      return true;
    }
    if (ReferenceEquals(firstMatrix, null) || ReferenceEquals(secondMatrix, null))
    {
      return false;
    }
    return firstMatrix.Determinant() == secondMatrix.Determinant();
  }

  // Перегрузка оператора !=
  public static bool operator !=(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
  {
    return !(firstMatrix == secondMatrix);
  }

  // Перегрузка оператора true
  public static bool operator true(SquareMatrix matrix)
  {
    return matrix.Determinant() != 0;
  }

  // Перегрузка оператора false
  public static bool operator false(SquareMatrix matrix)
  {
    return matrix.Determinant() == 0;
  }

  // Реализация интерфейса IComparable<SquareMatrix>
  public int CompareTo(SquareMatrix other)
  {
    if (other == null)
    {
      return 1; // Текущий объект больше null
    }
    return this.Determinant().CompareTo(other.Determinant());
  }

  // Переопределение метода Equals
  public override bool Equals(object obj)
  {
    if (obj is SquareMatrix other)
    {
      return this == other;
    }
    return false;
  }

  // Переопределение метода GetHashCode
  public override int GetHashCode()
  {
    return Determinant().GetHashCode();
  }

  // Реализация интерфейса ICloneable (глубокое копирование)
  public object Clone()
  {
    return new SquareMatrix((int[,])digits.Clone());
  }

  // Метод для глубокого копирования
  public SquareMatrix DeepClone()
  {
    return (SquareMatrix)Clone();
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
      int choice = Convert.ToInt32(ReadLine());
      if (choice == 0) break;
      WriteLine();

      // Выбор действия с матрицами
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
      catch (MatrixSizeMismatchException ex)
      {
        WriteLine($"Ошибка: {ex.Message}");
      }
      catch (SingularMatrixException ex)
      {
        WriteLine($"Ошибка: {ex.Message}");
      }
      catch (Exception ex)
      {
        WriteLine($"Произошла ошибка: {ex.Message}");
      }

      WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
      ReadKey();
    }
  }
}
