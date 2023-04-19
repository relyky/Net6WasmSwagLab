using Microsoft.Extensions.Logging;
namespace Net6WasmSwagLab.DTO;

public class ErrMsg
{
  public ErrMsg()
  {
    Message = String.Empty;
    Severity = LogLevel.None;
  }

  public ErrMsg(string message, LogLevel severity = LogLevel.Error)
  {
    Message = message;
    Severity = severity;
  }

  public string Message { get; set; }

  public LogLevel Severity { get; set; }
}