namespace Sisand.Core.Application.Exceptions;

[System.Serializable]
public class NotFoundException(string message) : System.Exception(message);
