using System;

// Исключение для вырожденной матрицы
public class SingularMatrixException : Exception
{
    public SingularMatrixException() : base("Матрица вырождена, операция невозможна.") { }
    public SingularMatrixException(string message) : base(message) { }
}