using System;
using static System.Console;

class SquareMatrix : ICloneable, IComparable<SquareMatrix>
{
    public int size;
    public int[,] dataOfMatrix;

    // Конструктор для инициализации размера матрицы
    public SquareMatrix(int size)
    {
        this.size = size;
        dataOfMatrix = new int[size, size];
    }

    // Конструктор для инициализации матрицы из двумерного массива
    public SquareMatrix(int[,] matrix)
    {
        if (matrix.GetLength(0) != matrix.GetLength(1))
        {
            throw new ArgumentException("Матрица должна быть квадратной.");
        }

        this.size = matrix.GetLength(0);
        this.dataOfMatrix = (int[,])matrix.Clone(); // Глубокая копия массива
    }

    // Метод для заполнения матрицы случайными числами
    public static SquareMatrix MatrixConstructor(int size, Random random)
    {
        SquareMatrix result = new SquareMatrix(size);

        for (int rowIndex = 0; rowIndex < size; ++rowIndex)
        {
            for (int columnIndex = 0; columnIndex < size; ++columnIndex)
            {
                result.dataOfMatrix[rowIndex, columnIndex] = random.Next(0, 10);
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
                Write("{0,5}", dataOfMatrix[rowIndex, columnIndex]);
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
                resultSum += matrix.dataOfMatrix[rowIndex, columnIndex];
            }
        }
        return resultSum;
    }

    // Метод для нахождения детерминанта
    public int Determinant()
    {
        if (size == 1)
        {
            return dataOfMatrix[0, 0];
        }
        if (size == 2)
        {
            return dataOfMatrix[0, 0] * dataOfMatrix[1, 1] - dataOfMatrix[0, 1] * dataOfMatrix[1, 0];
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
                    subMatrix[row - 1, subCol] = dataOfMatrix[row, k];
                    subCol++;
                }
            }
            determinant += (col % 2 == 0 ? 1 : -1) * dataOfMatrix[0, col] * new SquareMatrix(size - 1) { dataOfMatrix = subMatrix }.Determinant();
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
                        minorMatrix.dataOfMatrix[minorRow, minorColumn] = dataOfMatrix[rowDeterminant, columnDeterminant];
                        ++minorColumn;
                    }
                    ++minorRow;
                }
                int cofactor = (int)Math.Pow(-1, rowIndex + columnIndex) * minorMatrix.Determinant();
                inverseMatrix.dataOfMatrix[columnIndex, rowIndex] = cofactor / determinant;
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
                result.dataOfMatrix[rowIndex, columnIndex] = firstMatrix.dataOfMatrix[rowIndex, columnIndex] + secondMatrix.dataOfMatrix[rowIndex, columnIndex];
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
                result.dataOfMatrix[rowIndex, columnIndex] = 0;
                for (int step = 0; step < size; ++step)
                {
                    result.dataOfMatrix[rowIndex, columnIndex] += firstMatrix.dataOfMatrix[rowIndex, step] * secondMatrix.dataOfMatrix[step, columnIndex];
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
        return new SquareMatrix((int[,])dataOfMatrix.Clone());
    }

    // Метод для глубокого копирования
    public SquareMatrix DeepClone()
    {
        return (SquareMatrix)Clone();
    }
}