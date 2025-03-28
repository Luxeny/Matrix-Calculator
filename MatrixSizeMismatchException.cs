using System;

// Исключение для несовпадения размеров матриц
public class MatrixSizeMismatchException : Exception
{
    public MatrixSizeMismatchException() : base("Размеры матриц не совпадают.") { }
    public MatrixSizeMismatchException(string message) : base(message) { }
}