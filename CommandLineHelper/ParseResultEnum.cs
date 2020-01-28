namespace CommandLineHelper
{
  /// <summary>
  /// This enumeration is used to describe
  /// the result of a parse operation.
  /// </summary>
  public enum ParseResultEnum
  {
    /// <summary>
    ///  Use this value if the parse operation
    ///  didn't take place yet.
    /// </summary>
    NOT_PARSED,

    /// <summary>
    ///  Use this value if the parse operation
    ///  succeeded.
    /// </summary>
    PARSE_SUCCEEDED,

    /// <summary>
    ///  Use this value if the parse operation
    ///  failed for any reason.
    /// </summary>
    PARSE_FAILED
  }

}// END namespace