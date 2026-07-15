namespace Reveal.CommonObjects.Exceptions;

public class RevealException : Exception
{
    public RevealException() { }

    public RevealException(string message) : base(message) { }

    public RevealException(string message, Exception innerException) : base(message, innerException) { }
}
